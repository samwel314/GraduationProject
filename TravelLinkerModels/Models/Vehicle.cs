using System.ComponentModel.DataAnnotations;

namespace TravelLinkerModels.Models
{
    public class Vehicle
    {
        public int  Id { get; set; }
        public string Type { get; set; } = null!; 
        public int Capacity { get; set; }
        [StringLength(7)]
        public string LicenseNumber { get; set; } = null!;  // unique 
        public string CompanyId { get; set; } = null!;
        public string ? ImageUrl {  get; set; }
        public IEnumerable<VehicleSchedule>  VehicleSchedules { get; set; } = null!;
        public IEnumerable<VehicleFeature> vehicleFeatures { get; set; } = null!; 
        public Company Company { get; set; } = null!;

       
    }
}