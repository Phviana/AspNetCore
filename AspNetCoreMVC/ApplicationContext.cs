using AspNetCoreMVC.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMVC
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasKey(t => t.Id);

            modelBuilder.Entity<Product>().HasKey(t => t.Id);

            modelBuilder.Entity<Order>().HasKey(t => t.Id);
            modelBuilder.Entity<Order>().HasMany(t => t.Itens)
                .WithOne(t => t.Order);
            modelBuilder.Entity<Order>().HasOne(t => t.Register)
                .WithOne(t => t.Order)
                .IsRequired();

            modelBuilder.Entity<OrderItem>().HasKey(t => t.Id);
            modelBuilder.Entity<OrderItem>().HasOne(t => t.Order);
            modelBuilder.Entity<OrderItem>().HasOne(t => t.Order);

            modelBuilder.Entity<Register>().HasKey(t => t.Id);

        }
    }
}
