using GoByPDX.Dto;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace GoByPDX.ViewModels
{
    public class favoritesListViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Favorites> bindingFavListItems = new ObservableCollection<Favorites>();

        public ObservableCollection<Favorites> favList { get { return bindingFavListItems; } set { bindingFavListItems = value; OnPropertyChanged("bindingFavListItems"); } }


        public void populateFavIndexes(int contactid)
        {
            string sqlpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TransitFavorites.sqlite");
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), sqlpath))
            {
                var existingFavorite = conn.Query<Favorites>("select * from Favorites where Id= " + contactid).FirstOrDefault();
                App.routeListViewModel.routeComboIndex = existingFavorite.routeComboIndex;
                App.routeListViewModel.dirComboIndex = existingFavorite.dirComboIndex;
                App.routeListViewModel.stopComboIndex = existingFavorite.stopComboIndex;
            }
        }

        public List<Favorites> populateSqlCollection()
        {
            List<Favorites> myCollection = new List<Favorites>();

            string sqlpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TransitFavorites.sqlite");
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), sqlpath))
            {
                myCollection = conn.Table<Favorites>().ToList<Favorites>();
                var query = conn.Table<Favorites>();
                favList = new ObservableCollection<Favorites>(query.ToList());
            }
            return myCollection;
        }

        public void DeleteContact(int contactid)
        {
            string sqlpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TransitFavorites.sqlite");
            using (SQLiteConnection conn = new SQLiteConnection(new SQLitePlatformWinRT(), sqlpath))
            {
                var existingFavorite = conn.Query<Favorites>("select * from Favorites where Id= " + contactid).FirstOrDefault();
                if (existingFavorite != null)
                {
                    conn.RunInTransaction(() =>
                    {
                        conn.Delete(existingFavorite);
                    });
                    var query = conn.Table<Favorites>();
                    favList = new ObservableCollection<Favorites>(query.ToList());
                }
            }
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

        public void insertFav(ComboBox routeComboBox, ComboBox directionComboBox, ComboBox stopsComboBox)
        { 
            Dto.Favorites favorite = new Dto.Favorites();
            ////if (cRouteComboBox.SelectedValue != null && cDirectionComboBox.SelectedValue != null && cStopsComboBox.SelectedValue != null)
            ////{
            favorite.Route = routeComboBox.SelectedValue.ToString();
            favorite.Dir = directionComboBox.SelectedValue.ToString();
            favorite.Stop = stopsComboBox.SelectedValue.ToString();

            favorite.routeComboIndex = routeComboBox.SelectedIndex;
            favorite.dirComboIndex = directionComboBox.SelectedIndex;
            favorite.stopComboIndex = stopsComboBox.SelectedIndex;
            int contactid = UpdateDetails(favorite.Route, favorite.Dir, favorite.Stop);

            if (contactid == -1)
            {
                Insert(favorite);
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

        public int updateFavoriteDB(ComboBox routeComboBox, ComboBox directionComboBox, ComboBox stopsComboBox)
        {
            Dto.Favorites favorite = new Dto.Favorites();
            //if (cRouteComboBox.SelectedValue != null && cDirectionComboBox.SelectedValue != null && cStopsComboBox.SelectedValue != null)
            //{
            favorite.Route = routeComboBox.SelectedValue.ToString();
            favorite.Dir = directionComboBox.SelectedValue.ToString();
            favorite.Stop = stopsComboBox.SelectedValue.ToString();

            favorite.routeComboIndex = routeComboBox.SelectedIndex;
            favorite.dirComboIndex = directionComboBox.SelectedIndex;
            favorite.stopComboIndex = stopsComboBox.SelectedIndex;
            int contactid = UpdateDetails(favorite.Route, favorite.Dir, favorite.Stop);
            // return the cid if this exists or null if it doesn't        }
            return contactid;
        }
    }
}
