namespace TravelLinkerModels.Models
{
    public class RoomTransaction
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string SessionId { get; set; } = null!;
        public bool IsCompleted { get; set; }
        public string RoomId { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Room Room { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int NumberOfDay { get; set; }
        public int ScheduleId { get; set; }     
        public ApplicationUser User { get; set; } = null!;

        public RoomSchedule Schedule { get; set; } = null!;

    }
}