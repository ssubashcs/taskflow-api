using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Api.Dtos
{
    public class TaskUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }
    }
}
