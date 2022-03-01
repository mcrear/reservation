using Reservation.Models.RequestModel;
using Reservation.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Services.Interfaces
{
    public interface ILocationService
    {
        Task<BusLocationsResponseModel> GetLocations(BusLocationsRequestModel busLocationsRequestModel);
    }
}
