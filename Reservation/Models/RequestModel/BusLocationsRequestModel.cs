using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models.RequestModel
{
    /// <summary>
    /// lokasyon listesi için oluşturulan model. Base request içerisine boş değer alabilen bir string bekliyor. Bunun sebebi tüm lokasyonlar için gönderilecek request içerisinde data değerinin null olarak beklenmesidir.
    /// eğer içerisinde bir data varsa bu data isteğe eklenecektir.
    /// </summary>
    public class BusLocationsRequestModel : _BaseRequest<string?>
    {
    }
}
