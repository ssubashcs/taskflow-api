using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Dtos;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskFlowDbContext _dbContext;

        public TaskService(TaskFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TaskResponseDto> CreateAsync(TaskCreateDto taskDto)
        {
            TaskItem task = new()
            {
                Title = taskDto.Title,
                IsCompleted = false
            };

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            return ToResponseDto(task);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            TaskItem? task = await _dbContext.Tasks.FindAsync(id);

            if (task is null) return false;

            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<TaskResponseDto>> GetAllAsync()
        {
            return await _dbContext.Tasks.AsNoTracking()
                                              .Select(task => new TaskResponseDto()
                                                        {
                                                            Id = task.Id, 
                                                            Title = task.Title,
                                                            IsCompleted = task.IsCompleted
                                                        })
                                                        .ToListAsync();
        }

        public async Task<TaskResponseDto?> GetByIdAsync(int id)
        {
            var task = await _dbContext.Tasks.FindAsync(id);

            return task is null ? null : ToResponseDto(task);
        }

        public async Task<bool> UpdateAsync(int id, TaskUpdateDto taskDto)
        {
            // reference to the tracked entity object retrieved by ef core
            TaskItem? task = await _dbContext.Tasks.FindAsync(id);

            if (task is null) return false;

            task.Title = taskDto.Title;
            task.IsCompleted = taskDto.IsCompleted;

            await _dbContext.SaveChangesAsync();

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
