using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Dtos;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskFlowDbContext _dbContext;

        // ASP.NET Core's DI container creates and injects the DbContext instance here.
        public TasksController(TaskFlowDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskResponseDto>>> GetAll()
        {
            var tasks = await _dbContext.Tasks.AsNoTracking() // Don't track changes for read-only queries.
                                              .Select(task => new TaskResponseDto
                                              {
                                                  Id = task.Id,
                                                  Title = task.Title,
                                                  IsCompleted = task.IsCompleted
                                              })
                                              .ToListAsync();
        
            return Ok(tasks);
        }

        [HttpGet("{id:int}", Name = "GetTaskById")]
        public async Task<ActionResult<TaskResponseDto>> GetById(int id)
        {
            var task = await _dbContext.Tasks.FindAsync(id);

            if (task is null) return NotFound();

            return Ok(ToResponseDto(task));
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> Create(TaskCreateDto taskDto)
        {
            TaskItem task = new()
            {
                Title = taskDto.Title,
                IsCompleted = false
            };

            _dbContext.Add(task);
            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetTaskById", new { id = task.Id }, ToResponseDto(task));
        }

        // IActionResult represents an HTTP response produced by a controller action.
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TaskUpdateDto taskDto)
        {
            // reference to the tracked entity object retrieved by ef core
            var existingTask = await _dbContext.Tasks.FindAsync(id);

            if (existingTask is null)
            {
                return NotFound();
            }

            existingTask.Title = taskDto.Title;
            existingTask.IsCompleted = taskDto.IsCompleted;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _dbContext.Tasks.FindAsync(id);

            if (task is null)
            {
                return NotFound();
            }

            _dbContext.Tasks.Remove(task);
            await _dbContext.SaveChangesAsync();

            return NoContent();
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
