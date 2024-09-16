using Microsoft.AspNetCore.Mvc;
using TravelLinkerModels.Models;

namespace TravelLinker.Controllers
{
	public class GetStartHotelController : Controller
	{
        private readonly Room room = new Room
        {
            Id = "123",
            Number = 101,
            FloorNumber = 1,
            Type = "single Room",
            ImageUrl = "8362d8a7-96b7-4dc3-a5b4-040416f52ccf.jpeg",
            BedsNumber = 2,
  
            PricePerNight = 100.0,
            PricePerMonth = 2000.0,
            PricePerWeek = 700.0
        }; 

        public IActionResult	Index (int flage = 0 )
		{
            if ( flage == 0 )
                return View(new Room());

            return View(room);
		}

		public IActionResult Create () 
		{

			return View(room); 
		
		}
        public IActionResult Show (int flage =0 )
        {
            if (flage == 0)
                return View(new Room());

            return View(room);
        }

    }
}
