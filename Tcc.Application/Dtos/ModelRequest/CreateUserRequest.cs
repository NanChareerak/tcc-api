using System.ComponentModel.DataAnnotations;

namespace Tcc.Application.Dtos.ModelRequest
{
    public class CreateUserRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}