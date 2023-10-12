using Browser_based_chat.Areas.Identity.Data;
using Browser_based_chat.Hubs;
using Browser_based_chat.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;

namespace Browser_based_chat.Controllers
{
    [Authorize]
    public class RoomChatController : Controller
    {
        private readonly ILogger<RoomChatController> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHubContext<ChatHub> _hubContext;

        public RoomChatController(ILogger<RoomChatController> logger, ApplicationDbContext applicationDbContext, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _dbContext = applicationDbContext;
            _hubContext = hubContext;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
