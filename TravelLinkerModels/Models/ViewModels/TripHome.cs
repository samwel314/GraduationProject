namespace TravelLinkerModels.Models.ViewModels
{
    public class TripHome
    {
        public IEnumerable<OverViewTripViewModel> Trips { get; set; } = null!;

        public PaginationModel Pagination { get; set; } = null!;
    }
}
