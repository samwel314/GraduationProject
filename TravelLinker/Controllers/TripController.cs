using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models.ViewModels;
using TravelLinkerModels.Models;
using Microsoft.AspNetCore.Authorization;

namespace TravelLinker.Controllers
{
    [Authorize(policy: "IsCompany")]
    public class TripController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITripService _tripService;
        private readonly IVehicleScheduleService _vehicleScheduleService;

        public TripController(UserManager<ApplicationUser> userManager, ITripService tripService, IVehicleScheduleService vehicleScheduleService)
        {
            _userManager = userManager;
            _tripService = tripService;
            _vehicleScheduleService = vehicleScheduleService;
        }


        public async Task<IActionResult> Index (string id )
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This User Not Found");

            var trip = _tripService.GetTrip(id , true );
            if (trip == null)
                return NotFound(); 

            return View(trip);  
        }

        public async Task<IActionResult> UpCreate(string id = null! )    //id Vehicle >> Id 
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This User Not Found");
            // this Creata Requst 
            if (id == null)
            {
                return View(new UpCreateTripVM
                {
                    CompanyId = user.Id,
                });
            }
            //Update Requset 
            var trip = _tripService.GetTrip(id);
            if (trip == null)
                return NotFound();

            return View(new UpCreateTripVM
            {
                Id = trip.Id,
                CompanyId = user.Id,
                From = trip.From,
                To = trip.To,
                Code = trip.Code,
                Duration = trip.Duration , 
                LastDate = trip.LastDate,   
                StartAt = trip.StartAt, 
                Price = trip.Price, 
            });
        }



        [HttpPost]
        public async Task<IActionResult> HideNotHide(string Id)
        {
            await _tripService.HideNotHide(Id);
            return Json(new { success = true, message = "Done ..  " });
        }


        [HttpPost]
        public async Task<IActionResult> UpCreate(UpCreateTripVM model)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This User Not Found");

            if (model.Id != null)
            {
                if (await _tripService.CanUpdate(model.Id))
                {
                    ModelState.AddModelError
                        ("", "You Can not Update This Trip it Live Now and Have Tikcts Or Its Done With its Tickets    ");
                    return View(model);
                }
            }



            if (!ModelState.IsValid)
                return View(model);
            if (model.From == model.To)
            {
                ModelState.AddModelError("", "Cannot set Trip For same direction  ");
                return View(model);
            }
            if (model.StartAt <= DateTime.Now)
            {
                ModelState.AddModelError("StartAt", "invalid Start Date ");
                return View(model);
            }
            if (model.LastDate >=  model.StartAt!.Value.AddHours(model.Duration) ||
               model.LastDate <= DateTime.Now)
            {
                ModelState.AddModelError("LastDate", "invalid Last Booking Date  ");
                return View(model);
            }
            if (_tripService.AnyCode(model.Code , user.Id  , model.Id))
            {
                //--
                ModelState.AddModelError("Code", "This Code  Is Used ");
                return View(model);
            }

            bool check = false;
            if (model.Id == null)
            {
                check = _vehicleScheduleService.
                TryCreateSchedule
                   (model.VehicleId, model.StartAt!.Value, model.StartAt!.Value.AddHours(model.Duration));
            }
            else
            {
                check = _vehicleScheduleService.
               TryCreateSchedule
                (model.VehicleId, model.StartAt!.Value, model.StartAt.Value.AddHours(model.Duration) , model.Id , false);
            }
            if (!check)
            {
                ModelState.AddModelError("VehicleId", "This Vehicle Is not available in this time ");
                return View(model);
            }

 
            Trip? trip = new Trip
            {
                Id = model.Id!,
                CompanyId = user.Id,
                From = model.From,
                To = model.To,
                Code = model.Code,
                Duration = model.Duration,
                LastDate = model.LastDate!.Value,
                StartAt = model.StartAt!.Value,
                Price = model.Price,
            };

            var sche = new VehicleSchedule
            {
                VehicleId = model.VehicleId,
                TripId = trip.Id,
                WorkFrom = model.StartAt,
                WorkTo = model.StartAt.Value.AddHours(model.Duration),
            }; 


            if (model.Id == null)
            {

                trip = _tripService.Create(trip);
                sche.TripId = trip.Id;
                _vehicleScheduleService.Create(sche); 
            }
            else
            { 
                trip = _tripService.Update(trip);
                _vehicleScheduleService.Update(sche);
            }
            if (trip == null )
                return NotFound();

            return RedirectToAction("DisPlayAll" , "Company"); // soon
        }


        public async Task <IActionResult > ShowAllTickets(string Id)
        {
            var data =await _tripService.showAllTicket(Id);      
            return View(data);          
        }

    }
}
