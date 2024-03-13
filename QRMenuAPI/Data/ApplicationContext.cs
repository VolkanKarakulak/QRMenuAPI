using System;
using Microsoft.Extensions.Options;
using QRMenuAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace QRMenuAPI.Data
{
        public class ApplicationContext : IdentityDbContext<ApplicationUser>
        {

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
            {}

        public DbSet<Company>? Companies { get; set; }
        public DbSet<State>? States { get; set; }
        public DbSet<Restaurant>? Restaurant { get; set; }
        public DbSet<Food>? Foods { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<RestaurantUser>? RestaurantUsers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>().HasOne(u => u.State).WithMany().OnDelete(DeleteBehavior.NoAction);
          
            modelBuilder.Entity<Restaurant>().HasOne(u => u.State).WithMany().OnDelete(DeleteBehavior.NoAction);
    
            modelBuilder.Entity<Food>().HasOne(u => u.State).WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Category>().HasOne(u => u.State).WithMany().OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<RestaurantUser>().HasOne(u => u.Restaurant).WithMany().OnDelete(DeleteBehavior.NoAction);
     
            modelBuilder.Entity<RestaurantUser>().HasKey(r => new { r.UserId, r.RestaurantId });

            
        }

    }
}

