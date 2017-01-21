using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoByPDX;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Xml;
using System.Windows;
using System.Net.Http;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using System.ComponentModel;

namespace GoByPDX
{
    public class transitLocationViewModel
    {
  
        public vehicleLocation vehicleLocation = new vehicleLocation();
        public BasicGeoposition vehiclePosition = new BasicGeoposition();

        public Geopoint vehiclePoint { get; set; }
        public Geopoint stopPoint { get; set; }
        public string vehicleIDprev { get; set; }
        public Geopoint vehiclePointGP { get; set; }
        public string scheduled { get; set; }
        public string vehicleID { get; set; }

        public string stopLng { get; set; }
        public string stopLat { get; set; }
        public string stopName { get; set; }

        public transitLocationViewModel()
        {
            // Put all the crap from the showTransitLocation here

            //string vehicleTime = e.AddedItems[0].ToString();
            Debug.WriteLine("NSP transitSel: " + App.lastState.vehicleScheduledTime);

            //string vehicleID = "";
            vehicleID = "";
            stopLng = "";
            stopLat = "";
            stopName = "";
            //string scheduled = "";
            scheduled = "";
            //var vehicleClass = App.routeListViewModel.arrivals.Where(e => String.Equals(e.scheduled, source.Parameter)).Select(ret => ret);
            var vehicleClass = App.routeListViewModel.arrivals.Where(e => String.Equals(e.scheduled, App.lastState.vehicleScheduledTime)).Select(ret => ret);


            foreach (var ret in vehicleClass)
            {
                string[] schList = ret.scheduled.Split();
                scheduled = schList[1] + " " + schList[2];
                vehicleID = ret.vehicleID;
                stopLng = ret.lng;
                stopLat = ret.lat;
                stopName = ret.desc;
            }
            loadLocationInfo(vehicleID);
            BasicGeoposition snPosition = new BasicGeoposition() { Latitude = Convert.ToDouble(stopLat), Longitude = Convert.ToDouble(stopLng) };
            stopPoint = new Geopoint(snPosition);

            BasicGeoposition vehiclePosition = new BasicGeoposition() { Latitude = Convert.ToDouble(vehicleLocation.latitude), Longitude = Convert.ToDouble(vehicleLocation.longitude) };

            vehiclePoint = new Geopoint(vehiclePosition);

        }


        public void loadLocationInfo(string vehicleID)
        {
            vehicleIDprev = vehicleID;
            Debug.WriteLine("NSP Dir Selected: " + vehicleID);
            string urlString_stopInfo = "http://developer.trimet.org/ws/v2/vehicles/json/false/ids/" + vehicleID + "/appID/7BCBE4BB29666DDCBB7D73113";
            Uri uri_stops = new Uri(urlString_stopInfo);
            Task<vehicleLocation> returnLocationInfoTask = Task.Run(() => loadXML_vehicleLocations(uri_stops));
            vehicleLocation = returnLocationInfoTask.Result;

            // Specify lat, lng for the vehicle
            vehiclePosition = new BasicGeoposition() { Latitude = Convert.ToDouble(vehicleLocation.latitude), Longitude = Convert.ToDouble(vehicleLocation.longitude) };
            //vehiclePoint = new Geopoint(vehiclePosition);
            vehiclePointGP = new Geopoint(vehiclePosition);
            //bindingVehiclePoint.Add(vehiclePointGP);

        }


        //Load up the Stops For the selected Route Class
        async private Task<vehicleLocation> loadXML_vehicleLocations(Uri uri)
        {
            vehicleLocation vehicleLocation = new vehicleLocation();
            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(uri);
                XmlReader reader = XmlReader.Create(stream);

                //reader.ReadToFollowing("vehicle");

                while (reader.Read())
                {
                    Debug.WriteLine("Name: " + reader.Name);
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.HasAttributes && reader.Name == "vehicle")
                        {
                            vehicleLocation.latitude = reader.GetAttribute("latitude");
                            vehicleLocation.longitude = reader.GetAttribute("longitude");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //routeList = "EXCEPTION";
            }
            return vehicleLocation;
        }
        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
        //public event PropertyChangedEventHandler PropertyChanged;
    }
}
