using System.ComponentModel.DataAnnotations.Schema;

namespace TravelLinkerModels.Models
{
    public class VehicleFeature
    {
        public int VehicleId { get; set; } 
        public string FeatureName { get; set; } = null!;
        [ForeignKey(nameof (VehicleId))]
        public Vehicle  Vehicle { get; set; } = null!;
    }
}