using System.ComponentModel.DataAnnotations;

namespace Browser_based_chat.Models
{
    public class Room
    {
        public Room()
        {
            
        }
        public Room(int iD, string description)
        {
            ID = iD;
            Description = description;
        }

        [Key]
        public int ID { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; } = true;

        public ICollection<RoomChat> roomChats { get; set; }   
    }
}
