using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmsApi.Model.User
{

    [Table("Users")]
    public class User
    {
        [Key] public int Id { get; set; }
        [Required] public string Username { get; set; }
        [Required] public string Login { get; set; }
        [Required] public string Password { get; set; }
        [Required] public Boolean Subscription { get; set; }

        public virtual string Role => "BASE_USER";
    }
}
