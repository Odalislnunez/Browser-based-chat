using Browser_based_chat.Areas.Identity.Data;
using Browser_based_chat.Hubs;
using Browser_based_chat.Models;
using Browser_based_chat.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Diagnostics;

namespace Browser_based_chat.Controllers
{
    [Authorize]
    public class RoomChatController : Controller
    {
        private readonly ILogger<RoomChatController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IStockQBotService _stockQBotService;
        private readonly IHubContext<ChatHub> _hubContext;
        public UserManager<User> _userManager;

        public RoomChatController(ILogger<RoomChatController> logger, ApplicationDbContext applicationDbContext, IStockQBotService stockQBotService, IHubContext<ChatHub> hubContext, UserManager<User> userManager)
        {
            _logger = logger;
            _dbContext = applicationDbContext;
            _stockQBotService = stockQBotService;
            _hubContext = hubContext;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var rooms = _dbContext.Rooms.Where(x => x.Status).ToList();

            if (rooms != null)
                return View(rooms);
            else
                return NotFound();
        }

        public IActionResult Details(int roomId)
        {
            var roomChats = _dbContext.RoomChats.Where(x => x.roomID == roomId).ToList();
            var room = _dbContext.Rooms.Where(x => x.ID == roomId).FirstOrDefault();

            ViewBag.RoomID = roomId;
            ViewBag.RoomName = room.Description;

            return View(roomChats);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] RoomCreateModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.email);
            var room = _dbContext.Rooms.Where(x => x.ID == Convert.ToInt32(model.roomId)).FirstOrDefault();

            if(user != null && user.Id == room.UserId)
            {
                room.Status = !room.Status;

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest("You cannot delete a room which you haven't created.");
            }

            return Json("");
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RoomCreateModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.description.Trim()))
                {
                    var user = await _userManager.FindByEmailAsync(model.email);

                    var lastRoom = _dbContext.Rooms.OrderByDescending(x => x.ID).FirstOrDefault();

                    Room room;

                    if (lastRoom != null)
                        room = new Room(lastRoom.ID + 1, model.description, user.Id);
                    else
                        room = new Room(1, model.description, user.Id);

                    _dbContext.Rooms.Add(room);

                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Json("");
        }

        [HttpPost]
        public async Task<IActionResult> GetStockQuote([FromBody] StockModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.email);

            await _hubContext.Clients.Group(model.roomId).SendAsync("ReceiveMessageCommand", $"{user.FirstName} {user.LastName}", model.msg, DateTime.Now.ToString("G"));

            var messagesCount = _dbContext.RoomChats.Where(x => x.roomID == Convert.ToInt32(model.roomId)).Count();
            
            var stockCode = model.msg.Replace("/stock=", "");

            try
            {
                var quoteResult = await _stockQBotService.GetStockQuoteAsync(stockCode);
                if (quoteResult is JsonResult objectResult && objectResult.Value != null)
                {
                    var quote = objectResult.Value.ToString();

                    await _hubContext.Clients.Group(model.roomId).SendAsync("ReceiveMessageCommand", "Stock Quote Bot", quote, DateTime.Now.ToString("G"), messagesCount);
                }
                else
                {
                    await _hubContext.Clients.Group(model.roomId).SendAsync("ReceiveMessageCommand", "Stock Quote Bot", "No data received", DateTime.Now.ToString("G"), messagesCount);
                }
            }
            catch (Exception ex)
            {
                await _hubContext.Clients.Group(model.roomId).SendAsync("ReceiveMessageCommand", "Stock Quote Bot", ex.Message, DateTime.Now.ToString("G"), messagesCount);
                throw new Exception(ex.Message);
            }

            return Ok();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
