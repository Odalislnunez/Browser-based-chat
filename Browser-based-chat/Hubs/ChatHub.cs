using Browser_based_chat.Areas.Identity.Data;
using Browser_based_chat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
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

            await _dbcontext.SaveChangesAsync();

            await Clients.Group(roomId).SendAsync("ReceiveMessage", user.FirstName + " " + user.LastName, msg, time);
        }

        public Task JoinGroup(string roomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task GetRoomChats(string roomId)
        {
            var roomChats = _dbcontext.RoomChats.Include(x => x.user).Where(x => x.roomID == Convert.ToInt32(roomId)).OrderByDescending(x => x.date).Take(50).ToList();

            foreach(var roomChat in roomChats)
            {
                await Clients.Group(roomId).SendAsync("ReceiveMessage", roomChat.user.FirstName + " " + roomChat.user.LastName, roomChat.message, roomChat.date);
            }
        }
    }
}
