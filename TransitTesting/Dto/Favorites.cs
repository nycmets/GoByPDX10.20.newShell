using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Attributes;

namespace GoByPDX.Dto
{

    public class Favorites : BaseDto
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Route { get; set; }
        public string Dir { get; set; }
        public string Stop { get; set; }
        public int routeComboIndex { get; set; }
        public int dirComboIndex { get; set; }
        public int stopComboIndex { get; set; }


        public Favorites()
        { }
        public Favorites(string route, string dir, string stop)
        {
            Route = route;
            Dir = dir;
            Stop = stop;
        }
    }
}
