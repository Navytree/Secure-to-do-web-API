using System.ComponentModel.DataAnnotations;

namespace ToDoUserWebAPI.DTOs
{
    public class RegisterUserDTO
    {
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Login length must be between 3 and 20 characters")]
        public string Login { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Password length must be between 3 and 20 characters")]
        public string Password { get; set; } = string.Empty;
    }
}
