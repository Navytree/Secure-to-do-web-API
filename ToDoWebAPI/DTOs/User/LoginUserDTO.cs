using System.ComponentModel.DataAnnotations;

namespace ToDoUserWebAPI.DTOs
{
    public class LoginUserDTO
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
