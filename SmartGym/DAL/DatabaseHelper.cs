using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SmartGym.DAL
{
    /// <summary>
    /// Low-level ADO.NET helper. Centralises connection management and
    /// exposes ExecuteNonQuery / ExecuteScalar / ExecuteReader / FillDataSet
    /// to demonstrate SqlConnection, SqlCommand, SqlDataReader, SqlDataAdapter
    /// and DataSet usage in a single place.
    /// </summary>
    public static class DatabaseHelper
    {
        /// <summary>Reads the connection string from Web.config.</summary>
        public static string ConnectionString =>
            ConfigurationManager.ConnectionStrings["SmartGymDB"].ConnectionString;

        /// <summary>Creates a new open SqlConnection.</summary>
        public static SqlConnection GetOpenConnection()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Executes an INSERT / UPDATE / DELETE statement.
        /// Returns the number of rows affected.
        /// </summary>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = GetOpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a query returning a single scalar value (e.g. COUNT or newly-inserted ID).
        /// </summary>
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = GetOpenConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);
                return command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Returns a DataTable populated via a SqlDataAdapter.
        /// </summary>
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
        {
            DataTable table = new DataTable();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(table);
                }
            }
            return table;
        }

        /// <summary>
        /// Returns a fully populated DataSet (multiple tables) - useful for XML export.
        /// </summary>
        public static DataSet ExecuteDataSet(string sql, string tableName = "Table1",
            params SqlParameter[] parameters)
        {
            DataSet dataSet = new DataSet("SmartGymData");
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                if (parameters != null)
                    command.Parameters.AddRange(parameters);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataSet, tableName);
                }
            }
            return dataSet;
        }
    }
}
