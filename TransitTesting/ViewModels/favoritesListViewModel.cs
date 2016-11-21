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

        //Delete specific student  
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
    }
}
