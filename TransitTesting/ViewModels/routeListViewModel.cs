using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Xml;
using System.Net.Http;
using System.ComponentModel;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Diagnostics;

namespace GoByPDX
{
    public class routeListViewModel : INotifyPropertyChanged
    {

        #region  Class Variables

        public int routeComboIndex { get; set; }
        public int dirComboIndex { get; set; }
        public int stopComboIndex { get; set; }

        public List<RouteInfo> routes;
        public List<DirectionInfo> directions;
        public List<StopInfo> stops;
        public List<ArrivalInfo> arrivals;

        public ObservableCollection<string> bindingRouteListItems = new ObservableCollection<string>();
        public ObservableCollection<string> bindingStopListItems = new ObservableCollection<string>();
        public ObservableCollection<string> bindingDirListItems = new ObservableCollection<string>();
        public ObservableCollection<string> bindingArrivalListItems = new ObservableCollection<string>();

        List<RouteInfo> returnRouteInfoList = new List<RouteInfo>();
        List<DirectionInfo> returnDirInfoList = new List<DirectionInfo>();
        List<StopInfo> returnStopInfoList = new List<StopInfo>();
        List<ArrivalInfo> returnArrivalInfoList = new List<ArrivalInfo>();

        public ObservableCollection<string> routeList { get { return bindingRouteListItems; } set { bindingRouteListItems = value; OnPropertyChanged("bindingRouteListItems"); } }
        public ObservableCollection<string> dirList { get { return bindingDirListItems; } set { bindingDirListItems = value; OnPropertyChanged("bindingDirListItems"); } }
        public ObservableCollection<string> stopList { get { return bindingStopListItems; } set { bindingStopListItems = value; this.OnPropertyChanged("stopList"); } }
        public ObservableCollection<string> arrivalList { get { return bindingArrivalListItems; } set { this.OnPropertyChanged("arrivalList"); } }

        //public List<RouteInfo> routesClass { get { return routes; } set { return; } }
        //public List<DirectionInfo> directionClass { get { return directions; } set { return; } }
        //public List<StopInfo>  stopsClass { get { return stops; } set { return; } }
        //public List<ArrivalInfo> arrivalsClass { get { return arrivals; } set { return; } }

        #endregion 

        public routeListViewModel()
        {
            //if (App.routeListViewModel.routes == null)
            //{
                string urlString_routes = "https://developer.trimet.org/ws/V1/routeConfig/dir/true/appID/7BCBE4BB29666DDCBB7D73113";
                Uri uri_routes = new Uri(urlString_routes);

                Task<List<RouteInfo>> returnRouteInfoListTask = Task.Run(() => loadXML_routes(uri_routes));
                routes = returnRouteInfoListTask.Result;

                foreach (RouteInfo routeInfo in routes)
                {
                    if (!bindingRouteListItems.Contains(routeInfo.desc))
                    {
                        bindingRouteListItems.Add(routeInfo.desc);
                    }
                }
            //}
        }

        public void loadDirInfo(string description)
        {
            bindingDirListItems.Clear();
            var routeClass = routes.Where(e => String.Equals(e.desc, description)).Select(ret2 => ret2);

            foreach (var ret in routeClass)
            {
                bindingDirListItems.Add(ret.dirDesc);
            }
        }

        public void loadStopInfo(string routeDir, string routeID)
        {
            stopList.Clear();

            if (routeDir != "" && routeID != "")
            {
                Debug.WriteLine("NSP Dir Selected: " + routeDir);
                string urlString_stopInfo = "https://developer.trimet.org/ws/V1/routeConfig/route/" + routeID + "/dir/" + routeDir + "/stops/test/appID=7BCBE4BB29666DDCBB7D73113";
                Uri uri_stops = new Uri(urlString_stopInfo);
                if (stops != null)
                {
                    stops.Clear();
                }
                Task<List<StopInfo>> returnStopInfoListTask = Task.Run(() => loadXML_stops(uri_stops));
                stops = returnStopInfoListTask.Result;
                stopList.Clear();
                foreach (StopInfo stopInfo in stops)
                {
                    stopList.Add(stopInfo.desc);
                }
                App.lastState.routeID = routeID;
                App.lastState.routeDir = routeDir;
            }
        }
        

