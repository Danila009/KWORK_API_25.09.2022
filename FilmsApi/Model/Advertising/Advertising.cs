using System.ComponentModel.DataAnnotations;

namespace FilmsApi.Model.Advertising
{
    public class Advertising
    {
        [Key] public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string WebUrl { get; set; }
    }
}
