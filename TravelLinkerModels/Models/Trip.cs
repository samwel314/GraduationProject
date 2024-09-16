using System.ComponentModel.DataAnnotations;

namespace TravelLinkerModels.Models
{
    public class Trip
    {
        public string Id { get; set; } = null!;
        [StringLength(50)]
        public string From { get; set; } = null!;
        [StringLength(50)]
        public string To { get; set; }  =   null !; 
        public DateTime StartAt { get; set; }
        public DateTime LastDate { get; set; }  ///for Booking 
        public int Duration { get; set; }
        [StringLength (7)]
        [MinLength(7)]
        public string Code { get; set; } = null!;       // For Booking Ticket  ///  UNIQUE    
        public bool IsHidden { get; set; } = false;  
        public double Price {  get; set; }
        public string CompanyId { get; set; } = null!;
        public VehicleSchedule VehicleSchedule { get; set; } = null!;   
        public Company Company { get; set; } = null!;

    }
}