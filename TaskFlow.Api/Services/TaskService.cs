using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Dtos;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskFlowDbContext _dbContext;

        private readonly ILogger<TaskService> _logger;

        public TaskService(TaskFlowDbContext dbContext, ILogger<TaskService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<TaskResponseDto> CreateAsync(int userId, TaskCreateDto taskDto)
        {
            TaskItem task = new()
            {
                Title = taskDto.Title,
                IsCompleted = false,
                UserId = userId
            };

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Task {TaskId} created for user {UserId}.", task.Id, userId);

            return ToResponseDto(task);
        }

        public async Task<List<TaskResponseDto>> GetAllAsync(int userId)
        {
            return await _dbContext.Tasks.AsNoTracking()
                                              .Where(task => task.UserId == userId)
                                              .Select(task => new TaskResponseDto()
                                                        {
                                                            Id = task.Id, 
                                                            Title = task.Title,
                                                            IsCompleted = task.IsCompleted
                                                        })
                                                        .ToListAsync();
        }

        public async Task<TaskResponseDto?> GetByIdAsync(int userId, int taskId)
        {
            var task = await _dbContext.Tasks.FirstOrDefaultAsync(task => task.Id == taskId && task.UserId == userId);

            return task is null ? null : ToResponseDto(task);
        }

        public async Task<bool> UpdateAsync(int userId, int taskId, TaskUpdateDto taskDto)
        {
            // reference to the tracked entity object retrieved by ef core
            TaskItem? task = await _dbContext.Tasks.FirstOrDefaultAsync(existingTask => 
                                                        existingTask.Id == taskId &&
                                                        existingTask.UserId == userId);

            if (task is null) return false;

            task.Title = taskDto.Title;
            task.IsCompleted = taskDto.IsCompleted;

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Task {TaskId} updated for user {UserId}.", task.Id, userId);

            return true;
        }

        public async Task<bool> DeleteAsync(int userId, int taskId)
        {
            TaskItem? task = await _dbContext.Tasks.FirstOrDefaultAsync(existingTask =>
                                                        existingTask.Id == taskId &&
                                                        existingTask.UserId == userId);

            if (task is null) return false;

            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Task {TaskId} deleted for user {UserId}.", task.Id, userId);

            return true;
        }

        private static TaskResponseDto ToResponseDto(TaskItem task)
        {
            return new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };
        }
    }
}
