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
    }
}
