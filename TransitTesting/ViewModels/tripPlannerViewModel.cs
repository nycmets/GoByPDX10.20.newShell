using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
//using System.Xml;
using System.Xml.Schema;
using System.Xml.Linq;
using Windows.UI.Xaml;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.Devices.Geolocation;

using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI.Xaml.Navigation;
using Windows.Data.Xml.Dom;
using GoByPDX.Models.TripPlannerModels;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml;

namespace GoByPDX.ViewModels
{
    public class tripPlannerViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<string> bindingTripInfo1 = new ObservableCollection<string>();
        public ObservableCollection<string> tripList { get { return bindingTripInfo1; } set { this.OnPropertyChanged("bindingTripInfo1"); } }

        public ObservableCollection<string> bindingPossibleTo = new ObservableCollection<string>();
        public ObservableCollection<string> possibleTo_l { get { return bindingPossibleTo; } set { this.OnPropertyChanged("bindingPossibleTo"); } }

        public ObservableCollection<string> bindingPossibleFrom = new ObservableCollection<string>();
        public ObservableCollection<string> possibleFrom_l { get { return bindingPossibleFrom; } set { this.OnPropertyChanged("bindingPossibleFrom"); } }

        public tripPlannerViewModel()
        {
        }

        async public void getDirections( string fromLocation, string toLocation)
        {
            //BasicGeoposition from_lat_lng = find_location(fromLocation);
            //BasicGeoposition to_lat_lng = find_location(toLocation);
            //Task<List<dynamic>> returnArrivalInfoListTask = Task.Run(() => loadXML(uri_arrivals, classForXML.GetType(), topProps, lowerProps, xmlNode_l));
            //arrivals = returnArrivalInfoListTask.Result;

            //Task<Geopoint> return_fromGP = Geocode(fromLocation);
            //Geopoint from_GP = await return_fromGP;

            //Task<Geopoint> return_toGP = Geocode(toLocation);
            //Geopoint to_GP = await return_toGP;

            //find_trip_directions(from_lat_lng, to_lat_lng);

            //find_trip_directions(from_GP, to_GP);
            find_trip_directions(fromLocation, toLocation);
        }

        public BasicGeoposition find_location(string location)
        {
            location = location.Replace(" ", "%20");
            location = location.Replace("and", "");

            string location_url_string = "http://dev.virtualearth.net/REST/v1/Locations?culture=en-GB&query=" + location + "%20portland%20or&o=xml&key=d8l9b6RU2EcjMncrRwx4~fKKyHyzPFPmghAi-JVfP0w~Anvx0AfcGv4Xz3O6Y_K9TNDFSsU8-LJ4ycFNteI-emI3Ts_2rn61pJL78fKq8sp0";
            Uri uri_location = new Uri(location_url_string);

            //BasicGeoposition locationPosition = new BasicGeoposition() { Latitude = Convert.ToDouble(vehicleLocation.latitude), Longitude = Convert.ToDouble(vehicleLocation.longitude) };

            Task<Geopoint> returnTripPlannerListTask = Task.Run(() => loadXML_location(uri_location));
            string test = Task.CompletedTask.ToString();

            Geopoint location_GP = returnTripPlannerListTask.Result;
            BasicGeoposition locationPosition = new BasicGeoposition() { Latitude = Convert.ToDouble(45), Longitude = Convert.ToDouble(-122) };

            //http://dev.virtualearth.net/REST/v1/Locations?culture=en-GB&query=millikan%20way%20max%20portland%20or&o=xml&key=d8l9b6RU2EcjMncrRwx4~fKKyHyzPFPmghAi-JVfP0w~Anvx0AfcGv4Xz3O6Y_K9TNDFSsU8-LJ4ycFNteI-emI3Ts_2rn61pJL78fKq8sp0
            return locationPosition;
        }

