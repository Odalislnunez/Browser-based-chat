using Browser_based_chat.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Browser_based_chat.Models
{
    public class Room
    {
        public Room()
        {
            
        }
        public Room(int iD, string description, string userId)
        {
            ID = iD;
            Description = description;
            UserId = userId;
        }

        [Key]
        public int ID { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; } = true;
        public string? UserId { get; set; }

        public ICollection<RoomChat> roomChats { get; set; }   
    }
}
