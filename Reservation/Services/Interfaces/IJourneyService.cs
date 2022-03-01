using Reservation.Models.RequestModel;
using Reservation.Models.ResponseModel;
using System.Threading.Tasks;

namespace Reservation.Services.Interfaces
{
    public interface IJourneyService
    {
        Task<BusJourneysResponseModel> GetBusJourneys(BusJourneysRequestModel busJourneysRequestModel);
    }
}
