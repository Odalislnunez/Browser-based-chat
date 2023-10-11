using Browser_based_chat.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Browser_based_chat.Controllers
{
    public class RoomChatController : Controller
    {
        private readonly ILogger<RoomChatController> _logger;

        public RoomChatController(ILogger<RoomChatController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
