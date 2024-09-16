namespace TravelLinkerModels.Models
{
    public class VehicleSchedule
    {  
        public int Id { get; set; }     
        public int VehicleId { get; set; }
        public string TripId { get; set; } = null!;
        public DateTime? WorkFrom { get; set; } 
        public DateTime? WorkTo { get; set;} = null!;
        public Trip Trip { get; set; } = null!;       
        public Vehicle Vehicle { get; set; } = null!;
    }
}