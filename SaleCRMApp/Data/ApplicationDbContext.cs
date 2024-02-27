using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaleCRMApp.Models;
using SwadeshiApp.DTO;
using SwadeshiApp.Models;

namespace SaleCRMApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<UserEntity> user { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }

        public DbSet<Cart> CartItem { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
      
        public DbSet<Product> Product { get; set; }
        public DbSet<Review> Review { get; set; }
        public DbSet<Wishlist> Wishlist { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<State> States { get; set; }

        public DbSet<UserProfile> users { get; set; }
    }
}
