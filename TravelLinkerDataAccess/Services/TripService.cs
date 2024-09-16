using Humanizer;
using Microsoft.EntityFrameworkCore;
using TravelLinkerDataAccess.Data;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{

    public class TripService : ITripService
    {
        private readonly ApplicationDbContext _context;
        public TripService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int GetCountForCity(string City)
        {
            return _context.Trips.Where(r => r.Company.User.City == City).Count();
        }

        private async Task <int> GetNumberOfTakits(string TripId)
        {
            return await _context.Set<TripTransaction>().Where(t => t.TripId == TripId && t.IsCompleted == true)
                .SumAsync(t => t.TicketsNumber); 
        }
        public async Task<TripDetailsVM?> Details(string Id, string domain)
        {
            var nuTc = await GetNumberOfTakits(Id); 

            return await _context.Trips.Select(t => new TripDetailsVM
            {
                Id = t.Id,
                AvailablePlaces = t.VehicleSchedule.Vehicle.Capacity - nuTc  , 
                CompanyId = t.CompanyId,
                Price = t.Price,
                Available = t.VehicleSchedule.Vehicle.Capacity - nuTc == 0 ? false : true,    
                To = t.To,
                Duration = t.Duration,  
                From = t.From,
                NameCompany = t.Company.User.EnterpriseName , 
                City = t.Company.User.City! ,
                Date = t.StartAt.ToLongDateString(),
                Deadline = t.LastDate.ToLongDateString() , 
                Capacity = t.VehicleSchedule.Vehicle.Capacity , 
                VehicleType = t.VehicleSchedule.Vehicle.Type ,
                Image = domain + $"/TripApi/GetImage/{t.CompanyId}/{t.VehicleSchedule.Vehicle.ImageUrl}" , 
                Features = t.VehicleSchedule.Vehicle.vehicleFeatures.Select(v => v.FeatureName).ToList(),
            }).FirstOrDefaultAsync(t => t.Id == Id);
        }
        

        public bool AnyCode(string Code , string CompanyId  , string tripId )
        {
            return _context.Trips.Where
                (t=>t.CompanyId == CompanyId).Any(t=>t.Code == Code && t.Id != tripId);    
        }

        public Trip  Create(Trip trip)
        {
             trip.Id = Guid.NewGuid().ToString();
            _context.Trips.Add(trip);  
            _context.SaveChanges(); 
            return trip;    
        }



     

        public Trip ?  GetTrip(string id , bool include = false    )
        {
            if (!include)
                return _context.Trips.AsNoTracking().FirstOrDefault(t=>t.Id == id );
    
            // may need that or we will remove it soon 
            return _context.Trips.AsNoTracking()
                .Include(t=>t.VehicleSchedule)
                .ThenInclude(vs=>vs.Vehicle).ThenInclude(v=>v.vehicleFeatures).FirstOrDefault(t => t.Id == id);
        }

        public async Task<IEnumerable<OverViewTripViewModel>> GetAll(int Active, int pageSize, string imagePath, string City = null! )
        {
            var Data = _context.Trips.Where(t=>t.LastDate >= DateTime.Now && t.IsHidden == false).Select(t => new OverViewTripViewModel
            {
                Id = t.Id,
                CompanyId = t.CompanyId,
                Price = t.Price,
                City = t.Company.User.City!,
                From = t.From,
                To = t.To,
                Date = t.StartAt.ToLongDateString(),
                Image = imagePath + $"/TripApi/GetImage/{t.CompanyId}/{t.VehicleSchedule.Vehicle.ImageUrl}"
            }); 


            if (City != null)
                return await Data.Where(x => x.City == City).Skip(pageSize * Active).Take(pageSize).ToListAsync();

            return await Data.Skip(pageSize * Active).Take(pageSize).ToListAsync();
        }

        public Trip ? Update( Trip trip)
        {
            var MFdb =   _context.Trips.FirstOrDefault(t => t.Id == trip.Id);
            if (MFdb == null)   
                return null;

            MFdb.LastDate = trip.LastDate;
            MFdb.From = trip.From;
            MFdb.To  =  trip.To;
            MFdb.Code = trip.Code;      
            MFdb.Price = trip.Price;        
            MFdb.StartAt = trip.StartAt;
            MFdb.Duration = trip.Duration;

         //   MFdb.VehicleSchedule = trip.VehicleSchedule;// --
            _context.SaveChanges(); 

            return MFdb;    
        }

        public int GetCount()
        {
            return _context.Trips.Count(); 
        }

        public async Task<TripTransaction> CreateTransaction(TripTransaction transaction)
        {
            await _context.Set<TripTransaction>().AddAsync(transaction);
            await _context.SaveChangesAsync();       
            return transaction; 
        }

        public async Task  UpdateTransaction(TripTransaction transaction)
        {
           var model =   await _context.Set<TripTransaction>().FirstAsync(t=> t.Id ==  transaction.Id);
            model.IsCompleted = transaction.IsCompleted;    
            model.SessionId = transaction.SessionId;    
            await _context.SaveChangesAsync();  
        }
        public async Task HideNotHide(string id)
        {
            var trip = await _context.Trips.FirstAsync(r => r.Id == id);

            if (trip.IsHidden)
                trip.IsHidden = false;
            else
                trip.IsHidden = true;
            await _context.SaveChangesAsync();
        }
        public  async Task <TripTransaction?> GetTransaction(int id)
        {
            return await  _context.Set<TripTransaction>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task DeleteTransaction(int Id)
        {
           var tr = await  _context.Set<TripTransaction>().FirstOrDefaultAsync(x => x.Id == Id);
            if (tr != null)
                _context.Set<TripTransaction>().Remove(tr);

            await _context.SaveChangesAsync();  

        }




        public async Task<IEnumerable<ShowDetailsTicket>> showAllTicket(string  Id)
        {
            return await _context.Set<TripTransaction>().Where(tr => tr.TripId == Id && tr.IsCompleted == true ).
                Select(tr => new ShowDetailsTicket
                {
                    Id = tr.Id,
                    ClientName = tr.User.EnterpriseName,
                    Owner = tr.Trip.Company.User.EnterpriseName,
                    Total = tr.Amount.ToString("c"),
                    From = tr.Trip.From,
                    To = tr.Trip.To,
                    Code = tr.Trip.Code,
                    StartDate = tr.Trip.StartAt.ToLongDateString(),
                    NumberOfTickets = tr.TicketsNumber
                }).ToListAsync();


        }

        public async Task<bool> CanUpdate(string Id  )
        {
            return await _context.Set<TripTransaction>().AnyAsync(tr => tr.TripId == Id && tr.IsCompleted); 
        }
    }
}
