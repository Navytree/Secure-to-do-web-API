using System.ComponentModel.DataAnnotations;

namespace ToDoUserWebAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string Login {  get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        public virtual ICollection<ToDo> Todos { get; set; } = new List<ToDo>(); 

    }
}
