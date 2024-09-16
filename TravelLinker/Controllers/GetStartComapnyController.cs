using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinker.Controllers
{
    public class GetStartComapnyController : Controller
    {
        private readonly UpCreateVehicleVM  Vehicle = new UpCreateVehicleVM()
        {
            Id = 1, 
            Type = "Bus" ,
            LicenseNumber = "xxxxxx1" , 
            Capacity = 50 , 
            Features= new List <string>()
            {
                "Onboard Restroom" ,
                "Onboard Entertainment"
            }
        };

        private readonly UpCreateTripVM Trip = new UpCreateTripVM()
        {
            Id = "5445",
            From = "Alexandria",
            To = "Aswan" , 
            Duration = 12 , 
            StartAt = DateTime.Now.AddDays(4), 
            LastDate = DateTime.Now.AddDays(3) ,
            Price = 50 ,
            Code = "17#xx" , 

        }; 
         public IActionResult Index(int flage = 0 )
         {
            if (flage == 0)
            return View(new UpCreateTripVM ());

            return View(Trip);
        }
        public IActionResult CreateVehicle()
        {
            return View(Vehicle);
        }

        public IActionResult  ShowVehicle (int flage)
        {
            if (flage == 0)
                return View(new UpCreateVehicleVM());

            return View(Vehicle);
        }

        public IActionResult ShowTrip(int flage)
        {
            if (flage == 0)
                return View(new UpCreateTripVM());

            return View(Trip);
        }

        public IActionResult CreateTrip()
        {
            return View(Trip);  
        }


    }

 
}
