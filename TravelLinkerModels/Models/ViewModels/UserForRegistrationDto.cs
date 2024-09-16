using System.ComponentModel.DataAnnotations;

namespace TravelLinkerModels.Models.ViewModels
{
    public class UserForRegistrationDto
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; init; } = null!;
        [Required(ErrorMessage = "Last Name is required")]
        public string? LastName { get; init; } = null!;
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; init; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(40)]
        public string? City { get; set; }
    }
}
