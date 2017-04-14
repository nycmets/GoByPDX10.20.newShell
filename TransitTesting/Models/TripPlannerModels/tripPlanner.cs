using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoByPDX.Models.TripPlannerModels
{
    class tripPlanner
    {
        public List<string> possibleFrom { get; set; }
        public List<string> possibleTo { get; set; }

        public List<tripItinerary> tripItinerary_l { get; set; }

    }
}
