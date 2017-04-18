using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoByPDX.Models.TripPlannerModels
{
    public class tripLeg
    {
        public string mode { get; set; }

        public string from { get; set; }
        public string to { get; set; }


        public string viaRoute { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string duration { get; set; }
        public string distance { get; set; }

        public string number { get; set; }
        public string name { get; set; }
    }
}
