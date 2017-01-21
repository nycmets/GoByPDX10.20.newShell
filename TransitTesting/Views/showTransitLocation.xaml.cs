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
        MapIcon mapIcon1 = new MapIcon();
        //Geopoint vehiclePoint = Geopoint();
        MapIcon mapIcon2 = new MapIcon();
        // vehiclePoint { get; set; }




        public transitLocationViewModel transitLocationViewModel = new transitLocationViewModel();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        async protected override void OnNavigatedTo(NavigationEventArgs source)
        {
            //Debug.WriteLine("NSP here");

            DataContext = null;
            DataContext = transitLocationViewModel;

            // This was uncommented, moved to the view model
            ////string vehicleTime = e.AddedItems[0].ToString();
            //Debug.WriteLine("NSP transitSel: " + App.lastState.vehicleScheduledTime);

            //string vehicleID = "";
            //string stopLng = "";
            //string stopLat = "";
            //string stopName = "";
            //string scheduled = "";
            ////var vehicleClass = App.routeListViewModel.arrivals.Where(e => String.Equals(e.scheduled, source.Parameter)).Select(ret => ret);
            //var vehicleClass = App.routeListViewModel.arrivals.Where(e => String.Equals(e.scheduled, App.lastState.vehicleScheduledTime)).Select(ret => ret);


            //foreach (var ret in vehicleClass)
            //{
            //    string[] schList = ret.scheduled.Split();
            //    scheduled = schList[1] + " " + schList[2];
            //    vehicleID = ret.vehicleID;
            //    stopLng = ret.lng;
            //    stopLat = ret.lat;
            //    stopName = ret.desc;
            //}
            // END  This was uncommented, moved to the view model



            //NSP write out the lat/lng from the webpage
            //string vehicleID = App.lastState.vehicleID;

            //cTitleBox.Text = stopName + " - " + scheduled;
            cTitleBox.Text = transitLocationViewModel.stopName + " - " + transitLocationViewModel.scheduled;
            
            //NSP 1/7 commented out till the end for testing
            //// Add the MapControl and the specify maps authentication key.
            myMap.ZoomInteractionMode = MapInteractionMode.GestureAndControl;
            myMap.TiltInteractionMode = MapInteractionMode.GestureAndControl;
            myMap.MapServiceToken = "d8l9b6RU2EcjMncrRwx4~fKKyHyzPFPmghAi-JVfP0w~Anvx0AfcGv4Xz3O6Y_K9TNDFSsU8-LJ4ycFNteI-emI3Ts_2rn61pJL78fKq8sp0";

            ////this.InitializeComponent();
            ////myMap.Loaded += MyMap_Loaded;

            //// Specify lat, lng for the vehicle
            ////BasicGeoposition vehiclePosition = new BasicGeoposition() { Latitude = Convert.ToDouble(transitLocationViewModel.vehicleLocation.latitude), Longitude = Convert.ToDouble(transitLocationViewModel.vehicleLocation.longitude) };
            ////Geopoint vehiclePoint = new Geopoint(vehiclePosition);
            //transitLocationViewModel.loadLocationInfo(vehicleID);
            ////Geopoint vehiclePoint = new Geopoint(transitLocationViewModel.vehiclePosition);
            //vehiclePoint = new Geopoint(transitLocationViewModel.vehiclePosition);

            //Debug.WriteLine("Orig Lat: " + transitLocationViewModel.vehiclePosition.Latitude);
            //Debug.WriteLine("Orig Lng: " + transitLocationViewModel.vehiclePosition.Longitude);


            //// Create a MapIcon for Vehicles
            ////MapIcon mapIcon1 = new MapIcon();
            ////mapIcon1.Location = vehiclePoint;
            ////mapIcon1.Location = transitLocationViewModel.vehiclePoint;
            ////mapIcon1.Location = transitLocationViewModel.vehiclePoint[0];

            //mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
            //mapIcon1.Title = "Arrives " + scheduled;
            //mapIcon1.ZIndex = 0;


            //// Specify lat, lng for the vehicle
            //// BasicGeoposition snPosition = new BasicGeoposition() { Latitude = Convert.ToDouble(stopLat), Longitude = Convert.ToDouble(stopLng) };
            //BasicGeoposition snPosition = new BasicGeoposition() { Latitude = Convert.ToDouble(stopLat), Longitude = Convert.ToDouble(stopLng) };

            //Geopoint stopPoint = new Geopoint(snPosition);

            // Create a MapIcon for the stop
            MapIcon mapIcon_stop = new MapIcon();
            mapIcon_stop.Location = transitLocationViewModel.stopPoint;
            mapIcon_stop.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon_stop.Title = transitLocationViewModel.stopName;
            mapIcon_stop.ZIndex = 0;
            myMap.MapElements.Add(mapIcon_stop);



            ////this.Map.TrySetViewBoundsAsync(GeoboundingBox.TryCompute(basicPositions), null, MapAnimationKind.Default);
            ////GeoboundingBox.TryCompute(basicPositions)
            //// Center the map over the POI.
            myMap.Center = transitLocationViewModel.stopPoint;
            //string test = MapControl.LocationProperty.ToString();
            //Debug.WriteLine("NSP test: " + test);
            //mapIconAddButton_Click(stopPoint, stopName);
            //string vehicleTitle = "Arrives " + scheduled;
            mapIconRemoveButton_Click(transitLocationViewModel.vehiclePoint, "Arrives " + transitLocationViewModel.scheduled);// vehiclePoint, vehicleTitle);

            mapIconAddButton_Click(transitLocationViewModel.vehiclePoint, "Arrives " + transitLocationViewModel.scheduled);// vehiclePoint, vehicleTitle);

            //mapIconAddButton_Click(vehiclePoint, vehicleTitle);

            ////GeoboundingBox bBox = new GeoboundingBox(northWest, southEast);
            ////await MapControl.TrySetViewBoundsAsync(GeoboundingBox.TryCompute(basicPositions), null, MapAnimationKind.Default);
            ////MapControl.TrySetViewBoundsAsync(bBox, 10, animation: 0);
            myMap.ZoomLevel = 14; 
        }

        public showTransitLocation()
        {
            this.InitializeComponent();
            this.DataContext = transitLocationViewModel;
        }

        //public void mapRefreshButton(object sender, RoutedEventArgs e)
        public void mapRefreshButton()
        {
            //RefreshRequested(frame, frame.SourcePageType);
            //Shell shell = new Shell();
            //shell.RefreshRequested(Frame, Frame.SourcePageType);
            Geopoint vehiclePointToRemove = new Geopoint(transitLocationViewModel.vehiclePosition);
            mapIconRemoveButton_Click(vehiclePointToRemove, "Remove");
            Debug.WriteLine("Old Lat: " + transitLocationViewModel.vehiclePosition.Latitude);
            Debug.WriteLine("Old Lng: " + transitLocationViewModel.vehiclePosition.Longitude);


            transitLocationViewModel.loadLocationInfo(transitLocationViewModel.vehicleIDprev);
           // Geopoint vehiclePoint = new Geopoint(transitLocationViewModel.vehiclePosition);
            Geopoint vehiclePoint = new Geopoint(transitLocationViewModel.vehiclePosition);

            Debug.WriteLine("New Lat: " + transitLocationViewModel.vehiclePosition.Latitude);
            Debug.WriteLine("New Lng: " + transitLocationViewModel.vehiclePosition.Longitude);
            mapIconAddButton_Click(vehiclePoint, "VehicleRef");

        }

        public void mapRefreshButton(object sender, RoutedEventArgs e)
        {
            //RefreshRequested(frame, frame.SourcePageType);
            //Shell shell = new Shell();
            //shell.RefreshRequested(Frame, Frame.SourcePageType);
            Geopoint vehiclePointToRemove = new Geopoint(transitLocationViewModel.vehiclePosition);
            mapIconRemoveButton_Click(vehiclePointToRemove, "Remove");
            Debug.WriteLine("Old Lat: " + transitLocationViewModel.vehiclePosition.Latitude);
            Debug.WriteLine("Old Lng: " + transitLocationViewModel.vehiclePosition.Longitude);


            transitLocationViewModel.loadLocationInfo(transitLocationViewModel.vehicleIDprev);
            // Geopoint vehiclePoint = new Geopoint(transitLocationViewModel.vehiclePosition);
            Geopoint vehiclePoint = new Geopoint(transitLocationViewModel.vehiclePosition);

            Debug.WriteLine("New Lat: " + transitLocationViewModel.vehiclePosition.Latitude);
            Debug.WriteLine("New Lng: " + transitLocationViewModel.vehiclePosition.Longitude);
            mapIconAddButton_Click(vehiclePoint, "Arrives " + transitLocationViewModel.scheduled);

        }

        private void mapIconRemoveButton_Click(Geopoint stopPoint, string stopName)
        {
            // Create a MapIcon for the stop
            //MapIcon mapIcon2 = new MapIcon();
            mapIcon2.Location = stopPoint;
            mapIcon2.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon2.Title = stopName;
            mapIcon2.ZIndex = 0;

            // Add the MapIcon to the map.
            //  MapControl.MapElements.Add(mapIcon1);
            myMap.MapElements.Remove(mapIcon2);
        }


        //private void mapIconAddButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        private void mapIconAddButton_Click(Geopoint stopPoint, string stopName)
        {
            // Create a MapIcon for the stop
            //MapIcon mapIcon2 = new MapIcon();
            mapIcon2.Location = stopPoint;
            mapIcon2.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon2.Title = stopName;
            mapIcon2.ZIndex = 0;

            // Add the MapIcon to the map.
          //  MapControl.MapElements.Add(mapIcon1);
            myMap.MapElements.Add(mapIcon2);

        }

        private void MyMap_Loaded(object sender, RoutedEventArgs e)
        {
            //myMap.Center =
            //   new Geopoint(new BasicGeoposition()
            //   {
            //       //Geopoint for Seattle 
            //       Latitude = 47.604,
            //       Longitude = -122.329
            //   });
            //myMap.ZoomLevel = 17;
        }
    }
}
