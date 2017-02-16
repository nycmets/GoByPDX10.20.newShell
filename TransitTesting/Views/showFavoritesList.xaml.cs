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
using GoByPDX.ViewModels;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using GoByPDX.Dto;
using Windows.Storage;
using Windows.UI.Popups;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Core;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GoByPDX.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class showFavoritesList : Page
    {


        public static favoritesListViewModel favoritesListViewModel = new favoritesListViewModel();

        public showFavoritesList()
        {
            this.InitializeComponent();
           // this.DataContext = favoritesListViewModel;
        }

       
        private async void favoriteSelected(object sender, SelectionChangedEventArgs e)
        {
            Favorites favSelected = LV1.SelectedItem as Favorites;
            var contactid = favSelected.Id;

            if (favSelected != null)
            {
                favoritesListViewModel.populateFavIndexes(contactid);
                loadingBar.IsEnabled = true;
                loadingBar.Visibility = Visibility.Visible;
                loadingDarkness.Visibility = Visibility.Visible;
                await Task.Delay(100);
                this.Frame.Navigate(typeof(showRouteList));
               
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);             //not needed, base method is empty and does nothing
            DataContext = null;                //important part, whenever you navigate, refreshes the    ViewModel - no deletion, just resetting of the DataContext, so the page won't get stuck           
            var test = favoritesListViewModel.populateSqlCollection();
            this.LV1.ItemsSource = favoritesListViewModel.bindingFavListItems;
        }

        private void favorite_holding(object sender, HoldingRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private void deleteFavorite(object sender, TappedRoutedEventArgs e)
        {
            MenuFlyoutItem fly = sender as MenuFlyoutItem;
            Favorites favSelected = fly.DataContext as Favorites;
           
            int contactid = favSelected.Id;
            if (favSelected != null)
            {
                favoritesListViewModel.DeleteContact(contactid);
                this.LV1.ItemsSource = favoritesListViewModel.bindingFavListItems;
            }
        }
    }
}
