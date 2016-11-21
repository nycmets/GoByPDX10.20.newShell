using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using Windows.UI.Xaml.Controls.Maps;
using System.Globalization;
using Windows.Devices.Geolocation;
using System.Diagnostics;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GoByPDX.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class showTransitLocation : Page
    {
        public transitLocationViewModel transitLocationViewModel = new transitLocationViewModel();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        async protected override void OnNavigatedTo(NavigationEventArgs source)
        {
            //Debug.WriteLine("NSP here");

            DataContext = null;
            DataContext = App.routeListViewModel;
            //string vehicleTime = e.AddedItems[0].ToString();
            Debug.WriteLine("NSP transitSel: " + App.lastState.vehicleScheduledTime);

            string vehicleID = "";
            string stopLng = "";
            string stopLat = "";
            string stopName = "";
            string scheduled = "";
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

            //NSP write out the lat/lng from the webpage
            //string vehicleID = App.lastState.vehicleID;

            cTitleBox.Text = stopName + " - " + scheduled;

            transitLocationViewModel.loadLocationInfo(vehicleID);

            // Add the MapControl and the specify maps authentication key.
            MapControl MapControl = new MapControl();
            MapControl.ZoomInteractionMode = MapInteractionMode.GestureAndControl;
            MapControl.TiltInteractionMode = MapInteractionMode.GestureAndControl;
            MapControl.MapServiceToken = "d8l9b6RU2EcjMncrRwx4~fKKyHyzPFPmghAi-JVfP0w~Anvx0AfcGv4Xz3O6Y_K9TNDFSsU8-LJ4ycFNteI-emI3Ts_2rn61pJL78fKq8sp0";
            transitMap.Children.Add(MapControl);

            // Specify lat, lng for the vehicle
            BasicGeoposition vehiclePosition = new BasicGeoposition() { Latitude = Convert.ToDouble(transitLocationViewModel.vehicleLocation.latitude), Longitude = Convert.ToDouble(transitLocationViewModel.vehicleLocation.longitude) };
            Geopoint vehiclePoint = new Geopoint(vehiclePosition);

            // Create a MapIcon for Vehicles
            MapIcon mapIcon1 = new MapIcon();
            mapIcon1.Location = vehiclePoint;
            mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon1.Title = "Arrives " + scheduled;
            mapIcon1.ZIndex = 0;

            // Specify lat, lng for the vehicle
            // BasicGeoposition snPosition = new BasicGeoposition() { Latitude = Convert.ToDouble(stopLat), Longitude = Convert.ToDouble(stopLng) };
            BasicGeoposition snPosition = new BasicGeoposition() { Latitude = Convert.ToDouble(stopLat), Longitude = Convert.ToDouble(stopLng) };

            Geopoint stopPoint = new Geopoint(snPosition);
            
            // Create a MapIcon for the stop
            MapIcon mapIcon2 = new MapIcon();
            mapIcon2.Location = stopPoint;
            mapIcon2.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon2.Title = stopName;
            mapIcon2.ZIndex = 0;

            // Add the MapIcon to the map.
            MapControl.MapElements.Add(mapIcon1);
            MapControl.MapElements.Add(mapIcon2);

            double stopLatDoub = Convert.ToDouble(stopLat);
            double stopLngDoub = Convert.ToDouble(stopLng);
            double vehicleLatDoub = Convert.ToDouble(transitLocationViewModel.vehicleLocation.latitude);
            double vehicleLngDoub = Convert.ToDouble(transitLocationViewModel.vehicleLocation.longitude);
            double averageLat = (stopLatDoub + vehicleLatDoub) / 2;
            double averageLng = (stopLngDoub + vehicleLngDoub) / 2;


            // Specify lat, lng for the vehicle
            //BasicGeoposition test = new BasicGeoposition() { Latitude = averageLat, Longitude = averageLng };
            //Geopoint testPoint = new Geopoint(test);
            //double bigLat = functions.findMax(stopLatDoub, vehicleLatDoub);
            //double lowLat = functions.findMin(stopLatDoub, vehicleLatDoub);
            //double bigLng = functions.findMax(stopLngDoub, vehicleLngDoub);
            //double lowLng = functions.findMin(stopLngDoub, vehicleLngDoub);
            //BasicGeoposition northWest = new BasicGeoposition() { Latitude = stopLatDoub, Longitude = stopLngDoub };
            //BasicGeoposition southEast = new BasicGeoposition() { Latitude = vehicleLatDoub, Longitude = vehicleLngDoub };

            //List<BasicGeoposition> basicPositions = new List<BasicGeoposition>();
            //basicPositions.Add(new BasicGeoposition() { Latitude = lowLat, Longitude = bigLng });
            //basicPositions.Add(new BasicGeoposition() { Latitude = bigLat, Longitude = lowLng });

            //this.Map.TrySetViewBoundsAsync(GeoboundingBox.TryCompute(basicPositions), null, MapAnimationKind.Default);
            //GeoboundingBox.TryCompute(basicPositions)
            // Center the map over the POI.
            MapControl.Center = stopPoint;
            //GeoboundingBox bBox = new GeoboundingBox(northWest, southEast);
            //await MapControl.TrySetViewBoundsAsync(GeoboundingBox.TryCompute(basicPositions), null, MapAnimationKind.Default);
            //MapControl.TrySetViewBoundsAsync(bBox, 10, animation: 0);
            MapControl.ZoomLevel = 14;
        }

        public showTransitLocation()
        {
            this.InitializeComponent();
            this.DataContext = transitLocationViewModel;
        }
    }
}
