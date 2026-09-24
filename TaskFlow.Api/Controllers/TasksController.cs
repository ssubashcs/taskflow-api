using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
using TaskFlow.Api.Dtos;
using TaskFlow.Api.Models;
using TaskFlow.Api.Services;

namespace TaskFlow.Api.Controllers
{
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
            var tasks = await _taskService.GetAllAsync();
        
            return Ok(tasks);
        }

        [HttpGet("{id:int}", Name = "GetTaskById")]
        public async Task<ActionResult<TaskResponseDto>> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);

            if (task is null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskResponseDto>> Create(TaskCreateDto taskDto)
        {
            var task = await _taskService.CreateAsync(taskDto);

            return CreatedAtRoute("GetTaskById", new { id = task.Id }, task);
        }

        // IActionResult represents an HTTP response produced by a controller action.
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TaskUpdateDto taskDto)
        {
            bool updated = await _taskService.UpdateAsync(id, taskDto);

            if (!updated) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool deleted = await _taskService.DeleteAsync(id);

            if (!deleted) return NotFound();

            return NoContent();
        }
    }
}
