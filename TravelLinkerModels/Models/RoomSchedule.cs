namespace TravelLinkerModels.Models
{
    public class RoomSchedule
    {
        public int Id { get; set; }
        public string RoomId { get; set; } = null!;
        public DateTime? WorkFrom { get; set; }
        public DateTime? WorkTo { get; set; } = null!;
        public Room Room { get; set; } = null!;

    }
}