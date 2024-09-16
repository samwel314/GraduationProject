using System;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using TravelLinkerDataAccess.Data;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public class HotelService : IHotelService
    {
        private readonly ApplicationDbContext _context;

        public HotelService(ApplicationDbContext context )
        {
            _context = context;
 
        }

        public Hotel Create(string id)
        {
            var Hotel = new Hotel()
            {
                Id = id         
            };    
            _context.Hotels.Add(Hotel);
            _context.SaveChanges ();    
            return Hotel;       
        }
        public Hotel GetHotel(string id)
        {
            return _context.Hotels.
                AsNoTracking().Select(h => new Hotel
                {
                    Id = h.Id,
                    City = h.User.City,
                    Description = h.Description,
                    Rate = h.Rate,
                    Location = h.Location,
                    Stars = h.Stars,    
                }).First(h => h.Id == id);
        }
        public IEnumerable<Room> GetHotelRooms(string id)
        {
            return
                _context.Rooms.AsNoTracking()
                .Include(x=>x.RoomOffer).
                Where(r => r.HotelId == id).ToList();    
        }
        public  async Task  ChanegDescription(string Id , string  Description )
        {
             var Hotel = _context.Hotels.First( Hotel => Hotel.Id ==Id);     

             Hotel.Description = Description;        
             
            await  _context.SaveChangesAsync(); 
        }


        public async Task ChanegLocation(string Id, string Location)
        {
            var Hotel = _context.Hotels.First(Hotel => Hotel.Id == Id);

            Hotel.Location = Location;

            await _context.SaveChangesAsync();
        }

  

        public async Task<ProfileViewModel?> Profile(string Id  , string imagePath)
        {
            //
           var  Data =  await  _context.Hotels.Select(h=> new ProfileViewModel
          {
              Id = h.Id , 
              EnterpriseName = h.User.EnterpriseName , 
              Email = h.User.Email ,    
              PhoneNumber = h.User.PhoneNumber ,
              City = h.User.City ,  
              ProfileImage = h.User.ProfileImage == null ? null : imagePath + $"/RoomApi/GetImage/{h.Id}/{h.User.ProfileImage}" , 
              Location = h.Location! , 
              Description  = h.Description! , 
              Rate = h.Rate , 
              Stars = h.Stars , 
          } ).FirstOrDefaultAsync(h => h.Id == Id);
            if (Data != null )
            {


                Data.Reviews = await _context.Set<Rate>().Where(r => r.ToId == Id).CountAsync();

                var sum = await _context.Set<Rate>().Where(r => r.ToId == Id).SumAsync(r => r.Value);

                if (Data.Reviews == 0)
                    Data.Rate = 0;
                else
                    Data.Rate = (double)sum / Data.Reviews;

                

                var DataCount = await GetCountComment(Id);

                var Pagination = new PaginationModel(DataCount, 4, 0);
                Random random = new Random();

                var value = Pagination.PagesCount;
                value = value == 0 ? 0 : value;
                var r = random.Next(0, value);


                Data!.ViewComments = await GetEnterpriseComments(r, 4, Id);
            }


            return Data; 
        }

        public async Task<IEnumerable<ShowFrindes>> ShowFrindes(string City , string ID)
        {
            return await _context.Hotels.Where(h => h.User.City == City &&  h.Id != ID).Select(h => new ShowFrindes
            {
                Id = h.Id , 
                Name = h.User.EnterpriseName , 
                Image = h.User.ProfileImage!,
                Email = h.User.Email! , 
                Location = h.Location!
            }).ToListAsync();  
        }

        private async Task<IEnumerable<ViewCommentDTO>> GetEnterpriseComments(int Active, int pageSize, string Id)
        {
            var data = _context.Comments.Where(c => c.ToId == Id).Select(c => new ViewCommentDTO
            {
                UserName = c.From.EnterpriseName!,
                Email = c.From.Email!,
                Content = c.Content
            }).Skip(pageSize * Active);
            if (pageSize == 0)
                return await data.ToListAsync();

            return await data.Take(pageSize).ToListAsync();
        }

        private async Task<int> GetCountComment(string Id)
        {
            return await _context.Comments.Where(c => c.ToId == Id).CountAsync();
        }

        public async Task<FriendProfileVM> LoadFriend(string id)
        {
            var model =  await _context.Hotels.Where(h =>h.Id == id).Select(h => new FriendProfileVM { 
                Id = h.Id , 
                Location = h.Location! ,    
                Stars= h.Stars ,    
                Description = h.Description! ,  
                Rate = h.Rate , 
                City = h.User.City , 
                Email = h.User.Email ,
                EnterpriseName = h.User.EnterpriseName ,    
                PhoneNumber = h.User.PhoneNumber ,  
                ProfileImage = h.User.ProfileImage!, 
            }).FirstAsync();

            model.Rooms =await _context.Rooms.Where(r => r.HotelId == id).Select(r => new OverViewRoomViewModel
            {
                Id = r.Id ,
                Type = r.Type , 
                MainImage = r.ImageUrl! , 
                Price = r.PricePerNight , 
                OfferPrice = r.RoomOffer!.OfferPrice
            }).Take(9).ToListAsync(); 
            return model;   
        }
        public async Task<ProfileViewModel> AdminOverView(string Id)
        {
            var hotel = await _context.Hotels.Where(h => h.Id == Id)
                .Select(h => new ProfileViewModel
                {
                    Id = h.Id,
                    Location = h.Location!,
                    EnterpriseName = h.User.EnterpriseName,
                    Email = h.User.Email,
                    Description = h.Description!,
                    City = h.User.City,
                    ProfileImage = h.User.ProfileImage,
                    PhoneNumber = h.User.PhoneNumber,
                }).FirstAsync();

            hotel.Reviews = await _context.Set<Rate>().Where(r => r.ToId == Id).CountAsync();

            var sum = await _context.Set<Rate>().Where(r => r.ToId == Id).SumAsync(r => r.Value);

            if (hotel.Reviews == 0)
                hotel.Rate = 0;
            else
                hotel.Rate = (double)sum / hotel.Reviews;



            hotel.ViewComments = await GetEnterpriseComments(0, 0, Id);

            hotel.NumServies = await _context.Rooms.Where(r => r.HotelId == Id).CountAsync();

            hotel.NumBooking = await _context.Set<RoomTransaction>().Where(t => t.Room.HotelId == Id)
                .CountAsync();
            var TotalIn = await _context.Set<RoomTransaction>().Where(t => t.Room.HotelId == Id)
                   .SumAsync(t => t.Amount);

            hotel.TotalIn = TotalIn.ToString("c");

            return hotel;
        }
    }

}
