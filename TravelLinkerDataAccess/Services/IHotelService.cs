using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public interface IHotelService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"> For User  Which Will Be A Hotel </param>
        /// <returns></returns>
        Hotel Create(string id);
        public Task<ProfileViewModel> AdminOverView(string Id);
        Task<ProfileViewModel?> Profile(string Id , string imagePath);


        Task<FriendProfileVM> LoadFriend(string id); 
        Hotel  GetHotel(string id);
        
        IEnumerable<Room> GetHotelRooms (string id);
        

        Task<IEnumerable<ShowFrindes>> ShowFrindes(string City , string ID );      
        public Task ChanegDescription(string Id , string Description);
        public Task ChanegLocation(string Id, string Location);

    }
}
