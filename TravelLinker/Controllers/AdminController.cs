using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelLinker.Helpers;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinker.Controllers
{
  //  [Authorize(policy: "IsAdmin")]
    public class AdminController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ICompanyService _companyService;
        private readonly IHotelService _hotelService;
        public AdminController(UserManager<ApplicationUser> userManager, ICompanyService companyService, IHotelService hotelService)
        {
            _userManager = userManager;
            _companyService = companyService;
            _hotelService = hotelService;
        }


        public async Task<IActionResult> ShowEnterprise(string Id, string Type)
        {

            ProfileViewModel model = new ProfileViewModel();
            if (Type == StaticData.CompanyType)
            {
                model = await _companyService.AdminOverView(Id);
                model.Type = Type;
            }
            else
            {
                model = await _hotelService.AdminOverView(Id);
                model.Type = Type;
            }

            model.RateResult = HelperMethods.FindRateType(model.Rate);

            return View(model);


        }
        public async Task<IActionResult> Dashbord()
        {
            var Hotels = await _userManager.GetUsersForClaimAsync
                (new Claim(StaticData.UserTypeClaim, StaticData.HotelType));

            var Companies = await _userManager.GetUsersForClaimAsync
                (new Claim(StaticData.UserTypeClaim, StaticData.CompanyType));


            return View(new AdminDashbord
            {
                Hotels = Hotels.ToList(),
                Companies = Companies.ToList(),
            });
            //                    <button id="@data.Id" data-Id="@data.Id" data-lock="@(data.ISLocked ? 1 : 0)" class='clock btn @(data.ISLocked == true ? "btn-success" : "btn-warning")   fs-5  fw-bold'><i class=' @(data.ISLocked == true ? "bi bi-unlock" : "bi bi-lock-fill")'></i> @(data.ISLocked == true ? " UnLock" : " Lock")</button>

            /*
          

             
             * */











        }//--<-|->
        [HttpPost]//<//--//>
        public async Task<IActionResult> LockUnLock(string Id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == Id);
            if (user == null)
                return Json(new { success = false, message = "user Not Found  " });

            if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now; // 
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddDays(1);
            }
            await _userManager.UpdateAsync(user);
            return Json(new { success = true, message = "Done ..  " });
        }


    }
}

