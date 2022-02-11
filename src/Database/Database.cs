using FirebirdSql.Data.FirebirdClient;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;

namespace UtilityPack
{
    /// <summary>
    /// Different type of Database system
    /// </summary>
    public enum DbSystem
    {
        SQL_SERVER,
        FIREBIRD
    }

    /// <summary>
    /// Options used when parsing a returned value to another
    /// </summary>
    public class DbParsingOption
    {
        /// <summary>
        /// If true execute a trim on the obtained string
        /// </summary>
        public bool trim = false;
        /// <summary>
        /// if true, allow the function to return null is database value is null
        /// </summary>
        public bool returnNull = false;
        /// <summary>
        /// Specify the division simbol if the value is a float and must be returned as a string
        /// </summary>
        public string decimalDiv = null;
        /// <summary>
        /// Specify the number of decimal values
        /// </summary>
        public int decimalNumber = 2;
    }

    /// <summary> Abstraction class for easily connecting to an SQL database </summary>
    public class Database
    {
        
        /// <summary> Time for every command to run before calling an error (Default=0, unlimited time) </summary>
        public int CommandTimeout = 0;
        /// <summary> Type of this database </summary>
        public DbSystem system { get; private set; }
                
        private DbConnection connection;

        /// <summary> 
        /// Abstraction class for easily connecting to an SQL database 
        /// </summary>
        public Database(string database, string dataSource, string user, string password, DbSystem? _type = null)
        {
            bool isFdb = database.Contains(".FDB") || database.Contains(".fdb") || user.Contains("SYSDBA");
 
            if(_type == DbSystem.SQL_SERVER || (_type == null && isFdb == false))
            { 
                string connetionString = $@"Data Source={dataSource};User ID={user};Password={password};Database={database};";
                connection = new SqlConnection(connetionString);
                system = DbSystem.SQL_SERVER;
            }

            if(_type == DbSystem.FIREBIRD || (_type == null && isFdb == true))
            { 
                if(!database.Contains(":"))
                    database = Path.GetPathRoot(AppDomain.CurrentDomain.BaseDirectory)+"ETOS/Dati/"+database;

                string connetionString = $@"Data Source={dataSource};User ID={user};Password={password};Database={database};";
                connection = new FbConnection(connetionString);
                system = DbSystem.FIREBIRD;
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

            if(system == DbSystem.SQL_SERVER)
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
            else if(system == DbSystem.FIREBIRD)
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

            if(system == DbSystem.SQL_SERVER)
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
            else if(system == DbSystem.FIREBIRD)
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
        

        

        /// <summary>
        /// Parse a recived object from a query to a numeric value
        /// </summary>
        public static T ParseToNumber<T>(object value, T standard, DbParsingOption options = null)
        {
            if(options == null)
                options = new DbParsingOption();

            if(value != DBNull.Value)
            {
                try
                {
                    T valueConv;
                    NumberFormatInfo nfi = new NumberFormatInfo();

                    if(options.decimalDiv != null)
                        nfi.NumberDecimalSeparator = options.decimalDiv;

                    if(value is string)
                    {
                        if(options.trim)
                            value = value.ToString().Trim();  

                        value = double.Parse(value.ToString(), nfi);
                    }
                 
                    valueConv = (T)Convert.ChangeType(value, typeof(T), nfi);
                    return valueConv;
                }
                catch(Exception e)
                { Console.WriteLine(e.ToString());
                    return standard;    
                }
            }
            
            return standard;
        }


       
        /// <summary>
        /// Parse a recived object from a query to a string
        /// </summary>
        public static string ParseToString(Object value, string standard, DbParsingOption options = null)
        {
            if(options == null)
                options = new DbParsingOption();

            if(value != DBNull.Value)
            {
                string valueConv;
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalDigits = options.decimalNumber;

                if(options.decimalDiv != null)
                    nfi.NumberDecimalSeparator = options.decimalDiv;
            

                if (value is int)
                    valueConv = ((int)value).ToString("N", nfi);
                else if (value is double)
                    valueConv =  ((double)value).ToString("N", nfi);
                else if (value is float)
                    valueConv = ((float)value).ToString("N", nfi);
                else if (value is string)
                    valueConv = (string)value;
                else 
                    valueConv = (string)value;


                if(options.trim)
                    valueConv = valueConv.Trim();

                return valueConv;
            }
            else
            {
                if(options.returnNull)
                    return null;
                else
                    return standard;
            }
        }
    }
}
