using System;
using System.Data;
using System.IO;
using System.Data.SqlClient;

namespace UtilityPack
{
    /// <summary> Abstraction class for easily connecting to an SQL database </summary>
    public class Database
    {
        private SqlConnection connection;

        /// <summary> Abstraction class for easily connecting to an SQL database </summary>
        public Database(string database, string dataSource, string user, string password)
        {
            string connetionString = $@"Data Source={dataSource};User ID={user};Password={password};Database={database};";
            connection = new SqlConnection(connetionString);
        }

        /// <summary> Change the connected database </summary>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void ChangeDatabase(string name)
        {
            connection.ChangeDatabase(name);
        }

        /// <summary> Test the connection </summary>
        public bool TestConnection(bool print)
        {
            try
            {
                connection.Open();

                if(print)
                    Console.WriteLine("Connection Open!");

                connection.Close();

                return true;
            }
            catch
            {
                return false;
            } 
        }

        /// <summary> Execute a query on the database and return the result in form of a <see cref="DataTable"/> </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public DataTable ExecuteSqlQuery(string sql)
        {
            connection.Open();

            using(SqlCommand command = new SqlCommand(sql, connection))
            {
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable data = new DataTable();

                    data.Load(reader);

                    connection.Close();

                    return data;
                }
            };
            
        }

        /// <summary> Execute a command on the database </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public int ExecuteSqlCommand(string sql)
        {
            connection.Open();

            using(SqlCommand command = new SqlCommand(sql, connection))
            {
                using(SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    adapter.InsertCommand = command;
                    int changed = adapter.InsertCommand.ExecuteNonQuery();

                    connection.Close();

                    return changed;
                }
            }   
        }
    }
}
