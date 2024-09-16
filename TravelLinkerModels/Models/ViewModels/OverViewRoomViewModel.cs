using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TravelLinkerModels.Models.ViewModels
{
    public class OverViewRoomViewModel
    {
        public string  Id { get; set; } = string.Empty;

        public string HotelName { get; set; } =   null!;    
        public string City { get; set; } = null!;    
        public string HotelId { get; set; } = string.Empty; 
        public string Type { get; set; } = string.Empty; 
        public double Price { get; set; }
        public string  MainImage { get; set; } = null!; 
        public double ?  OfferPrice { get; set; }      

    }
}
