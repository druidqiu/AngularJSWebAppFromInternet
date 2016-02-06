using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using AngularJSDataModels;

namespace AngularJSDataService
{
    public class AngularJSDatabase : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ApplicationMenu> ApplicationMenuItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Shipper> Shippers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("dbo.Users");
            modelBuilder.Entity<ApplicationMenu>().ToTable("dbo.ApplicationMenu");
            modelBuilder.Entity<Customer>().ToTable("dbo.Customers");
            modelBuilder.Entity<Product>().ToTable("dbo.Products");
            modelBuilder.Entity<Order>().ToTable("dbo.Orders");
            modelBuilder.Entity<OrderDetail>().ToTable("dbo.OrderDetails");
            modelBuilder.Entity<Shipper>().ToTable("dbo.Shippers");  
           

        }
    }

}
