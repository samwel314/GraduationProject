using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TravelLinkerModels.Models
{
    public class ApplicationUser : IdentityUser
    {

        [MaxLength(60)]
        public string EnterpriseName { get; set; } = null!; // username 
          
        public string?  ProfileImage { get; set; } = null!;

        [MaxLength(11)]
        public override string? PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }

        [MaxLength(40)]
        public string ? City { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime  ? RefreshTokenExpiryTime { get; set; }

       
    }
}