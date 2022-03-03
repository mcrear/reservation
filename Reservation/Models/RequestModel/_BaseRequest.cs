using Newtonsoft.Json;
using Reservation.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models.RequestModel
{
    /// <summary>
    /// Tüm requestlerde ortak kullanılan istek bilgileri
    /// </summary>
    /// <typeparam name="T">İstek nesnelerinde değişken olarak bulunan alandır. Her endpoint için yeni bir model oluşturulmalıdır.</typeparam>
    public class _BaseRequest<T>
    {
        [JsonProperty("device-session")]
        public DeviceSession DeviceSession { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
