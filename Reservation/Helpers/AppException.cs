using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Helpers
{
    public class AppException : Exception
    {
        // Proje içerisinde kullanılacak custom exception sınıfı için boş constracture metodunun tanımlanması
        public AppException() : base() { }

        // Proje içerisinde kullanılacak custom exception sınıfı için sadece mesaj değeri alan constracture metodunun tanımlanması
        public AppException(string message) : base(message) { }

        // Proje içerisinde kullanılacak custom exception sınıfı için mesaj değeri ve paylaşılmak istenilen diğer objeleri alan constracture metodunun tanımlanması
        public AppException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
