using TaskFlow.Api.Dtos;

namespace TaskFlow.Api.Services
{
    public interface ITaskService
    {
        Task<List<TaskResponseDto>> GetAllAsync();

        Task<TaskResponseDto?> GetByIdAsync(int id);

        Task<TaskResponseDto> CreateAsync(TaskCreateDto taskDto);

        Task<bool> UpdateAsync(int id, TaskUpdateDto taskDto);

        Task<bool> DeleteAsync(int id);

    }
}
