using Dapper;
using Dapper.Contrib.Extensions;
using IpGeolocation.Database.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpGeolocation.Database.Dao
{
    public class GeolocationDao
    {
        private static readonly  ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        internal static readonly string conectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString.Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);
        private static T DatabaseAction<T>(Func<IDbConnection, T> databaseAction)
        {
            SQLiteConnection connection = null;
            try
            {
                connection = new SQLiteConnection(conectionString);
                connection.Open();
                return databaseAction(connection);
            }
            catch (Exception e)
            {
                log.Error(e);
                throw;
            }
            finally
            {
                connection?.Close();
            }
        }
        private static Geolocation Get(string sql)
        {
            return DatabaseAction<Geolocation>(c => c.QueryFirstOrDefault<Geolocation>(sql));
        }
        public static Geolocation GetGeolocationByIpOrHost(string ipOrHost)
        {
            return GetGeolocationByIp(ipOrHost) ?? GetGeolocationByHost(ipOrHost);
        }

        public static Geolocation GetGeolocationByIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return null;
            return Get($"SELECT * FROM Geolocations WHERE Ip = '{ip}'");
        }
        public static Geolocation GetGeolocationByHost(string host)
        {
            if (string.IsNullOrEmpty(host))
                return null;
            return Get($"SELECT * FROM Geolocations WHERE Host = '{host}'");
        }
        public static bool Insert(Geolocation geolocation)
        {
            return DatabaseAction(c => c.Insert(geolocation) != 0);
        }
        public static bool DeleteByIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return false;
            return DatabaseAction(c => c.Execute($"DELETE FROM Geolocations WHERE Ip = '{ip}'") != 0);
        }
        public static bool DeleteByHost(string host)
        {
            if (string.IsNullOrEmpty(host))
                return false;
            return DatabaseAction(c => c.Execute($"DELETE FROM Geolocations WHERE Host = '{host}'") != 0);
        }
        public static bool DeleteByIpOrHost(string ipOrHost)
        {
            return DeleteByIp(ipOrHost) || DeleteByHost(ipOrHost);
        }
    }
}
