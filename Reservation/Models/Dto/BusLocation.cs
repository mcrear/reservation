using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models.Dto
{
    // konum listesi dto nesnesi
    public class BusLocation : _Dto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("parent-id")]
        public int ParentId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("geo-location")]
        public GeoLocation GeoLocation { get; set; }

        [JsonProperty("zoom")]
        public int Zoom { get; set; }

        [JsonProperty("tz-code")]
        public string TzCode { get; set; }

        [JsonProperty("weather-code")]
        public object WeatherCode { get; set; }

        [JsonProperty("rank")]
        public int? Rank { get; set; }

        [JsonProperty("reference-code")]
        public object ReferenceCode { get; set; }

        [JsonProperty("keywords")]
        public string Keywords { get; set; }
    }
}
