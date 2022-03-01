using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models.Dto
{
    public class GeoLocation
    {

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longtitude")]
        public double Longitude { get; set; }

        [JsonProperty("zoom")]
        public int Zoom { get; set; }
    }
}
