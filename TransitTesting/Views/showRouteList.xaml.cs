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
using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;
using System.Net.Http;
using GoByPDX;
using GoByPDX.Views;
using Windows.UI.Popups;
using Windows.Networking.Connectivity;
using GoByPDX.Dto;
using SQLite.Net;
using Windows.Storage;
using SQLite.Net.Platform.WinRT;
using System.Collections.ObjectModel;
using GoByPDX.ViewModels;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GoByPDX.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class showRouteList : Page
    {
        //funcsNonStatic funcsNonStatic = new GoByPDX.funcsNonStatic();

        async private void messageFunc()
        {
            Frame rootFrame = Window.Current.Content as Frame;
            //rootFrame.Navigate(typeof(showRouteList));
            
            var messageDialog = new MessageDialog("Could Not Connect to Transit Servers");
            // Add commands and set their callbacks; both buttons use the same callback function instead of inline event handlers
            messageDialog.Commands.Add(new UICommand(
                "Try again",
            //this.Frame.Navigate(typeof(showTransitLocation), vehicleTime);

            new UICommandInvokedHandler(this.CommandInvokedHandler)));

            messageDialog.Commands.Add(new UICommand(
                "Close",
                new UICommandInvokedHandler(this.CommandInvokedHandler)));

            // Set the command that will be invoked by default
            messageDialog.DefaultCommandIndex = 0;
            // Set the command to be invoked when escape is pressed
            messageDialog.CancelCommandIndex = 1;
            object test = await messageDialog.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            this.Frame.Navigate(typeof(showRouteList));
        }

        public static bool IsInternet()
        {
            ConnectionProfile connections = NetworkInformation.GetInternetConnectionProfile();
            bool internet = connections != null && connections.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            return internet;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);             //not needed, base method is empty and does nothing

            if (IsInternet())
            {
                DataContext = null;                //important part, whenever you navigate, refreshes the    ViewModel - no deletion, just resetting of the DataContext, so the page won't get stuck
                DataContext = App.routeListViewModel;      //and finally the resetting  
                if (showRouteTabularView.Visibility.ToString() == "Visible")
                {
                    routeComboBox.SelectedIndex = App.routeListViewModel.routeComboIndex;
                    directionComboBox.SelectedIndex = App.routeListViewModel.dirComboIndex;
                    stopsComboBox.SelectedIndex = App.routeListViewModel.stopComboIndex;
                }
                else if (showRouteColumnarView.Visibility.ToString() == "Visible")
                {
                    cRouteComboBox.SelectedIndex = App.routeListViewModel.routeComboIndex;
                    cDirectionComboBox.SelectedIndex = App.routeListViewModel.dirComboIndex;
                    cStopsComboBox.SelectedIndex = App.routeListViewModel.stopComboIndex;
                }
            }
            else
            {
                messageFunc();
            }

        }

        public showRouteList()
        {
            this.InitializeComponent();
            //this.DataContext = App.routeListViewModel;
        }

        private void showDirInfo(object sender, SelectionChangedEventArgs tmp)
        {
            //this.DataContext = App.routeListViewModel;

            string description = "";
            description = functions.myComboVal(routeComboBox.SelectedValue, cRouteComboBox.SelectedValue);

            App.routeListViewModel.loadDirInfo(description);
        }

        private void showStopInfo(object sender, SelectionChangedEventArgs tmp)
        {
            //Debug.WriteLine("Selected: {0}", tmp.AddedItems[0]);
            //this.DataContext = App.routeListViewModel;

            //string routeDesc = "";
            string routeID = "";
            string routeDirDesc = functions.myComboVal(directionComboBox.SelectedValue, cDirectionComboBox.SelectedValue);
            string routeDesc = functions.myComboVal(routeComboBox.SelectedValue, cRouteComboBox.SelectedValue);

            if (routeDesc != "")
            {
                string routeDir = "";

                var routeClassDesc = App.routeListViewModel.routes.Where(e => String.Equals(e.desc, routeDesc)).Select(ret2 => ret2);
                foreach (var ret in routeClassDesc) { if (ret.dirDesc == routeDirDesc) { routeID = ret.route; routeDir = ret.dir; } }

                App.routeListViewModel.loadStopInfo(routeDir, routeID);
            }
        }

        private void showNextArrivals(object sender, SelectionChangedEventArgs tmp)
        {
            ComboBox routeComboBox_all = functions.comboSelection(routeComboBox, cRouteComboBox);
            ComboBox dirComboBox_all   = functions.comboSelection(directionComboBox, cDirectionComboBox);
            ComboBox stopsComboBox_all = functions.comboSelection(stopsComboBox, cStopsComboBox);

            if (routeComboBox_all != null && dirComboBox_all != null && stopsComboBox_all != null)
            {
                ToggleFavRouteButton.IsChecked = App.routeListViewModel.showNextArrivals(routeComboBox_all, dirComboBox_all, stopsComboBox_all);
            }
            else
            {
                ToggleFavRouteButton.IsChecked = false;
            }
        }

        private void transitSelected(object sender, SelectionChangedEventArgs e)
        {
            string listItemSelected = functions.myComboVal(listView.SelectedItem, cListView.SelectedItem);
            if (listItemSelected != null)
            {
                string vehicleTime = listItemSelected;

                vehicleLocation vehicleLocation = new vehicleLocation();
                vehicleLocation.vehicleTime = vehicleTime;
                App.lastState.vehicleScheduledTime = vehicleTime;

                string vehicleID = "";
                var vehicleClass = App.routeListViewModel.arrivals.Where(e2 => String.Equals(e2.scheduled, vehicleTime)).Select(ret => ret);

                foreach (var ret in vehicleClass)
                {
                    vehicleID = ret.vehicleID;
                }

                App.lastState.vehicleID = vehicleID;
                App.lastState.vehicleScheduledTime = vehicleTime;
                this.Frame.Navigate(typeof(showTransitLocation), vehicleTime);
                App.routeListViewModel.routeComboIndex = Convert.ToInt32(functions.PopulateComboIndex(routeComboBox.SelectedIndex, cRouteComboBox.SelectedIndex));
                App.routeListViewModel.dirComboIndex = Convert.ToInt32(functions.PopulateComboIndex(directionComboBox.SelectedIndex, cDirectionComboBox.SelectedIndex));
                App.routeListViewModel.stopComboIndex = Convert.ToInt32(functions.PopulateComboIndex(stopsComboBox.SelectedIndex, cStopsComboBox.SelectedIndex));
            }
        }

        private void FavRouteButtonToggled(object sender, RoutedEventArgs e)
        {
            ComboBox routeComboBox_all = functions.comboSelection(routeComboBox, cRouteComboBox);
            ComboBox dirComboBox_all = functions.comboSelection(directionComboBox, cDirectionComboBox);
            ComboBox stopsComboBox_all = functions.comboSelection(stopsComboBox, cStopsComboBox);

            if (routeComboBox_all != null && dirComboBox_all != null && stopsComboBox_all != null)
            {
                favoritesListViewModel favListVM = new favoritesListViewModel();
                ToggleFavRouteButton.Content = favListVM.FavRouteButtonToggled(ToggleFavRouteButton, routeComboBox_all, dirComboBox_all, stopsComboBox_all);
            }
        }


    }
}

