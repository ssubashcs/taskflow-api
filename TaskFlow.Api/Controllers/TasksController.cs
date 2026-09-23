using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Data;
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
        public async Task<ActionResult<List<TaskItem>>> GetAll()
        {
            // Don't track changes for read-only queries.
            var tasks = await _dbContext.Tasks.AsNoTracking().ToListAsync();
        
            return Ok(tasks);
        }

        [HttpGet("{id:int}", Name = "GetTaskById")]
        public async Task<ActionResult<TaskItem>> GetById(int id)
        {
            var task = await _dbContext.Tasks.FindAsync(id);

            if (task is null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> Create(TaskItem task)
        {
            _dbContext.Add(task);

            await _dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetTaskById", new { id = task.Id }, task);
        }

        // IActionResult represents an HTTP response produced by a controller action.
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, TaskItem updatedTask)
        {
            // reference to the tracked entity object retrieved by ef core
            var existingTask = await _dbContext.Tasks.FindAsync(id);

            if (existingTask is null)
            {
                return NotFound();
            }

            existingTask.Title = updatedTask.Title;
            existingTask.IsCompleted = updatedTask.IsCompleted;

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
    }
}
