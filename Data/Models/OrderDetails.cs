using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorShop.Data.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; }
        public List<OrderModel> Orders { get; set; } = new();
    }
}
