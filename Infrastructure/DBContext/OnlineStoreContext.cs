using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure
{
    public class OnlineStoreContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Forum> Forums { get; set; }

        public OnlineStoreContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        private void ConfigureModel<TEntity>(ModelBuilder modelBuilder, string containerName) where TEntity : class
        {
            modelBuilder.Entity<TEntity>()
                .ToContainer(containerName)
                .HasNoDiscriminator()
                .UseETagConcurrency();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("OnlineStoreContainer");

            // Order
            ConfigureModel<Order>(modelBuilder, nameof(Orders));
            modelBuilder.Entity<Order>()
                .HasPartitionKey(o => o.PartitionKey);

            // User
            ConfigureModel<User>(modelBuilder, nameof(Users));
            modelBuilder.Entity<User>()
                .HasPartitionKey(u => u.PartitionKey);

            // Product
            ConfigureModel<Product>(modelBuilder, nameof(Products));
            modelBuilder.Entity<Product>()
                .HasPartitionKey(p => p.PartitionKey);

            // Forum
            ConfigureModel<Forum>(modelBuilder, nameof(Forums));
            modelBuilder.Entity<Forum>()
                .HasPartitionKey(f => f.PartitionKey);
        }
    }
}
