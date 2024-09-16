namespace TravelLinkerModels.Models.ViewModels
{
    public class AuthenticationModel
    {

        public bool IsAuthenticated { get; set; }
        public string Message { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public string Token { get; set; } = null!;
        public string? City { get; set; }
    }
}