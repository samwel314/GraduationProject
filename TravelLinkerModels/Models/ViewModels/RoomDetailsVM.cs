using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelLinkerModels.Models.ViewModels
{
    public class RoomDetailsVM
    {
        public string Id { get; set; } = string.Empty;
        public string HotelId { get; set; } = string.Empty;
        public string HotelName { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public int Number { get; set; }
        public int FloorNumber { get; set; }
        public string Type { get; set; } = string.Empty;
        public double PricePerNight { get; set; }
        public double PricePerMonth { get; set; }
        public double PricePerWeek { get; set; }
        public double? OfferPrice { get; set; }

        public string ? OfferMessage { get; set; }      
        public int BedsNumber { get; set; }

        public List<string> Features { get; set; } = null!; 

        public List<string> Images { get; set; } = null!;   
    }
}
