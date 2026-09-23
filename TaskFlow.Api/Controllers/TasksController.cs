using Microsoft.AspNetCore.Mvc;
using TaskFlow.Api.Models;

namespace TaskFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private static readonly List<TaskItem> Tasks = new()
        {
            new TaskItem
            {
                Id = 1,
                Title = "Learn ASP.NET Core",
                IsCompleted = false
            }
        };

        [HttpGet]
        public ActionResult<List<TaskItem>> GetAll()
        {
            return Ok(Tasks);
        }

        [HttpGet("{id:int}", Name = "GetTaskById")]
        public ActionResult<TaskItem> GetById(int id)
        {
            var task = Tasks.FirstOrDefault(task => task.Id == id);

            if (task is null) return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public ActionResult<TaskItem> Create(TaskItem task)
        {
            task.Id = Tasks.Count == 0 ? 1 : Tasks.Max(existingTask => existingTask.Id) + 1;

            Tasks.Add(task);

            return CreatedAtRoute("GetTaskById", new { id = task.Id }, task);
        }

        // IActionResult represents an HTTP response produced by a controller action.
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, TaskItem updatedTask)
        {
            var existingTask = Tasks.FirstOrDefault(task => task.Id == id);

            if (existingTask is null)
            {
                return NotFound();
            }

            existingTask.Title = updatedTask.Title;
            existingTask.IsCompleted = updatedTask.IsCompleted;

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var task = Tasks.FirstOrDefault(task => task.Id == id);

            if (task is null)
            {
                return NotFound();
            }

            Tasks.Remove(task);

            return NoContent();
        }
    }
}
