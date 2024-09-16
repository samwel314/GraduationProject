using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public interface IVehicleScheduleService
    {
        bool TryCreateSchedule
    (int vehicleId, DateTime workFrom, DateTime workTo, string tripId = "", bool isnew = true);

        VehicleSchedule Create(VehicleSchedule  vehicleSchedule);
        VehicleSchedule Update(VehicleSchedule vehicleSchedule);

        Task<IEnumerable<ShowVehicleSchedule>> ShowVehicleSchedules(int Id);

    }
}
