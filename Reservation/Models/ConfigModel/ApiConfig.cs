using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models.ConfigModel
{
    /// <summary>
    /// App settin içerisinde api ile ilgili konfigürasyonların bulunduğu kısmın cast edileceği model
    /// </summary>
    public class ApiConfig
    {
        // İstekleri için kullanılacak endpointlerin domain kısımlarının saklanacağı parametre
        public string BaseUrl { get; set; }

        // Herhangi bir yetkilendirme işleminin kullanılacağını belirten parametre
        public bool UseAuthorization { get; set; }

       // Bir authentication yapısı olacak ise bu yapının türünü belirten parametre
        public string AuthType { get; set; }

        // Kullanılacak token değerinin saklandığı parametre
        public string Token { get; set; }
    }
}
