namespace TravelLinkerModels.Models.ViewModels
{
    public class ShowRoomSchedule
    {
        public int Id { get; set; }
        public string CheckIn { get; set; } = null!;
        public string CheckOut { get; set; } = null!;

        public int TrId { get; set; }
    }
}
