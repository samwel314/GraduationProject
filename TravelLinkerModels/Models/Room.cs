using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelLinkerModels.Models
{
    public class Room
    {
        public string Id { get; set; } = null!;
        public int Number { get; set; }
        public int FloorNumber { get; set; }
        [StringLength(50)]
        public string Type { get; set; } = null!;
        public string ? ImageUrl  { get; set; } = null!;
        public int BedsNumber { get; set; }
        public bool IsHide{ get; set; } = false;
        public double PricePerNight { get; set; }
        public double PricePerMonth { get; set; }
        public double PricePerWeek { get; set; }
        public string HotelId { get; set; } = null!;

        [ValidateNever]
        public Hotel Hotel { get; set; } = null!; 
        
        [ValidateNever]
        public IEnumerable<RoomImage> RoomImages { get; set; }= null!; 
        
        [ValidateNever]
        public IEnumerable<RoomFeature> RoomFeatures { get; set; } = null!;
        public RoomOffer ? RoomOffer { get; set; }        //

    }
}