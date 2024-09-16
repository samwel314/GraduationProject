using Microsoft.EntityFrameworkCore;
using TravelLinkerDataAccess.Data;
using TravelLinkerModels.Models;

namespace TravelLinkerDataAccess.Services
{
    public class RoomScheduleService : IRoomScheduleService
    {
        private readonly ApplicationDbContext _context;
        public RoomScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteSchedule(int id)
        {
            var Schedule = await _context.RoomSchedules.FirstOrDefaultAsync(SC => SC.Id == id);
            if (Schedule != null)
            {
                _context.RoomSchedules.Remove(Schedule);
                await _context.SaveChangesAsync();
            }

        }
        public async Task <RoomSchedule> Create(RoomSchedule  roomSchedule)
        {
            await _context.RoomSchedules.AddAsync(roomSchedule);
           await _context.SaveChangesAsync();
            return roomSchedule;
        }

        public async Task Delete()
        {
            var Schedule =  _context.Set<RoomTransaction>().Where(t => t.IsCompleted == false).Select(T => T.Schedule); 
      
                _context.RoomSchedules.RemoveRange(Schedule);      
                 await  _context.SaveChangesAsync();

        }

        public bool TryCreateSchedule(string roomId, DateTime workFrom, DateTime workTo)
        {
           
            var Schedules = _context.RoomSchedules.Where(rs => rs.RoomId == roomId); 

            if (Schedules.All(vs => vs.WorkFrom >= workTo) ||
                Schedules.All(vs => vs.WorkTo <= workFrom))
                return true;
            //
            var before = Schedules.Where(vs => vs.WorkTo <= workFrom).Count();
            var after = Schedules.Where(vs => vs.WorkFrom >= workTo).Count();

            if (before + after == Schedules.Count())
                return true;

            return false;
        }

       
    }
}
