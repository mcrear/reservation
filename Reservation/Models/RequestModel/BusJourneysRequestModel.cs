using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models.RequestModel
{
    /// <summary>
    /// Sefer listesi için oluşturulan istek modelinin base request model içine eklenmesi
    /// </summary>
    public class BusJourneysRequestModel : _BaseRequest<BusJourneyRequest>
    {
    }

    // sefer listesi için oluşturulan istek model
    public class BusJourneyRequest
    {
        [JsonProperty("origin-id")]
        public int OriginId { get; set; }

        [JsonProperty("destination-id")]
        public int DestinationId { get; set; }

        [JsonProperty("departure-date")]
        public string DepartureDate { get; set; }
    }
}
