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
using GoByPDX.ViewModels;
using System.Reflection;

namespace GoByPDX
{
    public class routeListViewModel : INotifyPropertyChanged
    {

        #region  Class Variables

        public int routeComboIndex { get; set; }
        public int dirComboIndex { get; set; }
        public int stopComboIndex { get; set; }

        public List<dynamic> routes;
        public List<DirectionInfo> directions;
        public List<dynamic> stops;
        public List<dynamic> arrivals;

       
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

      

        #endregion 

        public routeListViewModel()
        {
            string urlString_routes = "https://developer.trimet.org/ws/V1/routeConfig/dir/true/appID/7BCBE4BB29666DDCBB7D73113";
            Uri uri_routes = new Uri(urlString_routes);

            RouteInfo classForXML = new RouteInfo();
            Dictionary<string, string> topProps = new Dictionary<string, string>();
            topProps["route"] = "route";
            topProps["desc"] = "desc";

            // Remember Order matters, xmlreader will read to the first element of this dict
            Dictionary<string, string> lowerProps = new Dictionary<string, string>();
            lowerProps["dir"] = "dir";
            lowerProps["dirDesc"] = "desc";

            List<string> xmlNode_l = new List<string>() { "route", "dir", "dir" };

            Task<List<dynamic>> returnRouteInfoListTask = Task.Run(() => loadXML(uri_routes, classForXML.GetType(), topProps, lowerProps, xmlNode_l));
            string test = Task.CompletedTask.ToString();
            routes = returnRouteInfoListTask.Result;

            foreach (RouteInfo routeInfo in routes)
            {
                if (!bindingRouteListItems.Contains(routeInfo.desc))
                {
                    bindingRouteListItems.Add(routeInfo.desc);
                }
            }
        }

        public async Task<bool> loadDirInfo(string description)
        {
            bindingDirListItems.Clear();
            var routeClass = routes.Where(e => String.Equals(e.desc, description)).Select(ret2 => ret2);

            foreach (var ret in routeClass)
            {
                bindingDirListItems.Add(ret.dirDesc);
            }
            return true;
        }

        public async Task<bool> loadStopInfo(string routeDir, string routeID)
        {
            stopList.Clear();

            if (routeDir != "" && routeID != "")
            {
                //Debug.WriteLine("NSP Dir Selected: " + routeDir);
                string urlString_stopInfo = "https://developer.trimet.org/ws/V1/routeConfig/route/" + routeID + "/dir/" + routeDir + "/stops/test/appID=7BCBE4BB29666DDCBB7D73113";
                Uri uri_stops = new Uri(urlString_stopInfo);
                if (stops != null)
                {
                    stops.Clear();
                }

                Dictionary<string, string> topProps = new Dictionary<string, string>();
                topProps["dir"] = "null";

                Dictionary<string, string> lowerProps = new Dictionary<string, string>();

                lowerProps["desc"] = "desc";
                lowerProps["Lat"] = "lat";
                lowerProps["Lng"] = "lng";
                lowerProps["LocID"] = "locid";
                List<string> xmlNode_l = new List<string>() { "route", "stop", "stop" };

                StopInfo classForXML = new StopInfo();

                Task<List<dynamic>> returnStopInfoListTask = Task.Run(() => loadXML(uri_stops, classForXML.GetType(), topProps, lowerProps, xmlNode_l));
                string test = Task.CompletedTask.ToString();

                stops = returnStopInfoListTask.Result;
                stopList.Clear();
                foreach (StopInfo stopInfo in stops)
                {
                    stopList.Add(stopInfo.desc);
                }
                App.lastState.routeID = routeID;
                App.lastState.routeDir = routeDir;
            }
            return true;
        }

