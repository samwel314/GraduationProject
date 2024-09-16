using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TravelLinkerModels.Models.ViewModels
{
    public class UpCreateVehicleVM : ShowVehicleVm
    {
        
        [Required(ErrorMessage = "You Must Select AT least One Feature")]
        public IEnumerable<string> Features { get; set; } = null!;
        public IFormFile  ? Image { get; set; } = null!;
        public string ?  ImageUrl { get; set; }    
    }
}
