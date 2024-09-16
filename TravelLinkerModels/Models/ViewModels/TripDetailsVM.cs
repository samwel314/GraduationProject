namespace TravelLinkerModels.Models.ViewModels
{
    public class TripDetailsVM : OverViewTripViewModel
    {
        public string NameCompany { get; set; } = null!; 

        public string City   { get; set; } = null!; 
        public int Duration { get; set; }
        public string Deadline { get; set; } = null!; //**-*-*-*-*--**-
         public bool Available {  get; set; }
        public int Capacity { get; set; }       

        public int AvailablePlaces { get; set; }        

        public string VehicleType { get; set; } = null!;        
        public List<string> Features { get; set; } = null!; 

    }
}