        //public void find_trip_directions(Geopoint from_GP, Geopoint to_GP)
        public void find_trip_directions(string fromLocation, string toLocation)
        {
            // FInd the coordinates of place one
            //http://dev.virtualearth.net/REST/v1/Locations?culture=en-GB&query=millikan%20way%20max%20portland%20or&o=xml&key=d8l9b6RU2EcjMncrRwx4~fKKyHyzPFPmghAi-JVfP0w~Anvx0AfcGv4Xz3O6Y_K9TNDFSsU8-LJ4ycFNteI-emI3Ts_2rn61pJL78fKq8sp0

            //string fromCoord = from_GP.Position.Latitude.ToString() + "," + from_GP.Position.Longitude.ToString();
            //string toCoord   = to_GP.Position.Latitude.ToString() + "," + to_GP.Position.Longitude.ToString();

            fromLocation = fromLocation.Replace(" ", "%20");
            //fromLocation = fromLocation.Replace("and", "");
            toLocation = toLocation.Replace(" ", "%20");
            //toLocation = toLocation.Replace("and", "");

            //string location_url_string = "http://dev.virtualearth.net/REST/v1/Locations?culture=en-GB&query=" + location + "%20portland%20or&o=xml&key=d8l9b6RU2EcjMncrRwx4~fKKyHyzPFPmghAi-JVfP0w~Anvx0AfcGv4Xz3O6Y_K9TNDFSsU8-LJ4ycFNteI-emI3Ts_2rn61pJL78fKq8sp0";
            //Uri uri_location = new Uri(location_url_string);

            DateTime time = DateTime.Now;
            string timeForURL = time.TimeOfDay.Hours.ToString() + ":" + time.TimeOfDay.Minutes.ToString();
            // Find the routes between the two places
            //string urlString_routes = "https://developer.trimet.org/ws/V1/trips/tripplanner/maxIntineraries/6/format/xml/fromplace/MILLIKAN%20WAY%20MAX%20STATION/toplace/zoo/time/11:30%20PM/arr/D/min/T/walk/0.50/mode/A/appId/7BCBE4BB29666DDCBB7D73113";
            //string urlString_routes = "https://developer.trimet.org/ws/V1/trips/tripplanner/maxIntineraries/6/format/xml/fromCoord/" + fromCoord +"/toCoord/" + toCoord +"/time/11:30%20PM/arr/D/min/T/walk/0.50/mode/A/appId/7BCBE4BB29666DDCBB7D73113";
            //string urlString_routes = "https://developer.trimet.org/ws/V1/trips/tripplanner/maxIntineraries/6/format/xml/fromPlace/" + fromLocation + "/toPlace/" + toLocation + "/time/11:30%20PM/arr/D/min/T/walk/0.50/mode/A/appId/7BCBE4BB29666DDCBB7D73113";
            string urlString_routes = "https://developer.trimet.org/ws/V1/trips/tripplanner/maxIntineraries/6/format/xml/fromPlace/" + fromLocation + "/toPlace/" + toLocation + "/time/" + timeForURL +"/arr/D/min/T/walk/0.50/mode/A/appId/7BCBE4BB29666DDCBB7D73113";


            Uri uri_routes = new Uri(urlString_routes);

            //Task<List<tripItinerary>> returnTripPlannerListTask = Task.Run(() => loadXML_tripPlanner(uri_routes));
            Task<tripPlanner> returnTripPlannerListTask = Task.Run(() => loadXML_tripPlanner(uri_routes));

            string test = Task.CompletedTask.ToString();

            //List<tripItinerary> trips_l = returnTripPlannerListTask.Result;
            tripPlanner tripPlanner = returnTripPlannerListTask.Result;

            foreach (tripItinerary tripItinerary in tripPlanner.tripItinerary_l)
            {
                string legList = "";
                foreach (tripLeg leg in tripItinerary.tripLeg_l)
                {
                    legList = legList + leg.number + "->";
                }
                //str = str.Remove(str.Length - 3);
                legList = legList.Remove(legList.Length - 2);
                //tripList.Add(tripItinerary.startTime + " " + tripItinerary.endTime);
                //_output = tripItinerary.startTime + " " + tripItinerary.endTime;
                //Output = tripItinerary.startTime + " " + tripItinerary.endTime;

                //bindingTripInfo1_test = tripItinerary.startTime + " " + tripItinerary.endTime;
                bindingTripInfo1.Add(tripItinerary.startTime + " ->" + tripItinerary.endTime + ":   " + legList);
            }
            if (tripPlanner.possibleFrom != null)
            {
                foreach (string loc in tripPlanner.possibleFrom)
                {
                    bindingPossibleFrom.Add(loc);
                }
            }
            if (tripPlanner.possibleTo != null)
            {
                foreach (string loc in tripPlanner.possibleTo)
                {
                    bindingPossibleTo.Add(loc);
                }
            }
        }



