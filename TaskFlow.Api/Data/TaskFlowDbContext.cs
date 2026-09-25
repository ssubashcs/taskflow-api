using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Data
{
    /// <summary>
    /// <see cref="TaskFlowDbContext"/> represents a database session. 
    /// <see cref="DbContext"/> provides the ability for the application to interact with the database.
    /// </summary>
    public class TaskFlowDbContext : DbContext
    {
        public TaskFlowDbContext(DbContextOptions<TaskFlowDbContext> options) : base(options)
        {
            
        }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        public DbSet<User> Users => Set<User>();

        // Configure entity-to-database mappings, constraints, relationships, and other schema rules
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                // Configure a unique database index on the Email property
                entity.HasIndex(user => user.Email).IsUnique();

                entity.Property(user => user.Email).HasMaxLength(256).IsRequired();

                entity.Property(user => user.PasswordHash).IsRequired();
            });
        }
    }
}
