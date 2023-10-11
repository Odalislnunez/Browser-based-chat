using Browser_based_chat.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Browser_based_chat.Areas.Identity.Data;

public class User : IdentityUser
{ 
    public string FirstName { get; set; }     
    public string LastName { get; set; }     

    public ICollection<RoomChat> roomChats { get; set; }
}

