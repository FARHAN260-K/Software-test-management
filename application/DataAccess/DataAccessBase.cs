using System;
using MySql.Data.MySqlClient;

namespace SoftwareTestManager.Application.DataAccess
{
    public class DataAccessBase
    {
        private static readonly string connectionString = "Server=localhost;Database=test_management_db2;User ID=root;Password=;";

        protected static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static void ExecuteWithConnection(Action<MySqlConnection> action)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                action(connection);
            }
        }

        public static T ExecuteWithConnection<T>(Func<MySqlConnection, T> action)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                var result = action(connection);
                connection.Close();
                return result;
            }
        }
    }
} 