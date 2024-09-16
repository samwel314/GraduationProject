using System.ComponentModel.DataAnnotations;

namespace TravelLinkerModels.Models.ViewModels
{
    public class ShowVehicleVm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You Must Select Vehicle Type")]
        public string Type { get; set; } = null!;
        [Required(ErrorMessage = "You Must Select Vehicle Capacity")]
        public int Capacity { get; set; }
        [StringLength(7)]
        [Required(ErrorMessage = "You Must Enter  Vehicle License Plate Number")]

        public string LicenseNumber { get; set; } = null!;  // unique 
        public string CompanyId { get; set; } = null!;   // ==  user  id 
    }
}
