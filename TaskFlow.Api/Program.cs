
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Services;

namespace TaskFlow.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Register the dbContext to connect to the SQL Server database.
            // DI manages the creation and delivery of TaskFlowDbContext to classes that depend on it.
            builder.Services.AddDbContext<TaskFlowDbContext>(optionsBuilder => optionsBuilder.UseSqlServer(
                                                                builder.Configuration.GetConnectionString("TaskFlowDatabase")));

            // Register TaskService as the implementation for ITaskService with scoped lifetime.                                                                
            builder.Services.AddScoped<ITaskService, TaskService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
