using System.ComponentModel.DataAnnotations;

namespace BlazorShop.Data.Models
{
    public class SpecialTag
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2), MaxLength(25)]
        public string Name { get; set; }
    }
}
