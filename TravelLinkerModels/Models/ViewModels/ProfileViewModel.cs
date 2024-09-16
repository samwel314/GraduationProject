using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TravelLinkerModels.Models.ViewModels
{
    public class ProfileViewModel
    {
        public string? Id { get; set; } = null!;
        public string EnterpriseName { get; set; } = null!;
        public string? ProfileImage { get; set; } = null!;
        public string? PhoneNumber { get; set; } = null!;
        public string? Email { get; set; } = null!;
        [Required(ErrorMessage = "Enter Valid Location")]
        public string Location { get; set; } = null!;
        [Required(ErrorMessage = "Enter Valid Description")]
        public string Description { get; set; } = null!;
        public string? City { get; set; }
        public double Rate { get; set; } = 0;
        public int? Stars { get; set; } = null;
        public string RateResult { get; set; } = null!;
        public int Reviews { get; set; }
        public int NumServies { get; set; }
        public int NumBooking { get; set; }
        public string Type { get; set; } = null!;   
        public string TotalIn { get; set; } = null!;
        public IEnumerable<ViewCommentDTO> ViewComments { get; set; } = null!;
        [Required(ErrorMessage = "Select Image  .... ")]
        public IFormFile ImageToChaneg { get; set; } = null!;
    }

    public class FriendProfileVM : ProfileViewModel
    {
     public IEnumerable<OverViewRoomViewModel> Rooms { get; set; } = null!;     
    }

}
