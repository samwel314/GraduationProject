using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelLinker.Helpers;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinker.Controllers
{
    [Authorize(policy: "IsCompany")]
    public class VehicleController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVehicleService _vehicleService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IVehicleScheduleService _vehicleScheduleService;
        private string _path;
        public VehicleController
            (UserManager<ApplicationUser> userManager, IVehicleService vehicleService , IWebHostEnvironment hostEnvironment , IVehicleScheduleService vehicleScheduleService )
        {
            _userManager = userManager;
            _vehicleService = vehicleService;
            _hostEnvironment = hostEnvironment;
            _vehicleScheduleService = vehicleScheduleService;
            _path = _hostEnvironment.WebRootPath + PathHelper.CompanyPath;
        }

        

        public async Task<IActionResult> GetVehicleSchedules (int Id)
        {
            var data = await _vehicleScheduleService.ShowVehicleSchedules(Id);        

            return View(data);            
        }




        //UpCreateVehicleVM              
        // Create --->>
        public async Task<IActionResult> UpCreate(int id = 0)    //id Vehicle >> Id 
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This User Not Found");
            // this Creata Requst 
            if (id == 0)
            {
                return View(new UpCreateVehicleVM
                {
                    CompanyId = user.Id,    
                });
            }
            var vehicle = _vehicleService.GetVehicle(id);
            if (vehicle == null)
                return NotFound(); 

            return View(new UpCreateVehicleVM
            {
                Id = vehicle.Id,    
                CompanyId = user.Id,
                Type = vehicle.Type,    
                Capacity = vehicle.Capacity,
                LicenseNumber = vehicle.LicenseNumber,  
                ImageUrl = vehicle.ImageUrl,    
                Features = vehicle.vehicleFeatures.Select(vf=>vf.FeatureName).ToList(), 
            });
        }
        // 
        [HttpPost]
        public async Task<IActionResult> UpCreate(UpCreateVehicleVM model )
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This User Not Found");
            if (!ModelState.IsValid)
                return View(model);
            if (_vehicleService.AnyLicenseNumber(model.LicenseNumber , model.Id) )
            {
                ModelState.AddModelError("LicenseNumber", "This License Number Is Used "); 
                return View(model);
            }

            Vehicle ? vehicle = new Vehicle
            {
                Id = model.Id,
                CompanyId = model.CompanyId,
                Type = model.Type,
                Capacity = model.Capacity,
                LicenseNumber =model.LicenseNumber, 
                vehicleFeatures = model.Features.Select (s=> new VehicleFeature
                {
                    VehicleId = model.Id,   
                    FeatureName = s 
                }).ToList(),    
            };
            _path = Path.Combine(_path, user.Id); 
            if (model.Id == 0 )
            {
                if (model.Image == null)
                {
                    ModelState.AddModelError("Image", "Add Image For This Vehicle ");
                    return View(model);
                }
                if (!HelperMethods.CheckExtension(model.Image))
                {
                    ModelState.AddModelError("Image", "We allow .jpg , .png files  ");
                    return View(model);
                }
                vehicle.ImageUrl = HelperMethods.UrlVecicleImage(_path, model.Image);
                vehicle =  _vehicleService.Create(vehicle); 
            }
            else
            {
                if (model.Image != null )
                {
                    vehicle.ImageUrl = HelperMethods.UrlVecicleImage(_path, model.Image);
                }
                if (model.ImageUrl != null ) 
                {
                    HelperMethods.DeleteFile(Path.Combine(_path, model.ImageUrl)); 
                }

                vehicle = _vehicleService.Update(vehicle);
            }
            if (vehicle == null)
                return NotFound();

            return RedirectToAction(nameof (DisplayAll) ); // soon
        }


        public async Task<IActionResult> Delete (int Id)
        {
          var chek =   await  _vehicleService.Delete(Id);

            if (chek)
                TempData["message"] = "Vehicle Deleted";
            else
                TempData["message"] = "Can not Delete This Vehicle It Have Live Trip";

            return RedirectToAction("DisplayAll"); 
        }
        public async Task<IActionResult>  DisplayAll ()
        {

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound("This User Not Found");
            var data = _vehicleService.GetAll(user.Id).Select(v => new ShowVehicleVm
            {
                Id = v.Id,
                Capacity = v.Capacity,
                LicenseNumber = v.LicenseNumber,
                Type = v.Type,
                CompanyId = v.CompanyId,    
            }).ToList();
            if (TempData["message"] == null)
                TempData["message"] = "Welcom Back !"; 

                return View(data);
        }

    }
}
