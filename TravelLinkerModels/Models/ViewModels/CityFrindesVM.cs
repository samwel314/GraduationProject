namespace TravelLinkerModels.Models.ViewModels
{
   public class CityFrindesVM
   {
        public string City { get; set; } = null!;

        public IEnumerable<ShowFrindes> Frindes { get; set; }  = null!;      
        
   }


    public class TokenDto
    {
        public string RefreshToken { get; set; } = null!;
        public string Token { get; set; } = null!;
    }

    }
