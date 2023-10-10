using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using TestTaskUser.Enums;
using TestTaskUser.Models;

namespace TestTaskUser.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
            .Property(x => x.UserRoleId)
            .HasConversion<int>();

            modelBuilder.Entity<Role>()
                .HasData(
                    Enum.GetValues(typeof(UserRole))
                        .Cast<UserRole>()
                        .Select(e => new Role()
                        {
                            UserRoleId = e,
                            Name = e.ToString()
                        })
                );
        }
    }
}
