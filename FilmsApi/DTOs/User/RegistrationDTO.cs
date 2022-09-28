using System.ComponentModel.DataAnnotations;

namespace FilmsApi.DTOs.User
{
    public class RegistrationDTO
    {
        [Required] public string Username { get; set; }
        [Required] public string Login { get; set; }
        [Required] public string Password { get; set; }
    }
}
