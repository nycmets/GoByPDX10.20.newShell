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
 
            string routeComboBoxValueDesc = functions.myComboVal(routeComboBox.SelectedValue, cRouteComboBox.SelectedValue);

            string routeID = "";
            var routeClass = App.routeListViewModel.routes.Where(e => String.Equals(e.desc, routeComboBoxValueDesc)).Select(ret2 => ret2);

            foreach (var ret in routeClass) { routeID = ret.route; }

            string description = functions.myComboVal(stopsComboBox.SelectedValue, cStopsComboBox.SelectedValue);

            if (description != "" && description != null)
            { 
                string locIDs = "";
                var stopClass = App.routeListViewModel.stops.Where(e => String.Equals(e.desc, description)).Select(ret2 => ret2);

                foreach (var ret in stopClass) { locIDs = ret.LocID; }
                App.routeListViewModel.loadNextArrivals(locIDs, routeID);
                //this.DataContext = App.routeListViewModel;
            }
            //TODO CHeck the favorite star and see if route is favorite
            if (cRouteComboBox.SelectedValue != null && cDirectionComboBox.SelectedValue != null && cStopsComboBox.SelectedValue != null)
            {
                string Route = cRouteComboBox.SelectedValue.ToString();
                string Dir = cDirectionComboBox.SelectedValue.ToString();
                string Stop = cStopsComboBox.SelectedValue.ToString();

                int contactid = UpdateDetails(Route, Dir, Stop);
                if (contactid != -1)
                {
                    ToggleFavRouteButton.IsChecked = true;
                } else
                {
                    ToggleFavRouteButton.IsChecked = false;
                }
            } else
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

        private void FavRouteButtonUnChecked(object sender, RoutedEventArgs e)
        {
            ToggleFavRouteButton.Content = "\uE1CE";
            SolidColorBrush blackBrush = new SolidColorBrush(Windows.UI.Colors.Black);
            //ToggleFavRouteButton.Foreground = blackBrush;
            //ToggleFavRouteButton.Foreground = Theme SystemControlForegroundBaseHighBrush
            //ToggleFavRouteButton.Foreground = { SystemControlForegroundBaseHighBrush;
            Dto.Favorites favorite = new Dto.Favorites();
            if (cRouteComboBox.SelectedValue != null && cDirectionComboBox.SelectedValue != null && cStopsComboBox.SelectedValue != null)
            {
                favorite.Route = cRouteComboBox.SelectedValue.ToString();
                favorite.Dir = cDirectionComboBox.SelectedValue.ToString();
                favorite.Stop = cStopsComboBox.SelectedValue.ToString();

                favorite.routeComboIndex = cRouteComboBox.SelectedIndex;
                favorite.dirComboIndex = cDirectionComboBox.SelectedIndex;
                favorite.stopComboIndex = cStopsComboBox.SelectedIndex;
                int contactid = UpdateDetails(favorite.Route, favorite.Dir, favorite.Stop);
                // return the cid if this exists or null if it doesn't
                if (contactid != -1)
                {
                    favoritesListViewModel favListVM = new favoritesListViewModel();
                    favListVM.DeleteContact(contactid);
                }
            }
        }

        private void FavRouteButtonChecked(object sender, RoutedEventArgs e)
        {
            ToggleFavRouteButton.Content = "\uE1CF";
            //SolidColorBrush yellowBrush = new SolidColorBrush(Windows.UI.Colors.DarkGoldenrod);
            //ToggleFavRouteButton.Foreground = yellowBrush;
            Dto.Favorites favorite = new Dto.Favorites();
            if (cRouteComboBox.SelectedValue != null && cDirectionComboBox.SelectedValue != null && cStopsComboBox.SelectedValue != null)
            {
                favorite.Route = cRouteComboBox.SelectedValue.ToString();
                favorite.Dir = cDirectionComboBox.SelectedValue.ToString();
                favorite.Stop = cStopsComboBox.SelectedValue.ToString();

                favorite.routeComboIndex = cRouteComboBox.SelectedIndex;
                favorite.dirComboIndex = cDirectionComboBox.SelectedIndex;
                favorite.stopComboIndex = cStopsComboBox.SelectedIndex;
                int contactid = UpdateDetails(favorite.Route, favorite.Dir, favorite.Stop);

                if (contactid == -1)
                {
                    Insert(favorite);
                }
            }
        }

        public void Insert(Favorites objContact)
        {
            string sqlpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TransitFavorites.sqlite");
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), sqlpath))
            {
                conn.RunInTransaction(() =>
                {
                    conn.Insert(objContact);
                });
            }
        }

       
        public int UpdateDetails(string route, string dir, string stop)
        {
            int contactid = -1;
            string sqlpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TransitFavorites.sqlite");
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), sqlpath))
            {
                List<Favorites> myCollection = conn.Table<Favorites>().ToList<Favorites>();
                ObservableCollection<Favorites> FavoritesList = new ObservableCollection<Favorites>(myCollection);

                var existingFavorite = conn.Query<Favorites>("SELECT * from Favorites WHERE Route= \'" + route + "\' and Dir= \'" + dir + "\' and Stop= \'" + stop + "\'").FirstOrDefault();
                //var existingFavorite = conn.Query<Favorites>("select * from Favorites LIMIT).FirstOrDefault();

                if (existingFavorite != null)
                {
                    contactid = existingFavorite.Id;
                }
            }
            return contactid;
        }
    }
}

