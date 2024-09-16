using System.Xml.Schema;
using Microsoft.EntityFrameworkCore;
using TravelLinkerDataAccess.Data;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public class VehicleScheduleService : IVehicleScheduleService
    {
        private readonly ApplicationDbContext _context;
        public VehicleScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public VehicleSchedule Create(VehicleSchedule vehicleSchedule)
        {
           _context.VehicleSchedules.Add(vehicleSchedule);      
            _context.SaveChanges(); 
            return vehicleSchedule;
        }

        public async Task<IEnumerable<ShowVehicleSchedule>> ShowVehicleSchedules(int Id)
        {
              return await _context.VehicleSchedules.Where(vs=> vs.VehicleId == Id).Select(vs => new ShowVehicleSchedule
              {
                  TripCode = vs.Trip.Code , 
                  WorkFrom = vs.WorkFrom!.Value.ToLongDateString() ,
                  WorkTo = vs.WorkTo!.Value.ToLongDateString(),
              })
                .ToListAsync();
        }

        public bool TryCreateSchedule
    (int vehicleId,
            DateTime workFrom, DateTime workTo, string tripId  = "", bool isnew = true)
        {
            //for vehicle with id = vehicleId
            IQueryable<VehicleSchedule> Schedules;

            // means new trip >> new Schedule 
            if (isnew)
                Schedules = _context.VehicleSchedules
                   .AsNoTracking().Where(v => v.VehicleId == vehicleId);
            else  // old trip may change it Schedule
                Schedules = _context.VehicleSchedules
                  .AsNoTracking().Where(v => v.VehicleId == vehicleId && v.TripId != tripId);
            // means new Schedule after all trips Schedule or before all trips Schedule

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

        public VehicleSchedule Update(VehicleSchedule vehicleSchedule)
        {
            var FDBmodel = _context.VehicleSchedules
                .FirstOrDefault(vs=>vs.TripId == vehicleSchedule.TripId);
            FDBmodel!.WorkFrom = vehicleSchedule.WorkFrom;
            FDBmodel!.WorkTo = vehicleSchedule.WorkTo;
            FDBmodel!.VehicleId = vehicleSchedule.VehicleId;
             _context.SaveChanges();    

            return vehicleSchedule;     
        }
    }
}
