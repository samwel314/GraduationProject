using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelLinkerDataAccess.Services;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinker.Controllers.ApiControllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICompanyService _companyService;
        private readonly IRoomScheduleService _roomScheduleService; 
        public AuthenticationController(IUserService service, UserManager<ApplicationUser> userManager, ICompanyService companyService , IRoomScheduleService roomScheduleService)
        {
            _service = service;
            _userManager = userManager;
            _companyService = companyService;
            _roomScheduleService = roomScheduleService; 
        }
        [HttpPost("Register")]//-**
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto
           userForRegistration)
        {
            var result = await _service.RegisterUser(userForRegistration);

            if (result.IsAuthenticated)
                return StatusCode(201, result);

            return BadRequest(result.Message);

        }

        [HttpPost("Login")]//-**
        public async Task<IActionResult> LoginUser([FromBody] UserForLoginDto
      forLoginDto)
        {
            var result = await _service.ValidateUser(forLoginDto);

            if (result.IsAuthenticated)
                return StatusCode(201, result);

            return BadRequest(result.Message);

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsCustomer")]
        [HttpPost("Comment")]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return BadRequest();  // 401 

            comment.FromId = user.Id;

            await _service.CreateComment(comment);

            return Ok();
        }
        [HttpGet("Comments/{Id}")]

        public async Task<IActionResult> ViewComments(string Id)
        {
            var Data = await _companyService.GetEnterpriseComments(0, 0, Id);
            return Ok(Data);
        }

        [HttpPost("Rate")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsCustomer")]

        public async Task<IActionResult> CreateRate([FromBody] Rate rate)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();  // 401 

            rate.FromId = user.Id;

            var Check = await _service.GetRate(rate);

            if (Check == null)
                await _service.CreateRate(rate);
            else
                await _service.UpdateRate(rate);

            return Ok();
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshToken(TokenDto dto)
        {
            var Data = await _service.RefreshToken(dto);
            if (Data.Token == null)
                return Unauthorized();
            return Ok(Data);
        }





        [HttpGet("Transactions/{trip}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsCustomer")]

        public async Task<IActionResult> ShowTickets([FromRoute] bool trip)
        {
            await _roomScheduleService.Delete();
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();  // 401 


            var Data = await _service.ShowTransactions(user.Id, trip);
            return Ok(Data);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsCustomer")]

        [HttpGet("DetailsTicket/{id}")]
        public async Task<IActionResult> DetailsTicket([FromRoute] int id)
        {
            var Data = await _service.showDetailsTicket(id);

            if (Data == null)
                return NotFound();

            return Ok(Data);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsCustomer")]

        [HttpGet("DetailsBookingRoom/{id}")]
        public async Task<IActionResult> DetailsBookingRoom([FromRoute] int id)
        {
            var Data = await _service.ShowDetailsBookingRoom(id);

            if (Data == null)
                return NotFound();

            return Ok(Data);

        }
    }

}
