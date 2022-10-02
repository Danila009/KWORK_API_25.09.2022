using System.ComponentModel.DataAnnotations.Schema;

namespace FilmsApi.Model.User
{
    [Table("AdminUsers")]
    public class AdminUser : User
    {
        public override string Role => "ADMIN_USER";

    }
}
