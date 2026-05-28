using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ToDoUserWebAPI.Models
{
    public class ToDo
    {
       public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        public bool Done { get; set; } = false; 
        public DateOnly DateTo {  get; set; }

        [ForeignKey("UserId")]
        public int UserId { get; set; } 
        public virtual User User { get; set; } = null!;

    }
}
