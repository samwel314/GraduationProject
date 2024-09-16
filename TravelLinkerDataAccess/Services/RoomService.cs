using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TravelLinkerDataAccess.Data;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public class RoomService : IRoomService
    {
      
        private readonly ApplicationDbContext _context;
        public RoomService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Room ? >  GetRoom(string id , bool include = false, bool forOffer = false )
        {
            if (!include)
              return await _context.Rooms.AsNoTracking().Include(r=>r.RoomFeatures)
                    .Include(r => r.RoomImages)
                    .FirstOrDefaultAsync(r => r.Id == id);
           
            if (forOffer)
                return await _context.Rooms.AsNoTracking().Include(r=>r.RoomOffer)
                                .FirstOrDefaultAsync(r => r.Id == id);

            return await _context.Rooms.AsNoTracking()
                .Include(r=>r.RoomFeatures).
                     Include(r=>r.RoomImages)
                    .Include(r=>r.RoomOffer).
                       FirstOrDefaultAsync(r => r.Id == id);        
        }
        public bool Create(Room room)
        {
            room.ImageUrl = room.RoomImages.First().ImageUrl;  
            _context.Rooms.Add(room);   
            var rows = _context.SaveChanges();
            return rows > 0; 
        }

        public bool Update(Room room , bool haveimage = false)
        {
            Room? Model = null; 

            if (!haveimage)
            {
                Model =  _context.Rooms
                 .Include(x => x.RoomFeatures).
                 FirstOrDefault(x => x.Id == room.Id);
            }
            else
            {
                Model = _context.Rooms
                .Include(x => x.RoomImages)
                .Include(x => x.RoomFeatures).
                FirstOrDefault(x => x.Id == room.Id);
            }

            if (Model == null) 
            {
                return false;   
            }

            Model.PricePerWeek = room.PricePerWeek;
            Model.PricePerMonth = room.PricePerMonth;   
            Model.PricePerNight = room.PricePerNight;   
            Model.Number = room.Number; 
            Model.FloorNumber = room.FloorNumber;   
            Model.BedsNumber = room.BedsNumber;
            Model.Type = room.Type; 

            Model.RoomFeatures = room.RoomFeatures;
            if (haveimage)
            {
                Model.ImageUrl = room.ImageUrl;
                Model.RoomImages = room.RoomImages;
            }
           _context.Rooms.Update(Model);
            var rows =  _context.SaveChanges(); 
            return rows > 0; 
        }

        public bool SetOffer(RoomOffer offer)
        {

            var model = _context.RoomsOffers.
                FirstOrDefault(x => x.RoomId == offer.RoomId); 
            if (model == null) 
            { 
                _context.RoomsOffers.Add(offer);    
            }
            else
            {
                model.RoomId = offer.RoomId;    
                model.OfferPrice   = offer.OfferPrice;
                model.OfferDescription = offer.OfferDescription;    
                _context.RoomsOffers.Update(model); 
            }
            var rows = _context.SaveChanges();  
            return rows > 0;    
        }

        public async Task<IEnumerable<OverViewRoomViewModel>> GetAll(int Active, int pageSize, string imagePath, string City = null! )
        {
            var Data = _context.Rooms.Where(r=>r.IsHide == false).Select(r => new OverViewRoomViewModel
            {
                Id = r.Id,
                City = r.Hotel.User.City!,
                HotelName = r.Hotel.User.EnterpriseName , 
                HotelId = r.HotelId,
                Price = r.PricePerNight,
                Type = r.Type,
                MainImage = imagePath + $"/RoomApi/GetImage/{r.HotelId}/{r.ImageUrl}",
                OfferPrice = r.RoomOffer!.OfferPrice == 0 ? null : r.RoomOffer.OfferPrice
            }); 
         
            if (City != null )
                return await Data.Where(x => x.City == City).Skip(pageSize * Active).Take(pageSize).ToListAsync();

            return await Data.Skip(pageSize * Active).Take(pageSize).ToListAsync();
        }




        public int GetCount()
        {
            return _context.Rooms.Count(); 
        }

        public async Task<RoomDetailsVM?> Details(string Id, string domain)
        {
            return await _context.Rooms.Select( r => new RoomDetailsVM
            {
                Id = r.Id , 
                HotelId= r.HotelId, 
                Number = r.Number,
                HotelName = r.Hotel.User.EnterpriseName ,
                City = r.Hotel.User.City!,
                FloorNumber = r.FloorNumber,    
                Type = r.Type,  
                BedsNumber = r.BedsNumber,  
                OfferPrice = r.RoomOffer!.OfferPrice , 
                PricePerWeek = r.PricePerWeek , 
                PricePerNight = r.PricePerNight ,   
                PricePerMonth =r.PricePerMonth , 
                Features =  r.RoomFeatures.Select(rf => rf.FeatureName).ToList(),
                Images = r.RoomImages.Select(rm=> domain + $"/RoomApi/GetImage/{r.HotelId}/{rm.ImageUrl}").ToList(),
                OfferMessage = r.RoomOffer.OfferDescription
            }).FirstOrDefaultAsync(r=>r.Id == Id ); 
        }

        public int GetCountForCity(string City)
        {
            return _context.Rooms.Where(r => r.Hotel.User.City == City).Count(); 
        }



        public async Task<RoomTransaction> CreateTransaction(RoomTransaction transaction)
        {
            await _context.Set<RoomTransaction>().AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task UpdateTransaction(RoomTransaction transaction)
        {
            var model = await _context.Set<RoomTransaction>().FirstAsync(t => t.Id == transaction.Id);
            model.IsCompleted = transaction.IsCompleted;
            model.SessionId = transaction.SessionId;
            await _context.SaveChangesAsync();
        }

        public async Task<RoomTransaction?> GetTransaction(int id)
        {
            return await _context.Set<RoomTransaction>().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<IEnumerable<ShowRoomSchedule>> showRoomSchedules(string roomId)
        {
            return await _context.Set<RoomTransaction>().Where(rs => rs.RoomId == roomId)
                 .Select(rs => new ShowRoomSchedule
                 {
                     Id = rs.Schedule.Id,
                     CheckIn = rs.Schedule.WorkFrom!.Value.ToLongDateString(),
                     CheckOut = rs.Schedule.WorkTo!.Value.ToLongDateString(),
                     TrId = rs.Id
                 }).ToListAsync();

        }

        public async Task HideNotHide(string id)
        {
            var room = await _context.Rooms.FirstAsync(r => r.Id == id); 

            if (room.IsHide)
                room.IsHide = false;    
            else
                room.IsHide = true;
            await _context.SaveChangesAsync();      
        }
    }
}
