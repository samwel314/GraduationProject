namespace TravelLinkerModels.Models.ViewModels
{
    public class OverViewTripViewModel
    {
        public string Id { get; set; } = null!;

        public string City { get; set; } = null!; 
        public string CompanyId { get; set; } = null!;
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public double Price { get; set; }
        public string Date { get; set; } =   null!; 
        public string Image { get; set; } = null!; //car 


    }
}
