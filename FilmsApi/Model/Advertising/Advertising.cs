using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmsApi.Model.Advertising
{
    [Table("Advertisings")]
    public class Advertising
    {
        [Key] public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string WebUrl { get; set; }
    }
}
