using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TaskFlow.Api.Data;
using TaskFlow.Api.Dtos;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Tests
{
    public class TaskServiceTests
    {
        [Fact]
        internal async Task UserOnlySeeTheirOwnTasks()
        {
            // Ensures the test database context is disposed asynchronously after the test.
            await using TaskFlowDbContext dbContext = CreateDbContext();

            // Arrange
            TaskService service = new(dbContext, NullLogger<TaskService>.Instance);

            await service.CreateAsync(1, new TaskCreateDto { Title = "User one task" });
            await service.CreateAsync(2, new TaskCreateDto { Title = "User two task" });

            // Act
            var userOneTasks = await service.GetAllAsync(1);

            // Assert
            Assert.Single(userOneTasks);
            Assert.Equal("User one task", userOneTasks[0].Title);
        }

        private static TaskFlowDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<TaskFlowDbContext>()
                            .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;  // fast isolated database for testing.

            return new TaskFlowDbContext(options);
        }
    }
}
