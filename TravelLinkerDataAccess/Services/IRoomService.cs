using System.Numerics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public interface IRoomService
    {

       Task< Room?> GetRoom(string id, bool include = false, bool forOffer = false  );
        bool Create(Room  room );
        Task<RoomDetailsVM ?> Details (string Id , string domain);
        int GetCount();
        Task<IEnumerable<ShowRoomSchedule>> showRoomSchedules(string roomId);
        int GetCountForCity(string City); 
        bool Update(Room room , bool haveimage = false);
        bool SetOffer (RoomOffer offer);
        Task<IEnumerable<OverViewRoomViewModel>> GetAll(int pageSize, int Active, string imagePath , string City = null!);

        Task HideNotHide(string id); 

        Task<RoomTransaction> CreateTransaction(RoomTransaction transaction);

        Task<RoomTransaction?> GetTransaction(int id);
        Task UpdateTransaction(RoomTransaction transaction);

    }
}
