using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelLinkerModels.Models.ViewModels
{
    public class UpCreateTripVM
    {
        [ValidateNever]
        public string Id { get; set; } = null!;
        [StringLength(50)]
        public string From { get; set; } = null!;
        [StringLength(50)]
        public string To { get; set; } = null!;
        [Required]
        public DateTime ? StartAt { get; set; }
        [Required]
        public DateTime ? LastDate { get; set; }  ///for Booking 
        [Range(1 , 48)]
        public int Duration { get; set; }

        public double Price { get; set; }
        public string Code { get; set; } = null!;
        public string CompanyId { get; set; } = null!;
        [Required]
        public int VehicleId { get; set; }

    }
}
