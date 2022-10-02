using System.ComponentModel.DataAnnotations;

namespace FilmsApi.Model
{
    public class Freekassa
    {
        [Key] public int Id { get; set; }
        [Required] public int ShopId { get; set; }
        [Required] public string SecretWordOne { get; set; }
        [Required] public string SecretWordTwo { get; set; }
        [Required] public string CashKey { get; set; }
    }
}
