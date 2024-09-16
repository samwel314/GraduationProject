using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using TravelLinker.Helpers;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinker.Controllers.ApiControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsCustomer")]
    public class RoomApiController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IHotelService _hotelService; 
        private readonly IWebHostEnvironment _webHost;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRoomScheduleService _roomScheduleService;
        private  string _domain; 
        public RoomApiController(IRoomService roomService, IWebHostEnvironment webHost , IHotelService hotelService , IRoomScheduleService roomScheduleService  , UserManager<ApplicationUser> userManager)
        {
            _domain = ""; 
            _roomService = roomService;
            _webHost = webHost;
            _hotelService = hotelService;       
            _roomScheduleService = roomScheduleService; 
            _userManager = userManager; 
        }


        [HttpGet ("GetAll/{Active=0}")]
        public async Task<IActionResult> Index(int Active = 0)
        {
            // 4 Page size 
           
            RoomHome homeOverview = new ();
            var DataCount = _roomService.GetCount();
            _domain = Request.Scheme + "://" + Request.Host.Value;
            homeOverview.Pagination = new PaginationModel(DataCount, 6, Active);
            homeOverview.Rooms = await _roomService.GetAll(homeOverview.Pagination.Active, 6, _domain); 

            return Ok(homeOverview);
        }
        [HttpGet("GetByCity/{City}/{Active=0}")]
        public async Task<IActionResult> GetByCity( string City , int Active = 0)
        {
            // 4 Page size 

            RoomHome homeOverview = new();
            var DataCount = _roomService.GetCountForCity(City);
            _domain = Request.Scheme + "://" + Request.Host.Value;
            homeOverview.Pagination = new PaginationModel(DataCount, 6, Active);
            homeOverview.Rooms = await _roomService.GetAll(homeOverview.Pagination.Active, 6, _domain  , City);

            return Ok(homeOverview);
        }
        [HttpGet("View/{Id}")]
        public async Task <IActionResult> ViewHotel (string Id)
        {
            _domain = Request.Scheme + "://" + Request.Host.Value;
            var Data = await _hotelService.Profile(Id, _domain); 

            if (Data != null)
            {
                Data.RateResult = HelperMethods.FindRateType(Data.Rate);
                return Ok(Data);
            }        


            return NotFound("Try Againe ");    
           
        }


        [HttpGet("GetImage/{folder}/{name}")]
        public IActionResult GetImage([FromRoute] string folder, [FromRoute] string name)
        {
            string path = Path.Combine(_webHost.WebRootPath, "Hotels", folder, name);

            // Check if the file exists
            if (!System.IO.File.Exists(path))
            {
                return NotFound(); // Or return any other appropriate response
            }

            byte[] fileContents = System.IO.File.ReadAllBytes(path);
            string contentType = $"image/{Path.GetExtension(name).TrimStart('.')}"; // Get the file extension and remove the leading dot

            return File(fileContents, contentType);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> RoomDetails ([FromRoute] string id )
        {
            _domain = Request.Scheme + "://" + Request.Host.Value;
            var room =await _roomService.Details(id , _domain);    
            if (room == null) 
                return NotFound();
                       
            return Ok(room);    
        }


        [HttpPost("PayNow")]
        public async Task<IActionResult> PayNow([FromBody] RoomPayDtO model)
        {
            await _roomScheduleService.Delete(); 
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return BadRequest();  // 401 
                                     

            var CanBook = _roomScheduleService.TryCreateSchedule(model.RoomId, model.CheckIn, model.CheckOut);

            // means Can not Booking 
            if (!CanBook)
                return StatusCode(200, "This Room Is Booking in The Time");

            var Schedule = await _roomScheduleService.Create(new RoomSchedule
            {
                RoomId = model.RoomId,
                WorkFrom = model.CheckIn,
                WorkTo = model.CheckOut,
            });

            var Transaction = await _roomService.CreateTransaction(new RoomTransaction
            {
                SessionId = "",
                RoomId = model.RoomId,
                UserId = user.Id,
                Amount = ((model.Total)),
                NumberOfDay = model.DaysNumber,
                ScheduleId = Schedule.Id
            });

            var Domain = Request.Scheme + "://" + Request.Host.Value;
            var options = new SessionCreateOptions
            {
                //Go To user Bid sssssssssooooooooooooooonnnnnnnnnnnn
                SuccessUrl = $"{Domain}/?sc_checkout=success",
                CancelUrl = $"{Domain}/TripApi/Details/?sc_checkout=cancel",
                Mode = "payment",
                LineItems = new()
            };

            var SessionLine = new SessionLineItemOptions
            {
                PriceData = new()
                {
                    UnitAmount = (long)((model.Total / model.DaysNumber) * 100),
                    Currency = "usd",

                    ProductData = new()
                    {
                        Name = model.Title
                    }
                },
                Quantity = model.DaysNumber

            };

            options.LineItems.Add(SessionLine);

            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            Transaction.SessionId = session.Id;
            await _roomService.UpdateTransaction(Transaction);

            // Go to pay Now >>>>>> 
            Response.Headers.Add("Location", session.Url);
            return StatusCode( 201,new
            {
                sessionid = session.Id,
                scheduleid = Schedule.Id,  // if he need to Cancel  i  will delete it 
                id = Transaction.Id
            });
        }

        [HttpGet("ConfirmTransaction/{Id}")]
        public async Task<IActionResult> ConfirmTransaction(int Id)
        {
            var Transaction = await _roomService.GetTransaction(Id);
            if (Transaction == null)
                return NotFound();

            var servies = new SessionService();
            Session session = servies.Get(Transaction.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                Transaction.IsCompleted = true;
                await _roomService.UpdateTransaction(Transaction);

                return Ok();
            }
            return BadRequest("PayError");
        }

        [HttpDelete("DeleteSchedule/{Id}")]
        public async Task<IActionResult> DeleteSchedule(int Id)
        {
            await _roomScheduleService.DeleteSchedule(Id);
            return Ok();
        }
    }
}
