namespace TravelLinkerModels.Models.ViewModels
{
    public class ShowDetailsBookingRoom
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = null!;
        public string Owner { get; set; } = null!;
        public string Total { get; set; } = null!;
        public string CheckIn { get; set; } = null!;
        public string CheckOut { get; set; } = null!;
        public int NumberOfDays { get; set; }
        public string Type { get; set; } = null!;
        public int Floor { get; set; }
        public int Number { get; set; }
        public int ScheduleId { get; set; }
    }
    }