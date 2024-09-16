using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public interface ICompanyService
    {

        Task<int> GetCountComment(string Id);
        Task<IEnumerable<ViewCommentDTO>> GetEnterpriseComments(int Active, int pageSize, string Id); 
        public Task<FriendProfileVM> LoadFriend(string id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"> For User  Which Will Be A Company </param>
        /// <returns></returns>
        Company Create(string id);
        Task<IEnumerable<ShowFrindes>> ShowFrindes(string City, string ID);
        Company GetCompany(string id);
        IEnumerable<Trip> GetAllTrips(string id, bool include = false);
        public Task ChanegLocation(string Id, string Location);
        public Task ChanegDescription(string Id, string Description);
        Task<ProfileViewModel?> Profile(string Id, string imagePath);
        public Task<ProfileViewModel> AdminOverView(string Id);
    }
}
