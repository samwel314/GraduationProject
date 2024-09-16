using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelLinkerModels.Models
{
    public class Hotel
    {
        [Key]
        public string Id { get; set; } = null!; 
        [StringLength(maximumLength: 100)]
        public string? Location { get; set; }
        [StringLength(maximumLength: 200)]
        public string? Description { get; set; }
        [StringLength(maximumLength: 50)]
        [NotMapped]
        public string? City { get; set; }
        public int Rate { get; set; } = 0 ;
        public int Stars { get; set; } = 1;

        // nav 
        [ForeignKey("Id")]
        public ApplicationUser User { get; set; }      // -> <>


              
    }



}