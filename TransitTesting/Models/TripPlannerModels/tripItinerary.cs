using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoByPDX.Models.TripPlannerModels
{
    public class tripItinerary
    {
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string duration { get; set; }

        public string numberOfTransfers { get; set; }
        public string numberOfTripLegs { get; set; }

        public List<tripLeg> tripLeg_l { get; set; }
        
    }
}
