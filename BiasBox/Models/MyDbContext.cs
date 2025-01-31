using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiasBox.Models
{
    internal class MyDbContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShippingMethod> ShippingMethods { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=BiasBox;Trusted_Connection=True; TrustServerCertificate=True;");

            optionsBuilder.UseSqlServer("Server=tcp:feliciadb.database.windows.net,1433;Initial Catalog=FeliciaDb;Persist Security Info=False;User ID=Felicia;Password=BananKorv1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
    }
    
}
