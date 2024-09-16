using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TravelLinkerDataAccess.Data;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public class UserService : IUserService
    {

        private IHotelService _hotelService;
        private ICompanyService _companyService;
        private UserManager<ApplicationUser> _userManager;
        private IConfiguration _configuration;
        private ApplicationUser? _user;
        private ApplicationDbContext _db;
        public UserService(IHotelService hotelService, ICompanyService companyService, UserManager<ApplicationUser> userManager, IConfiguration configuration, ApplicationDbContext dbContext)
        {

            _hotelService = hotelService;
            _companyService = companyService;
            _userManager = userManager;
            _configuration = configuration;
            _db = dbContext;
        }

        public async Task<ProfileViewModel> GetUserByType(ApplicationUser applicationUser, string userType)
        {
            dynamic? user = null;
            var model = new ProfileViewModel();
            if (userType == "Hotel")
            {
                user = _hotelService.GetHotel(applicationUser.Id);
            }
            else if (userType == "TransportCompany")
            {
                user = _companyService.GetCompany(applicationUser.Id);
            }
            // map Hotel Or Company 
            //Learn About AutoMapper  NuGet Package


            model.Reviews = await _db.Set<Rate>().Where(r => r.ToId == applicationUser.Id).CountAsync();

            var sum = await _db.Set<Rate>().Where(r => r.ToId == applicationUser.Id).SumAsync(r => r.Value);

            if (model.Reviews == 0)
                model.Rate = 0;
            else
                model.Rate = (double)sum / model.Reviews;

            model.City = user!.City;
            model.Location = user!.Location;
            
            model.ProfileImage = applicationUser!.ProfileImage;

            model.Description = user!.Description;
            if (userType == "Hotel")
                model.Stars = user!.Stars;

            model = MapData(applicationUser, model);
            return model;
        }

        private ProfileViewModel MapData(ApplicationUser user, ProfileViewModel model)
        {
            model.Id = user.Id;
            model.PhoneNumber = user.PhoneNumber!;
            model.Email = user.Email!;
            model.EnterpriseName = user.EnterpriseName;
            //  model.ProfileImage = user.ProfileImage;    
            return model;
        }

        public async Task<AuthenticationModel> RegisterUser(UserForRegistrationDto
  userForRegistration)
        {
            var Auth = new AuthenticationModel();
            var user = new ApplicationUser()
            {
                UserName = userForRegistration.Email,
                Email = userForRegistration.Email,
                City = userForRegistration.City,
            };
            user.EnterpriseName = $"{userForRegistration.FirstName} {userForRegistration.LastName}";
            var result = await _userManager.CreateAsync(user, userForRegistration.Password!);

            if (result.Succeeded)
            {
                Claim UserType = new Claim("UserType", "Customer");
                await _userManager.AddClaimAsync(user, UserType);
                Auth.IsAuthenticated = true;
                Auth.City = userForRegistration.City;
                Auth.Message = $"Hello {userForRegistration.FirstName}";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    Auth.Message += error.Description;
                }
            }
            //------*
            return Auth;
        }

        public async Task<AuthenticationModel> ValidateUser(UserForLoginDto userForAuth)
        {
            var Aut = new AuthenticationModel();
            _user = await _userManager.FindByEmailAsync(userForAuth.Email!);
            var result = (_user != null && await _userManager.
                CheckPasswordAsync(_user, userForAuth.Password!));
            if (!result)
            {
                Aut.Message = "invalid email or password";
            }
            else
            {
                Aut.IsAuthenticated = true;
                Aut.Message = $"Hi {_user!.EnterpriseName.Split(" ")[0]}";
                var AccessToken = await CreateToken();
                Aut.Token = await CreateToken();
                Aut.City = _user.City;
                Aut.RefreshToken = GenerateRefreshToken();
                _user.RefreshToken = Aut.RefreshToken;
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
                await _userManager.UpdateAsync(_user);
            }
            return Aut;
        }





        private async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:key"]!);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>();


            var UserClaimes = await _userManager.GetClaimsAsync(_user!);
            foreach (var claim in UserClaimes)
            {

                claims.Add(claim);
            }
            claims.Add(new Claim(ClaimTypes.NameIdentifier, _user!
                .Id));
            claims.Add(new Claim(ClaimTypes.Name, _user.UserName!));
            return claims;
        }


        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials,
                List<Claim> claims)
        {

            var tokenOptions = new JwtSecurityToken
            (
            issuer: _configuration["JwtSettings:validIssuer"],
            audience: _configuration["JwtSettings:validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:expires"])),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }


        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
        // find the user 
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var TokenValidationParameters = new TokenValidationParameters
            {

                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _configuration["JwtSettings:validIssuer"],
                ValidAudience = _configuration["JwtSettings:validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:key"]!))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, TokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null ||
           !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }


        public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.Token);
            var user = await _userManager.FindByNameAsync(principal!.Identity!.Name!);
            if (user == null || user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new TokenDto
                {
                    Token = null!,
                    RefreshToken = null!
                };
            }

            _user = user;

            var NewOne = await CreateToken();

            var RefreshToken = GenerateRefreshToken();
            _user.RefreshToken = RefreshToken;
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(_user);
            return new TokenDto
            {
                Token = NewOne,
                RefreshToken = RefreshToken,
            };
        }











        /// <summary>
        /// //
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task CreateComment(Comment comment)
        {
            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();
        }

        public async Task CreateRate(Rate rate)
        {
            await _db.Set<Rate>().AddAsync(rate);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateRate(Rate rate)
        {
            var data = await _db.Set<Rate>().FirstAsync(r => r.FromId == rate.FromId && r.ToId == rate.ToId);
            data.Value = rate.Value;
            await _db.SaveChangesAsync();
        }

        public async Task<Rate?> GetRate(Rate rate)
        {
            return await _db.Set<Rate>().FirstOrDefaultAsync(r => r.FromId == rate.FromId && r.ToId == rate.ToId);
        }


        public async Task<IEnumerable<ShowTransactionDto>> ShowTransactions(string Id, bool trip = true)
        {
            if (trip)
                return await _db.Set<TripTransaction>()
                    .Where(trt => trt.UserId == Id && trt.IsCompleted == true).Select(trt => new ShowTransactionDto
                    {
                        CreatedAt = trt.CreatedAt.ToLongDateString(),
                        Id = trt.Id
                    })
                    .ToListAsync();


            return await _db.Set<RoomTransaction>()
                    .Where(trt => trt.UserId == Id).Select(trt => new ShowTransactionDto
                    {
                        CreatedAt = trt.CreatedAt.ToLongDateString(),
                        Id = trt.Id
                    })
                    .ToListAsync();
        }

        public async Task<ShowDetailsTicket?> showDetailsTicket(int Id)
        {
            return await _db.Set<TripTransaction>().Where(tr => tr.Id == Id  ).
                Select(tr => new ShowDetailsTicket
                {
                    Id = tr.Id,
                    ClientName = tr.User.EnterpriseName,
                    Owner = tr.Trip.Company.User.EnterpriseName,
                    Total = tr.Amount.ToString("c"),
                    From = tr.Trip.From,
                    To = tr.Trip.To,
                    Code = tr.Trip.Code,
                    StartDate = tr.Trip.StartAt.ToLongDateString(),
                    NumberOfTickets = tr.TicketsNumber
                }).FirstOrDefaultAsync();


        }


        public async Task<ShowDetailsBookingRoom?> ShowDetailsBookingRoom(int Id)
        {
            return await _db.Set<RoomTransaction>().Where(tr => tr.Id == Id).
                Select(tr => new ShowDetailsBookingRoom
                {
                    Id = tr.Id,
                    ClientName = tr.User.EnterpriseName,
                    Owner = tr.Room.Hotel.User.EnterpriseName,
                    Total = tr.Amount.ToString("c"),
                    CheckIn = tr.Schedule.WorkFrom!.Value.ToLongDateString(),
                    CheckOut = tr.Schedule.WorkTo!.Value.ToLongDateString(),
                    ScheduleId = tr.ScheduleId,
                    NumberOfDays = tr.NumberOfDay,
                    Floor = tr.Room.FloorNumber,
                    Number = tr.Room.Number,
                    Type = tr.Room.Type,

                }).FirstOrDefaultAsync();







        }
    }


}
