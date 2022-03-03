using Reservation.Models.Dto;
using System.Collections.Generic;

namespace Reservation.Models.ViewModel
{
    public class JourneySearchModel
    {
        public string DateInput { get; set; }
        public int SourceId { get; set; }
        public int DestinationId { get; set; }
        public int SetSourceId { get; set; }
        public int SetDestinationId { get; set; }
        public List<BusJourney> Journeys { get; set; }

        public JourneySearchModel()
        {
            Journeys = new List<BusJourney>();
        }
    }
}
