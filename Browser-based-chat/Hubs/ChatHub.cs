using Browser_based_chat.Areas.Identity.Data;
using Browser_based_chat.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

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

            var roomChat = new RoomChat
            {
                userID = user.Id,
                roomID = Convert.ToInt32(roomId),
                message = msg,
                date = DateTime.Now
            };

            _dbcontext.RoomChats.Add(roomChat);

            await _dbcontext.SaveChangesAsync();

            await GetRoomChats(roomId);
        }

        public Task JoinGroup(string roomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task GetRoomChats(string roomId)
        {
            var roomChats = await _dbcontext.RoomChats
                .Include(x => x.user)
                .Where(x => x.roomID == Convert.ToInt32(roomId))
                .OrderByDescending(x => x.date)
                .Take(50)
                .Select(x => new
                {
                    user = x.user.FirstName + " " + x.user.LastName,
                    msg = x.message,
                    time = x.date.ToString("G")
                })
                .ToListAsync();

            await Clients.Group(roomId).SendAsync("ReceiveMessage", roomChats);
        }
    }
}
