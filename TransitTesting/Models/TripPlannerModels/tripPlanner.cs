using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoByPDX.Models.TripPlannerModels
{
    public class tripPlanner
    {
        public string fromString { get; set; }
        public string toString { get; set; }
        public DateTime leaveTime { get; set; }

        public bool busToggle { get; set; }
        public bool trainToggle { get; set; }

        public List<string> possibleFrom { get; set; }
        public List<string> possibleTo { get; set; }

        public List<tripItinerary> tripItinerary_l { get; set; }

    }
}
