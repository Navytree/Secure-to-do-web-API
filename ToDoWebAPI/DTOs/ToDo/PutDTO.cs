using System.ComponentModel.DataAnnotations;

namespace ToDoUserWebAPI.DTOs
{
    public class PutDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public bool Done { get; set; }
        public DateOnly DateTo { get; set; }
    }
}
