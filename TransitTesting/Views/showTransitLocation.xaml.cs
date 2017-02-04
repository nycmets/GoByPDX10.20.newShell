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
        MapIcon mapIcon = new MapIcon();


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

            cTitleBox.Text = transitLocationViewModel.stopName + " - " + transitLocationViewModel.estimated;
            
            //NSP 1/7 commented out till the end for testing
            //// Add the MapControl and the specify maps authentication key.
            myMap.ZoomInteractionMode = MapInteractionMode.GestureAndControl;
            myMap.TiltInteractionMode = MapInteractionMode.GestureAndControl;
            myMap.MapServiceToken = "d8l9b6RU2EcjMncrRwx4~fKKyHyzPFPmghAi-JVfP0w~Anvx0AfcGv4Xz3O6Y_K9TNDFSsU8-LJ4ycFNteI-emI3Ts_2rn61pJL78fKq8sp0";

            // Create a MapIcon for the stop
            MapIcon mapIcon_stop = new MapIcon();
            mapIcon_stop.Location = transitLocationViewModel.stopPoint;
            mapIcon_stop.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon_stop.Title = transitLocationViewModel.stopName;
            mapIcon_stop.ZIndex = 0;
            myMap.MapElements.Add(mapIcon_stop);

            //// Center the map over the POI.
            myMap.Center = transitLocationViewModel.stopPoint;

            mapIconRemoveButton_Click(transitLocationViewModel.vehiclePoint, "Arrives " + transitLocationViewModel.estimated);// vehiclePoint, vehicleTitle);
            mapIconAddButton_Click(transitLocationViewModel.vehiclePoint, "Arrives " + transitLocationViewModel.estimated);// vehiclePoint, vehicleTitle);

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
            transitLocationViewModel.loadLocationInfo(transitLocationViewModel.vehicleID);
            //DataContext = transitLocationViewModel;

            Geopoint vehiclePointToRemove = new Geopoint(transitLocationViewModel.vehiclePosition);
            mapIconRemoveButton_Click(vehiclePointToRemove, "Remove");
            //Debug.WriteLine("Old Lat: " + transitLocationViewModel.vehiclePosition.Latitude);
            //Debug.WriteLine("Old Lng: " + transitLocationViewModel.vehiclePosition.Longitude);


            //transitLocationViewModel.loadLocationInfo(transitLocationViewModel.vehicleIDprev);
           // Geopoint vehiclePoint = new Geopoint(transitLocationViewModel.vehiclePosition);
            Geopoint vehiclePoint = new Geopoint(transitLocationViewModel.vehiclePosition);

            Debug.WriteLine("New Lat: " + transitLocationViewModel.vehiclePosition.Latitude);
            Debug.WriteLine("New Lng: " + transitLocationViewModel.vehiclePosition.Longitude);
            mapIconAddButton_Click(vehiclePoint, "Arrives " + transitLocationViewModel.estimated);

        }

        public void mapRefreshButton(object sender, RoutedEventArgs e)
        {
            transitLocationViewModel.loadLocationInfo(transitLocationViewModel.vehicleID);
            //DataContext = transitLocationViewModel;

            Geopoint vehiclePointToRemove = new Geopoint(transitLocationViewModel.vehiclePosition);
            mapIconRemoveButton_Click(vehiclePointToRemove, "Remove");
            //Debug.WriteLine("Old Lat: " + transitLocationViewModel.vehiclePosition.Latitude);
            //Debug.WriteLine("Old Lng: " + transitLocationViewModel.vehiclePosition.Longitude);


            //transitLocationViewModel.loadLocationInfo(transitLocationViewModel.vehicleIDprev);
            // Geopoint vehiclePoint = new Geopoint(transitLocationViewModel.vehiclePosition);
            Geopoint vehiclePoint = new Geopoint(transitLocationViewModel.vehiclePosition);

            Debug.WriteLine("New Lat: " + transitLocationViewModel.vehiclePosition.Latitude);
            Debug.WriteLine("New Lng: " + transitLocationViewModel.vehiclePosition.Longitude);
            mapIconAddButton_Click(vehiclePoint, "Arrives " + transitLocationViewModel.estimated);

        }

        private void mapIconRemoveButton_Click(Geopoint stopPoint, string stopName)
        {
            // Create a MapIcon for the stop
            //MapIcon mapIcon = new MapIcon();
            mapIcon.Location = stopPoint;
            mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon.Title = stopName;
            mapIcon.ZIndex = 0;

            // Add the MapIcon to the map.
            //  MapControl.MapElements.Add(mapIcon1);
            myMap.MapElements.Remove(mapIcon);
        }


        //private void mapIconAddButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        private void mapIconAddButton_Click(Geopoint stopPoint, string stopName)
        {
            // Create a MapIcon for the stop
            //MapIcon mapIcon = new MapIcon();
            mapIcon.Location = stopPoint;
            mapIcon.NormalizedAnchorPoint = new Point(0.5, 1.0);
            mapIcon.Title = stopName;
            mapIcon.ZIndex = 0;

            // Add the MapIcon to the map.
          //  MapControl.MapElements.Add(mapIcon1);
            myMap.MapElements.Add(mapIcon);

        }
    }
}
