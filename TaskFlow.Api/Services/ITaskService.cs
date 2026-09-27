using TaskFlow.Api.Dtos;

namespace TaskFlow.Api.Services
{
    public interface ITaskService
    {
        Task<List<TaskResponseDto>> GetAllAsync(int userId);

        Task<TaskResponseDto?> GetByIdAsync(int userId, int taskId);

        Task<TaskResponseDto> CreateAsync(int userId, TaskCreateDto taskDto);

        Task<bool> UpdateAsync(int userId, int id, TaskUpdateDto taskDto);

        Task<bool> DeleteAsync(int userId, int id);

    }
}
