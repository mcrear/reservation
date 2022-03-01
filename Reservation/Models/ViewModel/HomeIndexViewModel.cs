using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models.ViewModel
{
    public class HomeIndexViewModel
    {
        public string  DateInput { get; set; }
        public int SourceId { get; set; }
        public int DestinationId { get; set; }
    }
}
