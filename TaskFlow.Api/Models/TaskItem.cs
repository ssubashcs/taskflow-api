namespace TaskFlow.Api.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        public int? UserId { get; set; }

        // EF core navigation property to load related user object.
        public User? User { get; set; }
    }
}
