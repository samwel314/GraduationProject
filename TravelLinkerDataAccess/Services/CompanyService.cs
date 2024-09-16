using Microsoft.EntityFrameworkCore;
using TravelLinkerDataAccess.Data;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;
 
        public CompanyService(ApplicationDbContext context )
        {
            _context = context;

        }

        public async Task<ProfileViewModel?> Profile(string Id, string imagePath)
        {
            //
            var Data =  await _context.Companies.Select(C => new ProfileViewModel
            {
                Id = C.Id,
                EnterpriseName = C.User.EnterpriseName,
                Email = C.User.Email,
                PhoneNumber = C.User.PhoneNumber,
                City = C.User.City,
                ProfileImage = C.User.ProfileImage == null ? null : imagePath + $"/TripApi/GetImage/{C.Id}/{C.User.ProfileImage}",
                Location = C.Location!,
                Description = C.Description!,
                Rate = C.Rate,
      
            }).FirstOrDefaultAsync(C => C.Id == Id);

            if (Data != null)
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
        public Company Create(string id)
        {
            var Company = new Company()
            {
                Id = id
            };
            _context.Companies.Add(Company);
            _context.SaveChanges();
            return Company;
        }

        public IEnumerable<Trip> GetAllTrips(string id, bool include = false)
        {
            if (!include)
                return _context.Trips.AsNoTracking().Where(t => t.CompanyId == id).ToList();

            // my delete it 
            return _context.Trips.AsNoTracking().Where(t => t.CompanyId == id).ToList(); ;
        }

        public async Task<IEnumerable<ViewCommentDTO>> GetEnterpriseComments(int Active, int pageSize, string Id)
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

        public async Task<int> GetCountComment(string Id)
        {
            return await _context.Comments.Where(c => c.ToId == Id).CountAsync();
        }
        public Company GetCompany(string id)
        {
            return _context.Companies.
               AsNoTracking().Select(c => new Company
               {
                   Id = c.Id,   
                   City = c.User.City,
                   Description = c.Description , 
                   Rate = c.Rate , 
                   Location = c.Location ,  
                   
               }).First(c=> c.Id == id); 
        }


        public async Task ChanegDescription(string Id, string Description)
        {
            var Company = _context.Companies.First(Company => Company.Id == Id);

            Company.Description = Description;

            await _context.SaveChangesAsync();
        }


        public async Task ChanegLocation(string Id, string Location)
        {
            var Company = _context.Companies.First(Company => Company.Id == Id);

            Company.Location = Location;

            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ShowFrindes>> ShowFrindes(string City, string ID)
        {
            return await _context.Companies.Where(c =>c.User.City == City && c.Id != ID).Select(c => new ShowFrindes
            {
                Id = c.Id,
                Name = c.User.EnterpriseName,
                Image = c.User.ProfileImage!,
                Email = c.User.Email!,
                Location = c.Location!
            }).ToListAsync();
        }

        public async Task<FriendProfileVM> LoadFriend(string id)
        {
            var model = await _context.Companies.Where(h => h.Id == id).Select(h => new FriendProfileVM
            {
                Id = h.Id,
                Location = h.Location!,

                Description = h.Description!,
                Rate = h.Rate,
                City = h.User.City,
                Email = h.User.Email,
                EnterpriseName = h.User.EnterpriseName,
                PhoneNumber = h.User.PhoneNumber,
                ProfileImage = h.User.ProfileImage!,
            }).FirstAsync();

            model.Rooms = await _context.Trips.Where(r => r.CompanyId == id).Select(r => new OverViewRoomViewModel
            {
                Id = r.Id,
                City = r.From,
                Type = r.To!,
                Price = r.Price,
            }).Take(9).ToListAsync();
            return model;
        }
        // -*-*- 


        public async Task<ProfileViewModel> AdminOverView(string Id)
        {
            var hotel = await _context.Companies.Where(h => h.Id == Id)
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

            hotel.NumServies = await _context.Trips.Where(r => r.CompanyId == Id).CountAsync();

            hotel.NumBooking = await _context.Set<TripTransaction>().Where(t => t.Trip.CompanyId == Id)
                .CountAsync();
            var TotalIn = await _context.Set<TripTransaction>().Where(t => t.Trip.CompanyId == Id)
                   .SumAsync(t => t.Amount);

            hotel.TotalIn = TotalIn.ToString("c");

            return hotel;
        }
    }
  

}
