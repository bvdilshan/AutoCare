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
            "server=localhost;database=autocaredb;uid=root;pwd=1234;";
        public static MySqlConnection GetConnection()

        {
            return new MySqlConnection(connectionString);
        }
    }
}