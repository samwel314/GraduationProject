using Microsoft.EntityFrameworkCore;
using TravelLinkerDataAccess.Data;
using TravelLinkerModels.Models;

namespace TravelLinkerDataAccess.Services
{
    public class VehicleService : IVehicleService
    {

        private readonly ApplicationDbContext _context;
        public VehicleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool AnyLicenseNumber(string LicenseNumber , int id  )
        {
            return _context.Vehicles.Any(v => v.LicenseNumber == LicenseNumber && v.Id != id); 
        }

        public Vehicle Create(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle); 
            _context.SaveChanges(); 
            return vehicle;
        }

        public async Task<bool> Delete(int Id)
        {
            var chek = await _context.VehicleSchedules
                .AnyAsync(v => v.VehicleId == Id  && v.WorkTo >= DateTime.Now);

            if (chek)
                return false;
            
            var model = _context.Vehicles.FirstOrDefault(v => v.Id == Id);
            if (model != null)
            {
                _context.Remove(model); 
              await   _context.SaveChangesAsync();        
            }
            return true ;       
        }

        public IEnumerable<Vehicle> GetAll(string id, bool include = false)
        {
            if (!include)
              return   _context.Vehicles.AsNoTracking().Where(v =>v.CompanyId == id).ToList();

            return _context.Vehicles.AsNoTracking().Where(v => v.CompanyId == id).Include(v=>v.VehicleSchedules).ToList();
        }

        public Vehicle ? GetVehicle(int id , bool include = false)
        {
            Vehicle ? FDBmodel; 
            if (!include) 
            {
                FDBmodel = _context.Vehicles.AsNoTracking().Include(v=>v.vehicleFeatures)
                   .FirstOrDefault(v => v.Id == id);

                return FDBmodel;
            }
             FDBmodel = _context.Vehicles.AsNoTracking().Include(v => v.vehicleFeatures).Include(v=>v.VehicleSchedules)
                     .FirstOrDefault(v => v.Id == id);

            return FDBmodel;
        }

        public Vehicle Update(Vehicle vehicle)
        {
            var FDBmodel =   _context.Vehicles.Include(v => v.vehicleFeatures)  
                .FirstOrDefault(v=>v.Id == vehicle.Id);

            if (FDBmodel == null)
            {
                return null!; 
            }
            FDBmodel.ImageUrl = vehicle.ImageUrl;   
            FDBmodel.vehicleFeatures = vehicle.vehicleFeatures;
            FDBmodel.Type = vehicle.Type;
            FDBmodel.Capacity = vehicle.Capacity;   
            FDBmodel.LicenseNumber = vehicle.LicenseNumber;
            _context.SaveChanges  ();
            return FDBmodel; 

        }
    }
}
