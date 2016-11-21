using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoByPDX
{
    public class StopInfo
    {
        private string lng = "";
        private string lat = "";
        private string locid = "";

        public string dir { get; set; }
        public string dirDesc { get; set; }
        public string desc { get; set; }

        public string Lng
        {
            get { return this.lng; }
            set { this.lng = value; }
        }
        public string Lat
        {
            get { return this.lat; }
            set { this.lat = value; }
        }
        public string LocID
        {
            get { return this.locid; }
            set { this.locid = value; }
        }
    }
}
