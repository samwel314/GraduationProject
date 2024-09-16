using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelLinkerModels.Models
{
    public class RoomOffer
    {
        [Key]
        public string RoomId { get; set; }
        [Range(1 , int.MaxValue)]   
        public double OfferPrice  { get; set; }
        [StringLength(100)]
        public string OfferDescription { get; set; }
        // nav 
        [ForeignKey("RoomId")]
        [ValidateNever]
        public Room Room { get; set; }      
    }
}