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
using GoByPDX.Models.TripPlannerModels;
using System.Collections.ObjectModel;
using System.ComponentModel;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GoByPDX.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class tripPlannerResults : Page
    {
       
        public tripPlannerResults()
        {
            this.InitializeComponent();
        }
        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DataContext = new tripPlannerResultsViewModel();
        }

        private void loadNextResults(object sender, RoutedEventArgs e)
        {
            tripPlannerViewModel nextFive = new tripPlannerViewModel();

            DateTime oldTime = App.tripPlanner.leaveTime;
            tripItinerary lastTrip = App.tripPlanner.tripItinerary_l.Last<tripItinerary>();
            //1:57 PM
            //IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-US");
            DateTime lastStartTime = Convert.ToDateTime(lastTrip.startTime);
            //DateTime MyDateTime = DateTime.ParseExact(lastTrip.startTime, "HH:mm tt", theCultureInfo);
            App.tripPlanner.leaveTime = lastStartTime.AddMinutes(2);
            //App.tripPlanner.leaveTime = MyDateTime;


            nextFive.find_trip_directions(App.tripPlanner.fromString, App.tripPlanner.toString, App.tripPlanner.leaveTime);
            new tripPlannerViewModel();
            this.DataContext = new tripPlannerResultsViewModel();
        }
    }
}

