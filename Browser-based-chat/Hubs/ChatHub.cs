using Browser_based_chat.Areas.Identity.Data;
using Browser_based_chat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Browser_based_chat.Hubs
{
    public class ChatHub : Hub
    {
        public ApplicationDbContext _dbcontext;
        public UserManager<User> _userManager;

        public ChatHub(ApplicationDbContext applicationDbContext,UserManager<User> userManager)
        {
            _dbcontext = applicationDbContext;
            _userManager = userManager;
        }
        public async Task SendMessage(string msg, string roomId, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var time = DateTime.UtcNow;

            var roomChat = new RoomChat
            {
                userID = user.Id,
                roomID = Convert.ToInt32(roomId),
                message = msg,
                date = time
            };

            _dbcontext.RoomChats.Add(roomChat);

            await Clients.Group(roomId).SendAsync("ReceiveMessage", user.FirstName + " " + user.LastName, msg, time);
        }

        public Task JoinGroup(string roomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }
    }
}
