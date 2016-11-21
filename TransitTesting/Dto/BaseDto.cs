using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using System.IO;

namespace GoByPDX.Dto
{
    public class BaseDto
    {
        private string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TransitFavorites.sqlite");
            //using (SQLiteConnection conn = new SQLiteConnection(new sq))
            //string sqliteString = "Data Source =" + path + ";Version=3;";
        protected SQLiteConnection getDBConnection()
        {
            return new SQLiteConnection(new SQLitePlatformWinRT(), dbPath);
        }
           
    }
}
