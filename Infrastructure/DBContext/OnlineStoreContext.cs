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

        public DbSet<Forum> Forum { get; set; }

        public OnlineStoreContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultContainer("OnlineStoreContainer");

            modelBuilder.Entity<Order>()
                .ToContainer(nameof(Orders))
                .HasNoDiscriminator() 
                .HasPartitionKey(d => d.PartitionKey)
                .UseETagConcurrency();

            modelBuilder.Entity<User>()
                .ToContainer(nameof(Users))
                .HasNoDiscriminator()
                .HasPartitionKey(u => u.PartitionKey)
                .UseETagConcurrency();

            modelBuilder.Entity<Product>()
                .ToContainer(nameof(Products))
                .HasNoDiscriminator()
                .HasPartitionKey(p => p.PartitionKey)
                .UseETagConcurrency();

            modelBuilder.Entity<Forum>()
                .ToContainer(nameof(Forum))
                .HasNoDiscriminator()
                .HasPartitionKey(f => f.PartitionKey)
                .UseETagConcurrency();
        }
    }
}
