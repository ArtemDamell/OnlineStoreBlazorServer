using BlazorShop.Data.Models;
using System.Collections.Generic;

namespace BlazorShop.Data.DTOs
{
    public class ProductsAndPrice
    {
        public List<Product> Products { get; set; } = new();
        public double TotalPrice { get; set; }
    }
}
