using Microsoft.AspNetCore.Http;
using Reservation.Helpers;
using Reservation.Models.Dto;
using Reservation.Models.RequestModel;
using Reservation.Models.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Services.Interfaces
{
    public interface ISessionService
    {
        Task<DeviceSession> GetDeviceSession();
    }
}
