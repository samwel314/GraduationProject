using TravelLinkerModels.Models;

namespace TravelLinkerDataAccess.Services
{
    public interface IRoomScheduleService
    {
       bool  TryCreateSchedule(string roomId ,DateTime workFrom, DateTime workTo);

        Task<RoomSchedule> Create(RoomSchedule  roomSchedule);
        Task DeleteSchedule(int id);
        Task Delete(); 
    }
}
