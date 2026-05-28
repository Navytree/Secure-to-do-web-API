using System.ComponentModel.DataAnnotations;

namespace ToDoUserWebAPI.DTOs
{
    public class PostDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public DateOnly DateTo { get; set; }
    }
}
