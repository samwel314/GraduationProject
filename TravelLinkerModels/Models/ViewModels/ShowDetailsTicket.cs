namespace TravelLinkerModels.Models.ViewModels
{
    public class ShowDetailsTicket
    {
        public int Id { get; set; }
        public string ClientName { get; set; } = null!;
        public string Owner { get; set; } = null!;
        public string Total { get; set; } = null!;
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string Code { get; set; } = null!;

        public int NumberOfTickets { get; set; }

    }
}