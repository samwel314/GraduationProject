using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelLinkerModels.Models.ViewModels
{
    public class ViewCommentDTO
    {
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Content { get; set; } = null!;
    }

}