        // Load the Next Arrivals
        public void loadNextArrivals(string locIDs, string routeID)
        {
            string urlString_arrivalInfo = "http://developer.trimet.org/ws/v2/arrivals/locIDs/" + locIDs + "/json/false/arrivals/4/appID/7BCBE4BB29666DDCBB7D73113";
            Uri uri_arrivals = new Uri(urlString_arrivalInfo);
            //Make this a simple list of times return, no need for class info
            Task<List<ArrivalInfo>> returnArrivalInfoListTask = Task.Run(() => loadXML_arrivals(uri_arrivals));
            arrivals = returnArrivalInfoListTask.Result;
            arrivalList.Clear();
            foreach (ArrivalInfo arrivalInfo in arrivals)
            {
                if (arrivalInfo.fullSign != null && arrivalInfo.route == routeID)
                {
                    //Debug.WriteLine("NSP descSIGN: " + arrivalInfo.fullSign);
                    //Debug.WriteLine("NSP TIME: " + arrivalInfo.scheduled);
                                         
                    arrivalList.Add(arrivalInfo.scheduled.ToString());
                }
            }
        }

        // Load up the Routes Class
        async private Task<List<RouteInfo>> loadXML_routes(Uri uri)
        {
            List<RouteInfo> routeInfoList = new List<RouteInfo>();

            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(uri);
                XmlReader reader = XmlReader.Create(stream);
                //reader.ReadToFollowing("route");

                while (reader.Read())
                {
                    string routeNum = "";
                    string routeDesc = "";

                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.HasAttributes && reader.Name == "route")
                        {
                            //Debug.WriteLine("Outer Reader.LocalName: " + reader.LocalName);

                            //Debug.WriteLine("Outer Reader.Name: " + reader.Name);
                            //TODO Update these for my dir
                            //reader.ReadToFollowing("route");
                            routeNum = reader.GetAttribute("route");
                            routeDesc = reader.GetAttribute("desc");
                            if (reader.ReadToDescendant("dir"))
                            {
                                do
                                {
                                    RouteInfo routeInfo = new RouteInfo();
  
                                    routeInfo.desc = routeDesc;
                                    routeInfo.route = routeNum;
                                    routeInfo.dir = reader.GetAttribute("dir");
                                    routeInfo.dirDesc = reader.GetAttribute("desc");
                                    routeInfoList.Add(routeInfo);
                                } while (reader.ReadToNextSibling("dir"));

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //routeList. = "EXCEPTION";
            }
            return routeInfoList;
        }

        //Load up the Stops For the selected Route Class
        async private Task<List<StopInfo>> loadXML_stops(Uri uri)
        {
            List<StopInfo> stopInfoList = new List<StopInfo>();

            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(uri);
                XmlReader reader = XmlReader.Create(stream);

                string dir = "";
                string dirDesc = "";

                //reader.ReadToFollowing("stop");

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.HasAttributes && reader.Name == "stop")
                        {
                            StopInfo stopInfo = new StopInfo();
                            stopInfo.desc = reader.GetAttribute("desc");
                            stopInfo.Lng = reader.GetAttribute("lng");
                            stopInfo.Lat = reader.GetAttribute("lat");
                            stopInfo.LocID = reader.GetAttribute("locid");
                            stopInfoList.Add(stopInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //routeList = "EXCEPTION";
            }
            return stopInfoList;
        }

        //Load up the Arrivals For the selected Route Class
        async private Task<List<ArrivalInfo>> loadXML_arrivals(Uri uri)
        {
            List<ArrivalInfo> arrivalInfoList = new List<ArrivalInfo>();

            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(uri);
                XmlReader reader = XmlReader.Create(stream);

                string lat = "";
                string lng = "";
                string desc = "";


                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.HasAttributes && reader.GetAttribute("lng") != null)
                        {
                            lat = reader.GetAttribute("lat");
                            lng = reader.GetAttribute("lng");
                            desc = reader.GetAttribute("desc");
                        }

                        if (reader.HasAttributes && reader.GetAttribute("fullSign") != null)
                        {
                            ArrivalInfo arrivalInfo = new ArrivalInfo();

                            arrivalInfo.lng = lng;
                            arrivalInfo.lat = lat;
                            arrivalInfo.desc = desc;

                            arrivalInfo.fullSign = reader.GetAttribute("fullSign");
                            arrivalInfo.route = reader.GetAttribute("route");
                            arrivalInfo.scheduled = reader.GetAttribute("scheduled");
                            arrivalInfo.vehicleID = reader.GetAttribute("vehicleID");
                            arrivalInfo.estimated = reader.GetAttribute("estimated");
                            arrivalInfo.estimated = reader.GetAttribute("locid");

                            arrivalInfoList.Add(arrivalInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //routeList = "EXCEPTION";
            }
            return arrivalInfoList;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
