using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelLinker.Helpers;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinker.Controllers
{
    [Authorize (policy: "IsHotel")]//IsCompany
    public class HotelController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IHotelService _hotelService;

        public HotelController(UserManager<ApplicationUser> userManager, IHotelService hotelService)
        {
            _userManager = userManager;
            _hotelService = hotelService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null ) 
                return NotFound("This User Not Found");
            var Rooms  =   _hotelService.GetHotelRooms(user.Id);

            var ViewModel = HelperMethods.OverViewRoomMap(Rooms);
            return View(ViewModel);
        }
        public async Task<IActionResult> Show()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This User Not Found");

            var ViewModel = _hotelService.GetHotelRooms(user.Id);

            return View(ViewModel.ToList());
        }


        public async Task<IActionResult> Frindes (string City )
        {
            var user = await _userManager.GetUserAsync(User);
     
            if (string.IsNullOrEmpty(City))
            {
                City = user!.City!;
            }
            var data = new CityFrindesVM(); 
            data.City = City;
            data.Frindes = await _hotelService.ShowFrindes(City , user.Id ); 
            return View(data);  
        }

        public async Task<IActionResult> ViewFrind (string Id)
        {

            var data = await _hotelService.LoadFriend(Id); 

            return View(data);
        }
    }
}
