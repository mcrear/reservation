using Reservation.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models.ResponseModel
{
    /// <summary>
    /// Lokasyon listelerinin dönüşünü cast etmek için kullanılan modeldir.
    /// </summary>
    public class BusLocationsResponseModel : _BaseListResponse<IEnumerable<BusLocation>>
    {
    }
}