        public async Task<bool> showNextArrivals(ComboBox routeComboBox, ComboBox dirComboBox, ComboBox stopsComboBox)
        {
            string routeID = "";
            var routeClass = App.routeListViewModel.routes.Where(e => String.Equals(e.desc, routeComboBox.SelectedValue.ToString())).Select(ret2 => ret2);

            foreach (var ret in routeClass) { routeID = ret.route; }

            if (stopsComboBox.SelectedValue.ToString() != "" && stopsComboBox.SelectedValue.ToString() != null)
            {
                string locIDs = "";
                var stopClass = App.routeListViewModel.stops.Where(e => String.Equals(e.desc, stopsComboBox.SelectedValue)).Select(ret2 => ret2);

                foreach (var ret in stopClass) { locIDs = ret.LocID; }
                loadNextArrivals(locIDs, routeID);
            }
            
            //TODO CHeck the favorite star and see if route is favorite
            bool toggleState = false;
            string Route = routeComboBox.SelectedValue.ToString();
            string Dir = dirComboBox.SelectedValue.ToString();
            string Stop = stopsComboBox.SelectedValue.ToString();
            favoritesListViewModel updateSQL = new favoritesListViewModel();
            int contactid = updateSQL.UpdateDetails(Route, Dir, Stop);
            if (contactid != -1)
            {
                toggleState = true;
            }
            return toggleState;
        }

        // Load the Next Arrivals
        private void loadNextArrivals(string locIDs, string routeID)
        {
            string urlString_arrivalInfo = "http://developer.trimet.org/ws/v2/arrivals/locIDs/" + locIDs + "/json/false/arrivals/4/appID/7BCBE4BB29666DDCBB7D73113";
            Uri uri_arrivals = new Uri(urlString_arrivalInfo);
            //Make this a simple list of times return, no need for class info

            Dictionary<string, string> topProps = new Dictionary<string, string>();
            topProps["lat"] = "lat";
            topProps["lng"] = "lng";
            topProps["desc"] = "desc";

            Dictionary<string, string> lowerProps = new Dictionary<string, string>();
            
            lowerProps["fullSign"] = "fullSign";
            lowerProps["scheduled"] = "scheduled";
            lowerProps["vehicleID"] = "vehicleID";
            lowerProps["estimated"] = "estimated";
            lowerProps["locid"] = "locid";
            lowerProps["route"] = "route";

            List<string> xmlNode_l = new List<string>() { "location", "arrival", "arrival" };

            ArrivalInfo classForXML = new ArrivalInfo();
            Task<List<dynamic>> returnArrivalInfoListTask = Task.Run(() => loadXML(uri_arrivals, classForXML.GetType(), topProps, lowerProps, xmlNode_l));

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



        async private Task<List<dynamic>> loadXML(Uri uri, Type classType, Dictionary<string, string> topProps, Dictionary<string, string> lowerProps, List<string> xmlNode_l)
        {
            List<dynamic> routeInfoList = new List<dynamic>();
            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(uri);
                XmlReader reader = XmlReader.Create(stream);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.HasAttributes && reader.Name == xmlNode_l[0])
                        {
                            //Debug.WriteLine("Outer Reader.LocalName: " + reader.LocalName);
                            //Debug.WriteLine("Outer Reader.Name: " + reader.Name);

                            Dictionary<string, string> topPropVals = new Dictionary<string, string>();
                            foreach (string classPropName in topProps.Keys)
                            {
                                topPropVals.Add(classPropName, reader.GetAttribute(topProps[classPropName]));
                                //Debug.WriteLine(classPropName + ": " + reader.GetAttribute(topProps[classPropName]));
                            }

                            if (reader.ReadToFollowing(xmlNode_l[1]))
                            {
                                do
                                {
                                    //Debug.WriteLine("Outer Reader.LocalName: " + reader.LocalName);

                                    object routeInfo = Activator.CreateInstance(classType);

                                    foreach (string classPropName in topPropVals.Keys)
                                    {
                                        PropertyInfo prop = classType.GetProperty(classPropName);
                                        prop.SetValue(routeInfo, topPropVals[classPropName]);
                                        //Debug.WriteLine(classPropName + ": " + topPropVals[classPropName]);
                                    }

                                    foreach (string classPropName in lowerProps.Keys)
                                    {
                                        PropertyInfo prop = classType.GetProperty(classPropName);
                                        prop.SetValue(routeInfo, reader.GetAttribute(lowerProps[classPropName]));
                                        //Debug.WriteLine(classPropName + ": " + reader.GetAttribute(lowerProps[classPropName]));
                                    }
                                    
                                    routeInfoList.Add(routeInfo);
                                } while (reader.ReadToNextSibling(xmlNode_l[2]));
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
