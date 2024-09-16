namespace TravelLinkerModels.Models
{
    public class TripTransaction
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string SessionId { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public string  TripId { get; set; } = null!; 
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Trip Trip  { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int TicketsNumber { get; set; } 
        public ApplicationUser User { get; set; } = null!;
    }


}