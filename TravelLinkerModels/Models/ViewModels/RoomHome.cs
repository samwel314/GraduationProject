namespace TravelLinkerModels.Models.ViewModels
{
    public class RoomHome
    {
        public IEnumerable<OverViewRoomViewModel>  Rooms  { get; set; } = null!;
        public PaginationModel Pagination { get; set; } = null!;
    }

}
