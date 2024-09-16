namespace TravelLinkerModels.Models.ViewModels
{
    public class TripsPayDtO
    {
        public string TripId { get; set; } = null!; 

        public string Title { get; set; } = null!;  

        public int TicketsNumber { get; set; }  

        public double Total {  get; set; }      

    }
}
