using System;
using System.ComponentModel.DataAnnotations;

namespace FilmsApi.DTOs.User
{
    public class UserDTO
    {
        [Key] public int Id { get; set; }
        [Required] public string Username { get; set; }
        [Required] public string Login { get; set; }
        [Required] public Boolean Subscription { get; set; }
    }
}
