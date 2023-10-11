using Browser_based_chat.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Browser_based_chat.Models
{
    public class RoomChat
    {
        [Key]
        public int ID { get; set; }
        public string userID { get; set; }
        public int roomID { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }

        public User user { get; set; }
        public Room room { get; set; }
    }
}
