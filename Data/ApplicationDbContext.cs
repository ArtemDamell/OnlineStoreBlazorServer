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

        // Урок 1 (4)
        public DbSet<Category> Categories { get; set; }
        public DbSet<SpecialTag> SpecialTags { get; set; }
        // Урок 4 (2)
        public DbSet<Product> Products { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        // Урок 9 (6)
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // Урок 12/2 (3)
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<PaymentDetails> PaymentDetails { get; set; }
    }
}
