using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security;

namespace TravelLinkerModels.Models
{
    public class Company  
    {
        [Key]
        public string Id { get; set; }
        [StringLength(maximumLength: 100)]
        public string? Location { get; set; }
        [StringLength(maximumLength: 200)]
        public string? Description { get; set; }
        [StringLength(maximumLength: 50)]
        [NotMapped]
        public string? City { get; set; }
   
        public int Rate { get; set; } = 0;
        // nav 
        [ForeignKey("Id")]
        public ApplicationUser User { get; set; }      // -> <>
    }
}