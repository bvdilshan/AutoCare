using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace VehicleServiceSystem.Data
{
    internal class DbConnection
    {
        // Connection string to connect to the MySQL database
        private static string connectionString =
            "server=localhost;database=VEHICLESERVICEDB;uid=root;pwd=1234;";
        // Method to get a new MySqlConnection object
        public static MySqlConnection GetConnection()

        {
            return new MySqlConnection(connectionString);
        }
    }
}