        //async private Task<List<tripItinerary>> loadXML_tripPlanner(Uri uri)
        async private Task<tripPlanner> loadXML_tripPlanner(Uri uri)
        {
            tripPlanner tripPlanner = new tripPlanner();
            //List<tripItinerary> tripItinerary_l = new List<tripItinerary>();
            tripPlanner.tripItinerary_l = new List<tripItinerary>();


            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(uri);

                //var doc = new Windows.Data.Xml.Dom.XmlDocument();
                //string url = "https://developer.trimet.org/ws/V1/trips/tripplanner/maxIntineraries/6/format/xml/fromplace/MILLIKAN%20WAY%20MAX%20STATION/toplace/pdx/time/11:30%20PM/arr/D/min/T/walk/0.50/mode/A/appId/7BCBE4BB29666DDCBB7D73113";

                var httpClient = new HttpClient();
                //HttpResponseMessage response = await httpClient.GetAsync(new Uri(url));
                HttpResponseMessage response = await httpClient.GetAsync(uri);

                var xmlStream = await response.Content.ReadAsStreamAsync();
                XDocument document = XDocument.Load(xmlStream);
                //loadedData.Document

                foreach (var test in document.Root.Elements("request"))
                {
                    Debug.WriteLine("test: " + test.ToString());
                }

                // Leave in, finds the namespace
                //var result = document.Root.Attributes().
                //        Where(a => a.IsNamespaceDeclaration).
                //        GroupBy(a => a.Name.Namespace == XNamespace.None ? String.Empty : a.Name.LocalName,
                //                a => XNamespace.Get(a.Value)).
                //        ToDictionary(g => g.Key,
                //                     g => g.First());


                string ns = "{http://maps.trimet.org/maps/model/xml}";


                foreach (var placeGuessList in document.Descendants(ns + "fromList"))
                {
                    tripPlanner.possibleFrom = new List<string> { };

                    Debug.WriteLine("Descendants: " + placeGuessList.ToString());
                    foreach (var location in placeGuessList.Descendants(ns + "location"))
                    {
                        Debug.WriteLine("Location: " + location.ToString());
                        //Debug.WriteLine("LocName: " + location.Element(ns + "description").FirstNode);
                        string element = location.Element(ns + "description").FirstNode.ToString();
                        element = element.Replace("&amp;", "and");
                        tripPlanner.possibleFrom.Add(element);
                    }
                }

                foreach (var placeGuessList in document.Descendants(ns + "toList"))
                {
                    tripPlanner.possibleTo = new List<string> { };

                    Debug.WriteLine("Descendants: " + placeGuessList.ToString());
                    foreach (var location in placeGuessList.Descendants(ns + "location"))
                    {
                        Debug.WriteLine("Location: " + location.ToString());
                        //Debug.WriteLine("LocName: " + location.Element(ns + "description"));
                        string element = location.Element(ns + "description").FirstNode.ToString();
                        element = element.Replace("&amp;", "and");
                        tripPlanner.possibleTo.Add(element);
                    }
                }
                foreach (var itinerary in document.Descendants(ns + "itinerary"))
                {
                    tripItinerary tripItinerary = new tripItinerary();

                    Debug.WriteLine("Descendants: " + itinerary.ToString());

                    tripItinerary.startTime = itinerary.Element(ns + "time-distance").Element(ns + "startTime").FirstNode.ToString();
                    tripItinerary.endTime   = itinerary.Element(ns + "time-distance").Element(ns + "endTime").FirstNode.ToString();
                    tripItinerary.duration  = itinerary.Element(ns + "time-distance").Element(ns + "duration").FirstNode.ToString();
                    tripItinerary.numberOfTransfers  = itinerary.Element(ns + "time-distance").Element(ns + "numberOfTransfers").FirstNode.ToString();
                    tripItinerary.numberOfTripLegs   = itinerary.Element(ns + "time-distance").Element(ns + "numberOfTripLegs").FirstNode.ToString();
                    tripItinerary.tripLeg_l = new List<tripLeg>();

                    foreach (var leg in itinerary.Descendants(ns + "leg"))
                    {
                        tripLeg tripLeg = new tripLeg();

                        tripLeg.mode      = leg.Attribute("mode").Value.ToString();
                        if (tripLeg.mode == "Walk")
                        {
                            tripLeg.number = "Walk";
                        } else
                        {
                            tripLeg.startTime = leg.Element(ns + "time-distance").Element(ns + "startTime").FirstNode.ToString();
                            tripLeg.endTime = leg.Element(ns + "time-distance").Element(ns + "endTime").FirstNode.ToString();
                            tripLeg.name   = leg.Element(ns + "route").Element(ns + "name").FirstNode.ToString();
                            tripLeg.number = leg.Element(ns + "route").Element(ns + "number").FirstNode.ToString();
                        } 
                        tripLeg.from      = leg.Element(ns + "from").Element(ns + "description").FirstNode.ToString();
                        tripLeg.to        = leg.Element(ns + "to").Element(ns + "description").FirstNode.ToString();

                        tripItinerary.tripLeg_l.Add(tripLeg);
                     }
                     tripPlanner.tripItinerary_l.Add(tripItinerary);
                }
                Debug.WriteLine("NSP STOP:");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION");
            }
            return tripPlanner;
        }

        private async Task<Geopoint> Geocode(string location)
        {
            Geopoint locPosition_GP = null;
            //Windows.Services.Maps.LocalSearch.LocalLocationFinder


            // Address or business to geocode.
            //string addressToGeocode = "nike";
            string addressToGeocode = "jenkins and muarry";


            // Nearby location to use as a query hint.
            BasicGeoposition queryHint = new BasicGeoposition();
            // Portland Lat Lng  45.51179° N, -122.6756° E
            queryHint.Latitude = 45.51179;
            queryHint.Longitude = -122.6756;
            Geopoint hintPoint = new Geopoint(queryHint);

            // Geocode the specified address, using the specified reference point
            // as a query hint. Return no more than 3 results.
            MapLocationFinderResult result =
                await MapLocationFinder.FindLocationsAsync(
                                    //addressToGeocode,
                                    location,
                                    hintPoint,
                                    3);

            // If the query returns results, display the coordinates
            // of the first result.
            if (result.Status == MapLocationFinderStatus.Success && result.Locations.Count > 0)
            {
                string lat = result.Locations[0].Point.Position.Latitude.ToString();
                string lng = result.Locations[0].Point.Position.Longitude.ToString();
                Debug.WriteLine("Result Lat: " + result.Locations[0].Point.Position.Latitude.ToString());
                Debug.WriteLine("Result Lat: " + result.Locations[0].Point.Position.Longitude.ToString());
                Debug.WriteLine(result.Locations[0].Point.Position.Latitude.ToString() + "," + result.Locations[0].Point.Position.Longitude.ToString());
                BasicGeoposition locPosition = new BasicGeoposition() { Latitude = Convert.ToDouble(lat), Longitude = Convert.ToDouble(lng) };
                Geopoint locPosition_GP_prelim = new Geopoint(locPosition);


                MapLocationFinderResult stateResult =
                await MapLocationFinder.FindLocationsAtAsync(
                                   //addressToGeocode,
                                   locPosition_GP_prelim);
                if (stateResult.Status == MapLocationFinderStatus.Success && stateResult.Locations.Count > 0)
                {
                    //string lat = result.Locations[0].Point.Position.Latitude.ToString();
                    //string lng = result.Locations[0].Point.Position.Longitude.ToString();
                    Debug.WriteLine("Result Lat: " + stateResult.Locations[0].Address.Region.ToString());
                    if (stateResult.Locations[0].Address.Region.ToString() == "Oregon")
                    {
                        locPosition_GP = new Geopoint(locPosition);

                        //Debug.WriteLine("No result found: " + location);
                    }
                    //Point.Position.Latitude.ToString());
                }
            } else
            {
                Debug.WriteLine("No result found: " + location);
            }
            return locPosition_GP;

        }


        async private Task<Geopoint> loadXML_location(Uri uri)
        {
            Geopoint locPosition_GP = null;

            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(uri);

                var httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                var xmlStream = await response.Content.ReadAsStreamAsync();
                XDocument document = XDocument.Load(xmlStream);
                //loadedData.Document

                // Leave in, finds the namespace
                //var result = document.Root.Attributes().
                //        Where(a => a.IsNamespaceDeclaration).
                //        GroupBy(a => a.Name.Namespace == XNamespace.None ? String.Empty : a.Name.LocalName,
                //                a => XNamespace.Get(a.Value)).
                //        ToDictionary(g => g.Key,
                //                     g => g.First());


                string ns = "{http://schemas.microsoft.com/search/local/ws/rest/v1}";

                foreach (var geocodepoint in document.Descendants(ns + "GeocodePoint"))
                {
                    Debug.WriteLine("Descendants: " + geocodepoint.ToString());

                    Debug.WriteLine("LAT: " + geocodepoint.Element(ns + "Latitude").FirstNode.ToString());
                    Debug.WriteLine("LNG: " + geocodepoint.Element(ns + "Longitude").FirstNode.ToString());

                    string lat = geocodepoint.Element(ns + "Latitude").FirstNode.ToString();
                    string lng = geocodepoint.Element(ns + "Longitude").FirstNode.ToString();

                    BasicGeoposition locPosition = new BasicGeoposition() { Latitude = Convert.ToDouble(lat), Longitude = Convert.ToDouble(lng) };
                    locPosition_GP = new Geopoint(locPosition);
                }

                Debug.WriteLine("NSP STOP:");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION");
            }
            return locPosition_GP;
        }



        async private Task<List<tripItinerary>> loadXML_locationOLD(Uri uri)
        {
            List<tripItinerary> tripPlanner = new List<tripItinerary>();

            try
            {
                var client = new HttpClient();
                var stream = await client.GetStreamAsync(uri);
                XmlReader reader = XmlReader.Create(stream);

                reader.ReadToDescendant("GeocodePoint");
                XmlReader innerRead = reader.ReadSubtree();
                while (innerRead.Read())
                {
                    Debug.WriteLine("reader node 1: " + reader.Name);

                    if (reader.Name == "Latitude")
                    {
                        //reader.Read();
                        Debug.WriteLine("Attribute: " + reader.GetAttribute("Latitude"));
                        Debug.Write("Value: " + reader.Value);
                        Debug.WriteLine("reader hasval 1: " + reader.HasValue.ToString());

                        reader.Read();

                        Debug.WriteLine("Attribute: " + reader.GetAttribute("Latitude"));
                        Debug.Write("Value: " + reader.Value);
                        Debug.WriteLine("reader hasval 1: " + reader.HasValue.ToString());
                        reader.Read();
                        Debug.Write("Value: " + reader.Value);


                    }

                    reader.Read();

                    Debug.WriteLine("reader att 1: " + reader.GetAttribute("Latitude"));
                    Debug.WriteLine("reader hasatt 1: " + reader.HasAttributes.ToString());
                    Debug.WriteLine("reader hasval 1: " + reader.HasValue.ToString());

                    reader.GetAttribute("Latitude");
                    reader.Read();
                    reader.GetAttribute("Longitude");
                }

               


            }
            catch (Exception ex)
            {
                Debug.WriteLine("EXCEPTION");
            }
            return tripPlanner;
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
