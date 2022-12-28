using CosmeticWeb.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CosmeticWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Blog>? Blogs { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Wishlist>? Wishlists { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<ContactUs>? ContactUs { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<Subscribe>? Subscribtions { get; set; }
        public DbSet<Testimonial>? Testimonials { get; set; }
    }
}