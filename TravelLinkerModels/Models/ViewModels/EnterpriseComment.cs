namespace TravelLinkerModels.Models.ViewModels
{
    public class EnterpriseComment
    {
        public IEnumerable<ViewCommentDTO> Comments  { get; set; } = null!;
        public PaginationModel Pagination { get; set; } = null!;
    }

}
