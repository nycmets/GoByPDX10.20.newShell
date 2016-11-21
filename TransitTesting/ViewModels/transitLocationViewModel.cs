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


namespace GoByPDX
{
    public class transitLocationViewModel
    {
        public vehicleLocation vehicleLocation = new vehicleLocation();

        public void loadLocationInfo(string vehicleID)
        {
            Debug.WriteLine("NSP Dir Selected: " + vehicleID);
            string urlString_stopInfo = "http://developer.trimet.org/ws/v2/vehicles/json/false/ids/" + vehicleID + "/appID/7BCBE4BB29666DDCBB7D73113";
            Uri uri_stops = new Uri(urlString_stopInfo);
            Task<vehicleLocation> returnLocationInfoTask = Task.Run(() => loadXML_vehicleLocations(uri_stops));
            vehicleLocation = returnLocationInfoTask.Result;
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
    }
}
