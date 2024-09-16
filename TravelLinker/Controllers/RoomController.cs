using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Hosting;
using TravelLinker.Helpers;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinker.Controllers
{
    [Authorize(policy: "IsHotel")]//IsCompany
    public class RoomController : Controller
    {
        private readonly IRoomService _RoomService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IUserService  _userService;     
        private string _path;
        public RoomController(IRoomService roomService, UserManager<ApplicationUser> userManager, IWebHostEnvironment hostEnvironment , IUserService userService)
        {
            _RoomService = roomService;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
            _path = _hostEnvironment.WebRootPath + PathHelper.HotelPath;
            _userService = userService; 
            
        }

        public async Task<IActionResult> Index(string Id)
        {
            if (Id == null)
                return BadRequest();

            var ViewModel = await _RoomService.GetRoom(Id, true);
            if (ViewModel == null)
                return NotFound("This Room Not Found ");


            return View(ViewModel);
        }

        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This User Not Found");

            var viewModel = new UpDateCreateRoomViewModel
            {
                HotelId = user.Id,
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult>
            Create(UpDateCreateRoomViewModel Model)
        {

            if (!ModelState.IsValid)
            {
                return View(Model);
            }
            if (Model.Images == null)
            {
                ModelState.AddModelError("Images", "Upload At Lest One Photo");
                return View(Model);
            }
            if (Model.Images.Count > 3)
            {
                ModelState.AddModelError("Images", "Maximum  3 Photos ");
                return View(Model);
            }
            foreach (var image in Model.Images)
            {
                if (!HelperMethods.CheckExtension(image))
                {
                    ModelState.AddModelError("Image", "We allow .jpg , .png files  ");
                    return View(Model);
                }
            }
            var user = await _userManager.GetUserAsync(User);

            // Authorization By resource --- ---> sooon 

            if (user == null || Model.HotelId != user.Id)
                return NotFound("This User Not Found");

            // --- HelperMethods Is Static Class 
            var room = HelperMethods.MapToRoom(Model);
            //delete this line 
            _path = Path.Combine(_path, user.Id);

            room.RoomImages = HelperMethods.
                UrlImagesRoom(room.Id, _path, Model.Images);

            room.RoomFeatures = Model.Features.Select
                (f => new RoomFeature
                {
                    FeatureName = f,
                    RoomId = room.Id
                }).ToList();

            // Call Room Service To Create new Room
            var check = _RoomService.Create(room);

            // we will Create a view for this Errors soon 
            if (!check)
                return BadRequest("Some Error ");

            return RedirectToAction("Index", "Hotel");
        }

        public async Task<IActionResult> Update(string Id)
        {
            var room = await _RoomService.GetRoom(Id);

            if (room == null)
                return NotFound("This resourse Not Found");

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();
            // Authorization By resource --- ---> sooon 
            if (user.Id != room.HotelId)
            {
                return BadRequest("Can't Do This Action ");
            }

            var viewModel = HelperMethods.MapToUpdateViewModel(room);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpDateCreateRoomViewModel Model)
        {
            if (!ModelState.IsValid)
            {
                return View(Model);
            }

            if (Model.Images != null && Model.Images.Count > 3)
            {
                ModelState.AddModelError("Images", "Maximum  3 Photos ");
                return View(Model);
            }
            if (Model.Images != null)
            {
                foreach (var image in Model.Images)
                {
                    if (!HelperMethods.CheckExtension(image))
                    {
                        ModelState.AddModelError("Image", "We allow .jpg , .png files  ");
                        return View(Model);
                    }
                }
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null || Model.HotelId != user.Id)
                return NotFound("This User Not Found");

            var room = await _RoomService.GetRoom(Model.RoomId);
            if (room == null)
                return NotFound("This Resource  Not Found");

            room = MapUpdateRoom(Model, room);
            room.RoomFeatures = Model.Features.Select
                (f => new RoomFeature
                {
                    FeatureName = f,
                    RoomId = room.Id
                }).ToList();

            bool have = false;
            if (Model.Images != null)
            {
                _path = Path.Combine(_path, user.Id);
                //delete old images 
                var urls = room.RoomImages.Select(rm => Path.Combine(_path, rm.ImageUrl)).ToList();
                foreach (var url in urls)
                {
                    HelperMethods.DeleteFile(url);
                }


                room.RoomImages = HelperMethods.
                    UrlImagesRoom(room.Id, _path, Model.Images);

                room.ImageUrl = room.RoomImages.First().ImageUrl;
                have = true;
            }

            var check = _RoomService.Update(room, have);
            if (!check)
                return BadRequest("Some Error ");

            return RedirectToAction("Show", "Hotel");
        }

        public async Task<IActionResult> SetOffer(string Id)
        {
            var room = await _RoomService.GetRoom(Id, true, true);
            if (room == null)
                return NotFound("This Resource not Found ");

            var user = await _userManager.GetUserAsync(User);

            if (user == null || user.Id != room.HotelId)
                return NotFound("This User Not Found");

            if (room.RoomOffer == null)
                room.RoomOffer = new RoomOffer
                {
                    RoomId = room.Id,
                };

            return View(room.RoomOffer);
        }
        [HttpPost]
        public async Task<IActionResult> SetOffer(RoomOffer offer)
        {
            if (!ModelState.IsValid)
            {
                return View(offer);
            }

            var room = await _RoomService.GetRoom(offer.RoomId);
            if (room == null)
                return NotFound("This Resource not Found ");

            var user = await _userManager.GetUserAsync(User);

            if (user == null || user.Id != room.HotelId)
                return NotFound("This User Not Found");

            var check = _RoomService.SetOffer(offer);
            if (!check)
                return BadRequest("Some Error ");

            return RedirectToAction("Show", "Hotel");
        }
        private Room MapUpdateRoom(UpDateCreateRoomViewModel model, Room room)
        {
            room.PricePerMonth = model.PricePerMonth;
            room.PricePerWeek = model.PricePerWeek;
            room.PricePerNight = model.PricePerNight;

            room.Type = model.Type;
            room.Number = model.Number;
            room.FloorNumber = model.FloorNumber;
            room.BedsNumber = model.BedsNumber;

            return room;
        }
        public async Task<IActionResult> ShowRoomSchedule(string roomId)
        {

            var data = await _RoomService.showRoomSchedules(roomId);

            return View(data);

        }


        public async Task <IActionResult> ShowTicket (int Tid)
        {
            var Data = await _userService.ShowDetailsBookingRoom(Tid); 

            return View(Data);      //ShowDetailsBookingRoom
        }


        [HttpPost]
        public async Task <IActionResult> HideNotHide (string Id)
        {
          await  _RoomService.HideNotHide(Id);
            return Json(new { success = true, message = "Done ..  " });
        }
    }

}
