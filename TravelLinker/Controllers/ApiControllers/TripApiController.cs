using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using TravelLinker.Helpers;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinker.Controllers.ApiControllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme , Policy = "IsCustomer")]

    public class TripApiController : ControllerBase
    {
        private readonly ITripService _tripService;
        private readonly ICompanyService _companyService;       
        private readonly IWebHostEnvironment _webHost;
        private readonly UserManager<ApplicationUser> _userManager;
        private string _domain;
        public TripApiController(ITripService tripService, IWebHostEnvironment webHost , ICompanyService companyService , UserManager<ApplicationUser> userManager ) 
        {
            _domain = "";
            _tripService = tripService;
            _webHost = webHost;
            _companyService = companyService;   
            _userManager = userManager; 
        }
        [HttpGet("View/{Id}")]
        public async Task<IActionResult> ViewCompany(string Id)
        {

            _domain = Request.Scheme + "://" + Request.Host.Value;
            var Data = await _companyService.Profile(Id, _domain);
            if (Data != null)
            {
                Data.RateResult = HelperMethods.FindRateType(Data.Rate);
                return Ok(Data);
            }


            return NotFound("Try Againe ");

        }

        [HttpGet("GetAll/{Active=0}")]
        public async Task<IActionResult> Index(int Active = 0)
        {
            // 4 Page size 

            TripHome homeOverview = new();
            var DataCount = _tripService.GetCount();
            _domain = Request.Scheme + "://" + Request.Host.Value;
            homeOverview.Pagination = new PaginationModel(DataCount, 6, Active);
            homeOverview.Trips = await _tripService.GetAll(homeOverview.Pagination.Active, 6, _domain);

            return Ok(homeOverview);
        }

        [HttpGet("GetByCity/{City}/{Active=0}")]
        public async Task<IActionResult> GetByCity( string City , int Active = 0)
        {
            // 4 Page size 

            TripHome homeOverview = new();
            var DataCount = _tripService.GetCountForCity(City);
            _domain = Request.Scheme + "://" + Request.Host.Value;
            homeOverview.Pagination = new PaginationModel(DataCount, 6, Active);
            homeOverview.Trips = await _tripService.GetAll(homeOverview.Pagination.Active, 6, _domain , City);

            return Ok(homeOverview);
        }

        [HttpGet("GetImage/{folder}/{name}")]
        public IActionResult GetImage([FromRoute] string folder, [FromRoute] string name)
        {
            string path = Path.Combine(_webHost.WebRootPath, "Companies", folder, name);

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
        public async Task<IActionResult> TripDetails([FromRoute] string id)
        {
            _domain = Request.Scheme + "://" + Request.Host.Value;
            var trip = await _tripService.Details(id, _domain);
            if (trip == null)
                return NotFound();

            return Ok(trip);
        }

        [HttpPost("PayNow")]
        public async Task<IActionResult> PayNow([FromBody] TripsPayDtO model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return BadRequest();  // 401 


            var Transaction = await _tripService.CreateTransaction(new TripTransaction
            {
                SessionId = "",
                TripId = model.TripId,
                UserId = user.Id,
                Amount = ((model.Total)),
                TicketsNumber = model.TicketsNumber,
            });

            var Domain = Request.Scheme + "://" + Request.Host.Value;
            var options = new SessionCreateOptions
            {
                //Go To user Bid sssssssssooooooooooooooonnnnnnnnnnnn
                SuccessUrl = $"{Domain}/?sc_checkout=success" , 
                CancelUrl = $"{Domain}/TripApi/Details/?sc_checkout=cancel", 
                Mode = "payment",
                LineItems = new()

            };

            var SessionLine = new SessionLineItemOptions
            {
                PriceData = new()
                {
                    UnitAmount = (long)((model.Total / model.TicketsNumber) * 100),
                    Currency = "usd",

                    ProductData = new()
                    {
                        Name = model.Title
                    }
                },
                Quantity = model.TicketsNumber

            };

            options.LineItems.Add(SessionLine);

            var service = new SessionService();
            Session session = await service.CreateAsync(options);
            Transaction.SessionId = session.Id;
            await _tripService.UpdateTransaction(Transaction);


            // Go to pay Now >>>>>> 
            Response.Headers.Add("Location", session.Url);
            return Ok(new
            {
                sessionid = session.Id , 
                id = Transaction.Id
            });

        }



        //[HttpPost("PayNow")]
        //public async Task<IActionResult> PayNow([FromBody] TripsPayDtO model)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //        return BadRequest();  // 401 

        //    var paymentIntentOptions = new PaymentIntentCreateOptions
        //    {
        //        Amount = (long )model.Total, 
        //        Currency = "eur",
        //        PaymentMethodTypes = new List<string>
        //        {
        //            "card"
        //        },
        //        // In the latest version of the API, specifying the `automatic_payment_methods` parameter
        //        // is optional because Stripe enables its functionality by default.

        //    };

        //    var paymentIntentService = new PaymentIntentService();
        //    PaymentIntent paymentIntent = paymentIntentService.Create(paymentIntentOptions);


        //    var Transaction = await _tripService.CreateTransaction(new TripTransaction
        //    {
        //        SessionId = paymentIntent.ClientSecret,
        //        TripId = model.TripId,
        //        UserId = user.Id,
        //        Amount = (long)(model.Total) * 100,
        //        TicketsNumber = model.TicketsNumber,
        //    });


        //    return Ok(new
        //    {
        //        clientsecret = paymentIntent.ClientSecret,
        //        amount = (long)(model.Total) 
        //    });

        //}
        [HttpGet("ConfirmTransaction/{Id}")]
        public async Task <IActionResult> ConfirmTransaction(int Id)
        {
            var Transaction = await _tripService.GetTransaction(Id);
            if (Transaction == null)
                return NotFound();

            var servies = new SessionService();
            Session session = servies.Get(Transaction.SessionId);

            if (session.PaymentStatus.ToLower() == "paid")
            {
                Transaction.IsCompleted = true;
                await _tripService.UpdateTransaction(Transaction);
                return Ok(); 
            }
            else
            {
               await _tripService.DeleteTransaction(Transaction.Id); 
            }

            return BadRequest("Pay Error");
        }

        [HttpDelete("DeleteTransaction/{Id}")]
        public async Task<IActionResult> DeleteTransaction(int Id)
        {
             await  _tripService.DeleteTransaction(Id);

            return Ok(); 

        }

    }
}
