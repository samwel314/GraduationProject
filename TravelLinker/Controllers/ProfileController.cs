using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TravelLinker.Helpers;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinker.Controllers
{
    [Authorize (policy: "IsEnterprise")]
    public class ProfileController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IUserService _userService;

        private readonly IHotelService _hotelService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string _path;
        private readonly ICompanyService _companyService;
        public ProfileController
            (UserManager<ApplicationUser> userManager, IUserService userService, IHotelService hotelService , ICompanyService companyService , IWebHostEnvironment webHostEnvironment )
        {
            _userManager = userManager;
            _userService = userService;
            _hotelService = hotelService;
            _companyService = companyService;
            _hostEnvironment = webHostEnvironment;
            _path = ""; 
        }


        // service for Hotel , Company 
        [HttpGet]
        public async Task<IActionResult> ViewComments (string Id)
        {
            var Data =await _companyService.GetEnterpriseComments(0, 0, Id); 
            return View(Data); 
        }

        public async Task<IActionResult> Index()
        {
            // Check If is this user in Me db And no Problem IN Authentication Cooke 

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This User Not Found");

            var UserType =
                  User.Claims.First(x => x.Type == StaticData.UserTypeClaim).Value;
            // Get This User 
            var model = await _userService.GetUserByType(user, UserType);
            var DataCount = await _companyService.GetCountComment(user.Id);
       
            var Pagination = new PaginationModel(DataCount, 4 , 0);
            Random random = new Random();

            var value = Pagination.PagesCount; 
            value = value == 0   ? 0 : value  ;       
              var r = random.Next(0, value);

            model.RateResult =  HelperMethods.FindRateType(model.Rate);

            model.ViewComments =
                await _companyService.GetEnterpriseComments(r, 4, user.Id); 
            return View(model);
        }
        [HttpPost ]
        public async Task<IActionResult> ChangeProfileImage(ProfileViewModel Model )
        {
            var UserType =
             User.Claims.First(x => x.Type == StaticData.UserTypeClaim).Value;
            if (UserType == StaticData.HotelType)
            {
                _path = _hostEnvironment.WebRootPath + PathHelper.HotelPath;
            }
            else
            {
                _path = _hostEnvironment.WebRootPath + PathHelper.CompanyPath;
            }
         
            _path = Path.Combine(_path, Model.Id!);
            if (Model.ProfileImage != null)
            {
                HelperMethods.DeleteFile(Path.Combine(_path , Model.ProfileImage));  
            }

           

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }

            var name = Guid.NewGuid().ToString() + Path.GetExtension(Model.ImageToChaneg.FileName);
            using FileStream stream =
                     new FileStream(Path.Combine(_path, name), FileMode.Create);
             await   Model.ImageToChaneg.CopyToAsync(stream);


            var user = await _userManager.GetUserAsync(User);
            if (user!= null )
            {
                user.ProfileImage = name;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction(nameof(Index)); 
        }

        public async Task<IActionResult> ChangeDescription(ProfileViewModel Model)
        {

            // we need  new View Called Error 
            //if (Model.Description != null && Model.Description.Length > 200)
            //{
            //    ModelState.AddModelError("Description", "Max allowed 200 Character");
            //    return View(Model);
            //}


            var UserType =
                 User.Claims.First(x => x.Type == StaticData.UserTypeClaim).Value;
            if (UserType == StaticData.HotelType)
            {
               await _hotelService.ChanegDescription(Model.Id!, Model.Description!); 
            }
            else
            {
                await _companyService.ChanegDescription(Model.Id!, Model.Description!);
            }


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeLocation(ProfileViewModel Model)
        {
            //if (Model.Location != null && Model.Location.Length > 100)
            //{
            //    ModelState.AddModelError("Location", "Max allowed 100 Character"); 
            //    return View(Model);  
            //}
            var UserType =
                 User.Claims.First(x => x.Type == StaticData.UserTypeClaim).Value;
            if (UserType == StaticData.HotelType)
            {
                await _hotelService.ChanegLocation(Model.Id!, Model.Location!);
            }
            else
            {
                await _companyService.ChanegLocation(Model.Id!, Model.Location!);
            }


            return RedirectToAction(nameof(Index));
        }

    }
}
