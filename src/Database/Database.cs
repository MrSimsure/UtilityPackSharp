using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;

namespace UtilityPack
{
    public enum DbType 
    {
        SQL_SERVER,
        FIREBIRD
    }

    /// <summary> Abstraction class for easily connecting to an SQL database </summary>
    public class Database
    {

        /// <summary> Time for every command to run before calling an error (Default=0, unlimited time) </summary>
        public int CommandTimeout = 0;
        /// <summary> Type of this database </summary>
        public DbType type { get; private set; }
                
        private DbConnection connection;

        /// <summary> 
        /// Abstraction class for easily connecting to an SQL database 
        /// </summary>
        public Database(string database, string dataSource, string user, string password, DbType? _type = null)
        {
            bool isFdb = database.Contains(".FDB") || database.Contains(".fdb");
 
            if(_type == DbType.SQL_SERVER || (_type == null && isFdb == false))
            { 
                string connetionString = $@"Data Source={dataSource};User ID={user};Password={password};Database={database};";
                connection = new SqlConnection(connetionString);
                type = DbType.SQL_SERVER;
            }

            if(_type == DbType.FIREBIRD || (_type == null && isFdb == true))
            { 
                if(!database.Contains(":"))
                    database = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory)+"ETOS/Dati/"+database;

                string connetionString = $@"Data Source={dataSource};User ID={user};Password={password};Database={database};";
                connection = new FbConnection(connetionString);
                type = DbType.FIREBIRD;
            }     
        }

        /// <summary> Change the connected database </summary>
        /// <exception cref="SqlException"></exception>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public void ChangeDatabase(string name)
        {
            connection.ChangeDatabase(name);
        }

        /// <summary> 
        /// Test the connection 
        /// </summary>
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
            catch(Exception e)
            {
                if(print)
                { 
                    Console.WriteLine("Connection Failed\n");
                    Console.WriteLine(e.ToString());
                }

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

            if(type == DbType.SQL_SERVER)
            { 
                using(SqlCommand command = new SqlCommand(sql, (SqlConnection)connection))
                {
                    command.CommandTimeout = CommandTimeout;
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        DataTable data = new DataTable();

                        data.Load(reader);

                        connection.Close();

                        return data;
                    }
                };
            }
            else if(type == DbType.FIREBIRD)
            { 
                using(FbCommand command = new FbCommand(sql, (FbConnection)connection))
                {
                    command.CommandTimeout = CommandTimeout;
                    using(FbDataReader reader = command.ExecuteReader())
                    {
                        DataTable data = new DataTable();

                        data.Load(reader);

                        connection.Close();

                        return data;
                    }
                };
            }
            else
            {
                return null;
            }
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

            if(type == DbType.SQL_SERVER)
            { 
                using(SqlCommand command = new SqlCommand(sql, (SqlConnection)connection))
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
            else if(type == DbType.FIREBIRD)
            { 
                using(FbCommand command = new FbCommand(sql, (FbConnection)connection))
                {
                    using(FbDataAdapter adapter = new FbDataAdapter())
                    {
                        adapter.InsertCommand = command;
                        int changed = adapter.InsertCommand.ExecuteNonQuery();

                        connection.Close();

                        return changed;
                    }
                }   
            }
            else
            {
                return 0;
            }
        }
    }
}
