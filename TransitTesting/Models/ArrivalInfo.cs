using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoByPDX
{
    public class ArrivalInfo
    {
        public string route { get; set; }
        public string vehicleID { get; set; }
        public string fullSign { get; set; }

        private string _scheduled = "";
        public string scheduled
        {
            get { return this._scheduled; }
            set
            {
                //this._scheduled = value; 
                if (value != null)
                {
                    double doubSch = Convert.ToDouble(value.Remove(value.Length - 3));
                    DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);

                    dt = dt.AddSeconds(doubSch).ToLocalTime();
                    this._scheduled = dt.ToString();
                }
            }
        }
        public string lng { get; set; }
        public string lat { get; set; }
        public string locid { get; set; }
        public string desc { get; set; }


        //public string estimated { get; set; }

        private string _estimated = "";
        public string estimated
        {
            get { return this._estimated; }
            set
            {
                //this._scheduled = value; 
                if (value != null)
                {
                    double doubSch = Convert.ToDouble(value.Remove(value.Length - 3));
                    DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);

                    dt = dt.AddSeconds(doubSch).ToLocalTime();
                    this._estimated = dt.ToString();
                }
            }
        }

        public string tripID { get; set; }
    }
}
