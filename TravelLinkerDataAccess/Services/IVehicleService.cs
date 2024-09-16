using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public interface IVehicleService
    {
        Vehicle  ? Create(Vehicle vehicle);
        Vehicle ?  Update(Vehicle vehicle);
        Vehicle ? GetVehicle(int id  , bool include = false);

        IEnumerable<Vehicle> GetAll(string id  ,bool include = false);


        bool AnyLicenseNumber(string LicenseNumber , int id );


        Task<bool> Delete(int Id);
    }
}
