using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GoByPDX.ViewModels;
using GoByPDX.Models.TripPlannerModels;
using System.Diagnostics;

using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GoByPDX.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class showTripPlanner : Page
    {

        //public transitLocationViewModel transitLocationViewModel = new transitLocationViewModel();

        public tripPlannerViewModel tripPlannerViewModel = new tripPlannerViewModel();
        public showTripPlanner()
        {
            this.InitializeComponent();
            this.DataContext = new tripPlannerViewModel();

        }

        private void loadItinerary_button(object sender, RoutedEventArgs e)
        {
            //this datas tripPlannerViewModel();
            tripPlannerViewModel tripPlanner = new tripPlannerViewModel();
            //tripPlannerPageFieldsModel tripPlannerPage = new tripPlannerPageFieldsModel(); 

            Debug.WriteLine("To: " + toTextBox.ToString());
            Debug.WriteLine("TO: " + toTextBox.Text);
            tripPlanner.getDirections(fromTextBox.Text, toTextBox.Text);
            //this.DataContext = new tripPlannerViewModel();
            this.DataContext = tripPlanner;
            // Load up a model that holds the from and to locations from the form
            //tripPlannerPage.

            if (tripPlanner.bindingPossibleFrom != null && tripPlanner.bindingPossibleFrom.Count > 0)
            {
                possibleFromComboBox.Visibility = Visibility.Visible;
            }
            if (tripPlanner.bindingPossibleTo != null && tripPlanner.bindingPossibleTo.Count > 0)
            {
                possibleToComboBox.Visibility = Visibility.Visible;
            }

            this.Frame.Navigate(typeof(showTransitLocation), vehicleTime);
        }

        private void updateFromTextBox(object sender, SelectionChangedEventArgs e)
        {
            if (possibleFromComboBox.SelectedValue != null)
            {
                fromTextBox.Text = possibleFromComboBox.SelectedValue.ToString();
                possibleFromComboBox.Visibility = Visibility.Collapsed;
            }
        }

        private void updateToTextBox(object sender, SelectionChangedEventArgs e)
        {
            if (possibleToComboBox.SelectedValue != null)
            {
                toTextBox.Text = possibleToComboBox.SelectedValue.ToString();
                possibleToComboBox.Visibility = Visibility.Collapsed;
            }
        }
    }
}
