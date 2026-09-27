using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Api.Dtos;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        // ASP.NET Core's DI container creates and injects the ITaskService implementation here.
        public TasksController(ITaskService taskService) 
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<List<TaskResponseDto>>> GetAll()
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            var tasks = await _taskService.GetAllAsync(userId);
        
            return Ok(tasks);
        }

        [HttpGet("{id:int}", Name = "GetTaskById")]
        public async Task<ActionResult<TaskResponseDto>> GetById(int id)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            var task = await _taskService.GetByIdAsync(userId, id);

            if (task is null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> Create(TaskCreateDto taskDto)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            var task = await _taskService.CreateAsync(userId, taskDto);

            return CreatedAtRoute("GetTaskById", new { id = task.Id }, task);
        }

        // IActionResult represents an HTTP response produced by a controller action.
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TaskUpdateDto taskDto)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            bool updated = await _taskService.UpdateAsync(userId, id, taskDto);

            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!TryGetCurrentUserId(out var userId))
            {
                return Unauthorized();
            }

            bool deleted = await _taskService.DeleteAsync(userId, id);

            if (!deleted) return NotFound();

            return NoContent();
        }

        private bool TryGetCurrentUserId(out int userId)
        {
            return int.TryParse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
        }
    }
}
