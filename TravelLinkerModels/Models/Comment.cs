using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TravelLinkerModels.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [StringLength(250)]
        [Required ]
        public string Content { get; set; } = null!;
        [ValidateNever]
        public string FromId { get; set; } = null!;

        public string ToId { get; set; } = null!;
        [ForeignKey("FromId")]
        [ValidateNever]
        public ApplicationUser From { get; set; } = null!;
        [ForeignKey("ToId")]
        [ValidateNever]
        public ApplicationUser To { get; set; } = null!;    
        
    }
}
