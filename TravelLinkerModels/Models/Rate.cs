using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelLinkerModels.Models
{
    public class Rate
    {
        public int Id { get; set; }

        public int Value { get; set; }  

        [ValidateNever]
        public string FromId { get; set; } = null!;  // Customer 
        public string ToId { get; set; } = null!; // Enterprise 
        [ForeignKey("FromId")]
        [ValidateNever]
        public ApplicationUser From { get; set; } = null!;
        [ForeignKey("ToId")]
        [ValidateNever]
        public ApplicationUser To { get; set; } = null!;

    }
}
