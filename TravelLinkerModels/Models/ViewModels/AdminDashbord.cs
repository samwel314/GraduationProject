namespace TravelLinkerModels.Models.ViewModels
{
    public class AdminDashbord
    {
        public List<ApplicationUser> Hotels { get; set; } = null!; 
        public List<ApplicationUser>  Companies { get; set; } = null!;
    }
}
