using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinker.Controllers
{
    [Authorize(policy: "IsCompany")]
    public class CompanyController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ICompanyService _companyService;
        public CompanyController(UserManager<ApplicationUser> userManager, ICompanyService  companyService)
        {
            _userManager = userManager;
            _companyService = companyService;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This User Not Found");
            var data = _companyService.GetAllTrips(user.Id);
            return View(data);
        }

        public async Task<IActionResult> DisPlayAll()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This User Not Found");
            var data = _companyService.GetAllTrips(user.Id);
            return View(data);
        }
        public async Task<IActionResult> Frindes(string City)
        {
            var user = await _userManager.GetUserAsync(User);

            if (string.IsNullOrEmpty(City))
            {
                City = user!.City!;
            }
            var data = new CityFrindesVM();
            data.City = City;
            data.Frindes = await _companyService.ShowFrindes(City, user.Id);
            return View(data);
        }
        public async Task<IActionResult> ViewFrind(string Id)
        {

            var data = await _companyService.LoadFriend(Id);

            return View(data);
        }
    }
}
