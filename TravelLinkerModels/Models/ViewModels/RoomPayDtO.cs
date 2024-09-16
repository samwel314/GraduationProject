namespace TravelLinkerModels.Models.ViewModels
{
    public class RoomPayDtO
    {

        public string RoomId { get; set; } = null!;
        public string Title { get; set; } = null!;  
        public int DaysNumber { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime  CheckOut { get; set; } 
        public double Total { get; set; }
    }
}
