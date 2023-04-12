using BlazorShop.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SpecialTag> SpecialTags { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
    }
}
