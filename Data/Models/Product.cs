using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorShop.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2), MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(1, 1000000)]
        public double Price { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; } = 1;

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        [Required]
        public int SpecialTagId { get; set; }

        [ForeignKey(nameof(SpecialTagId))]
        public SpecialTag SpecialTag { get; set; }

        
    }
}
