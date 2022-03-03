using Reservation.Models.Dto;
using System.Collections.Generic;

namespace Reservation.Models.ViewModel
{
    /// <summary>
    /// sefer sorgulama işlemi için kullanılan modeldir.
    /// </summary>
    public class JourneySearchModel
    {
        // sefer tarihi bu parametrede saklanır.
        public string DateInput { get; set; }

        // kalkış lokasyonuna ait Id burada saklanır.
        public int SourceId { get; set; }

        // varış lokasyonuna ait Id burada saklanır.
        public int DestinationId { get; set; }

        // daha sonra kullanılmak üzere saklanacak olan kalkış lokasyon bilgisi burada saklanır.
        public int SetSourceId { get; set; }

        // daha sonra kullanılmak üzere saklanacak olan varış lokasyon bilgisi burada saklanır.
        public int SetDestinationId { get; set; }

        // yapılan sorguya ait sefer listesi burada saklanır.
        public List<BusJourney> Journeys { get; set; }

        public JourneySearchModel()
        {
            // null reference exception hatasını almamak için her bir model oluşturulduğunda listeden yeni bir instance alınır.
            Journeys = new List<BusJourney>();
        }
    }
}
