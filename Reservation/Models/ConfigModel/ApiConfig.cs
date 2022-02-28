using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models.ConfigModel
{
    public class ApiConfig
    {
        public string BaseUrl { get; set; }
        public bool UseAuthorization { get; set; }
        public string AuthType { get; set; }
        public string Token { get; set; }
    }
}
