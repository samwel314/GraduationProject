using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TravelLinkerModels.Models;
using TravelLinkerModels.Models.ViewModels;

namespace TravelLinkerDataAccess.Services
{
    public interface ITripService
    {
        Trip Create(Trip trip);
        Task<IEnumerable<ShowDetailsTicket>> showAllTicket(string Id); 
        Trip ?  Update(Trip trip);
        Task HideNotHide(string id);

        Trip? GetTrip(string id , bool include = false );

        Task<bool> CanUpdate(string Id ); 
        Task<TripTransaction> CreateTransaction(TripTransaction transaction);

        Task<TripTransaction?> GetTransaction(int id);

        Task DeleteTransaction(int Id); 
        Task UpdateTransaction (TripTransaction transaction);   
        int GetCountForCity(string City ); 
          // id for Company 
        bool AnyCode(string Code , string CompanyId, string tripId);
        Task<TripDetailsVM?> Details(string Id, string domain);
        int GetCount ();    
        Task<IEnumerable<OverViewTripViewModel>> GetAll(int pageSize, int Active, string imagePath, string City = null!);

    }
}
