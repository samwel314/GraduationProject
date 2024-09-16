using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelLinkerModels.Models.ViewModels
{
    public class UpDateCreateRoomViewModel
    {
        public string HotelId { get; set; } = null!; 
        [ValidateNever]
        public string RoomId { get; set; } = null!;

        [Required(ErrorMessage = "Enter Room  Number  ")]
        [Range(minimum: 0, maximum: int.MaxValue)]
        public int Number { get; set; }

        [Required(ErrorMessage = "Enter Floor Number ")]
        [Range(minimum: 0, maximum: 20)]
        public int FloorNumber { get; set; }

        [Required(ErrorMessage = "Selct Room Type  ")]
        public string Type { get; set; } = null!;

        [Required(ErrorMessage = "Enter Number Of Beds ")]
        [Range(minimum: 1, maximum: 20)]
        public int BedsNumber { get; set; }
       
        [Required (ErrorMessage = "Enter Room Price per Night")]
        [Range(minimum: 1 , maximum: int.MaxValue)]
        public double PricePerNight { get; set; }
        
        [Required(ErrorMessage = "Enter Room Price per Month ")]
        [Range(minimum: 0, maximum: int.MaxValue)]
        public double PricePerMonth { get; set; }

        [Required(ErrorMessage = "Enter Room Price per Week  ")]
        [Range(minimum: 0, maximum: int.MaxValue)]
        public double PricePerWeek { get; set; }

       // [Required (ErrorMessage = "Upload At Lest One Photo  ")]
        public IFormFileCollection ? Images { get; set; }

        [Required(ErrorMessage = "Select At Lest One  Feature ")]
        public List<string> Features { get; set; } = null!;

        // We use It in Update 
        public List<string> ? ImagesUrls  { get; set;  }
    }
}
