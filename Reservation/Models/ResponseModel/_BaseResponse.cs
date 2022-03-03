using Newtonsoft.Json;
using Reservation.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models.ResponseModel
{
    /// <summary>
    /// Tekil dönüşler için hazırlanmış olan base response modeldir. Burada kullanılacak olan nesneler _dto ile imzalanmış olmalıdır.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class _BaseResponse<T> where T : _Dto
    {

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("message")]
        public object Message { get; set; }

        [JsonProperty("user-message")]
        public object UserMessage { get; set; }

        [JsonProperty("api-request-id")]
        public object ApiRequestId { get; set; }

        [JsonProperty("controller")]
        public object Controller { get; set; }

        [JsonProperty("client-request-id")]
        public object ClientRequestId { get; set; }
    }
}
