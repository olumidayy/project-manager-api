using System;
using Microsoft.EntityFrameworkCore;
using ProjectManager.ApplicationCore.Entities;

namespace ProjectManager.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }

        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Project>()
                .Property(e => e.Technologies)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
                );
        }
    }
}