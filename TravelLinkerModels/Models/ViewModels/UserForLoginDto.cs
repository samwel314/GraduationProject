using System.ComponentModel.DataAnnotations;

namespace TravelLinkerModels.Models.ViewModels
{
    public class UserForLoginDto
    {
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; init; }
    }
}
