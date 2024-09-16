using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public interface IUserService
    {
        Task<ProfileViewModel> GetUserByType(ApplicationUser applicationUser, string userType);

        Task<AuthenticationModel> RegisterUser(UserForRegistrationDto userForRegistration);

        Task<TokenDto> RefreshToken(TokenDto tokenDto);
        Task<AuthenticationModel> ValidateUser(UserForLoginDto userForAuth);


        Task CreateComment(Comment comment);



        Task CreateRate(Rate rate);

        Task UpdateRate(Rate rate);

        Task<Rate?> GetRate(Rate rate);

        /// <summary>
        ///  new from her 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="trip"></param>
        /// <returns></returns>
        Task<IEnumerable<ShowTransactionDto>> ShowTransactions(string Id, bool trip = true);

        Task<ShowDetailsTicket?> showDetailsTicket(int Id);

        Task<ShowDetailsBookingRoom?> ShowDetailsBookingRoom(int Id);



    }
}
