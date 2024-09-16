using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;

namespace TravelLinkerModels.Models.ViewModels
{
    public class ChangeIMagesModel
    {
        public string Id { get; set; } = null!;
        public IFormFile File { get; set; } = null!;    
    }
}
