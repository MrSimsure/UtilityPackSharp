using System;
using System.Text.RegularExpressions;

namespace UtilityPack.SqlBuilder
{
    /// <summary> Sql strings type </summary>
    public enum SqlFactoryType
    {
        /// <summary> total manual command </summary>
        MANUAL,
        /// <summary>template for: <br/>INSERT 'table_name' (...) values (...)</summary>
        INSERT,
        /// <summary>template fo:r <br/>SELECT ... FROM 'table_name' WHERE ... = ... </summary>
        SELECT,
        /// <summary>template for: <br/>UPDATE 'table_name' SET ... = ... WHERE ... = ... </summary>
        UPDATE,
        /// <summary>template for: <br/>DELETE FROM 'table_name' WHERE ... = ... </summary>
        DELETE
    }

    public enum SqlFactoryParam
    {
        NULL,
        PLUS,
        MINUS
    }

    public enum SqlFactoryJoin
    {
        INNER
    }


    /// <summary> Class to easly build sql strings </summary>
    public class SqlFactory
    {
        private string table;
        private string command;
        private SqlFactoryType? commandType = null;

        /// <summary> Class to easly build sql strings </summary>
        private SqlFactory()
        {}

        #region    CREATE METHODS

        /// <summary> Create from zero a SqlFactory </summary>
        public static SqlFactory CreateManual(string text)
        {
            SqlFactory sql = new SqlFactory();

            sql.command     = text;
            sql.commandType = SqlFactoryType.MANUAL;

            return sql;
        }

        /// <summary> Create a SqlFactory with an INSERT template </summary>
        public static SqlFactory CreateInsert(string tableName)
        {
            SqlFactory sql = new SqlFactory();

            sql.commandType = SqlFactoryType.INSERT;
            sql.command     = $@"INSERT INTO {tableName} (@@@) values (###)";
            sql.table = tableName;

            return sql;
        }

        /// <summary> Create a SqlFactory with an UPDATE template </summary>
        public static SqlFactory CreateUpdate(string tableName)
        {
            SqlFactory sql = new SqlFactory();

            sql.commandType = SqlFactoryType.UPDATE;
            sql.command     = $@"UPDATE {tableName} SET @@@ = ### WHERE +++ = ---";
            sql.table = tableName;

            return sql;
        }

        /// <summary> Create a SqlFactory with an DELETE template </summary>
        public static SqlFactory CreateDelete(string tableName)
        {
            SqlFactory sql = new SqlFactory();

            sql.commandType = SqlFactoryType.DELETE;
            sql.command     = $@"DELETE FROM {tableName} WHERE +++ = ---";
            sql.table = tableName;

            return sql;
        }

        /// <summary> Create a SqlFactory with an SELECT template </summary>
        public static SqlFactory CreateSelect(string tableName)
        {
            SqlFactory sql = new SqlFactory();

            sql.commandType = SqlFactoryType.SELECT;
            sql.command     = $@"SELECT *** FROM {tableName} !!! WHERE +++ = ---";
            sql.table = tableName;

            return sql;
        }

        #endregion

        private string ConvertValue(object value)
        {
            string val = "";

            if(value is string)
                val = $"'{ ((string)value).Replace("'", "") }'";
               
            if(value is float)
                val = $"{ ((float)value).ToString().Replace(",", ".") }";
                      
            if(value is int)
                val = $"{(int)value}";

            if(value is double)
                val = $"{ ((double)value).ToString().Replace(",", ".") }";

            if(value is null)
                val = $"NULL";

            return val;
        }
  

        /// <summary> Set a select option inside the command, valid for SELECT type </summary>
        public SqlFactory SetSelect(string index)
        {
            if(commandType == SqlFactoryType.MANUAL)
                throw new Exception("Can't use SetSelect inside a SELECT SqlString");
            if(commandType == SqlFactoryType.INSERT)
                throw new Exception("Can't use SetSelect inside a SELECT SqlString");
            if(commandType == SqlFactoryType.UPDATE)
                throw new Exception("Can't use SetSelect inside a UPDATE SqlString");
            if(commandType == SqlFactoryType.DELETE)
                throw new Exception("Can't use SetSelect inside a DELETE SqlString");

            if(commandType == SqlFactoryType.SELECT)
                 command = Regex.Replace(command, @"\*\*\*", $"{index}, ***");     
            
            return this;
        }

        /// <summary> Set a parameter inside the command, valid for MANUAL, INSERT and UPDATE type </summary>
        public SqlFactory SetParam(string index, object value, SqlFactoryParam type = SqlFactoryParam.NULL)
        {
            if(commandType == SqlFactoryType.SELECT)
                throw new Exception("Can't use SetParam inside a SELECT SqlString");
            if(commandType == SqlFactoryType.DELETE)
                throw new Exception("Can't use SetParam inside a DELETE SqlString");

            string val = ConvertValue(value);
            if (type == SqlFactoryParam.MINUS)
                val = index + "-" + val;

            if (type == SqlFactoryParam.PLUS)
                val = index + "+" + val;


            if(commandType == SqlFactoryType.MANUAL)
                command = Regex.Replace(command, @"\B" + index + @"\b", val);


            if(commandType == SqlFactoryType.INSERT)
            {
                command = Regex.Replace(command, "@@@", $"{index}, @@@");
                command = Regex.Replace(command, "###", $"{val}, ###");
            }       

            if(commandType == SqlFactoryType.UPDATE)
            {
                command = Regex.Replace(command, "@@@", $"{index}");
                command = Regex.Replace(command, "###", $"{val}, @@@ = ###");
            }  

            return this;
        }
        

        /// <summary> Set where conditions inside the command, valid for SELECT, UPDATE and DELETE type </summary>
        public SqlFactory SetWhere(string index, object value)
        {
            if(commandType == SqlFactoryType.INSERT)
                throw new Exception("Can't use SetWhere inside a INSERT SqlString");
            if(commandType == SqlFactoryType.MANUAL)
                throw new Exception("Can't use SetWhere inside a MANUAL SqlString");

            string val = ConvertValue(value);

            if(commandType == SqlFactoryType.SELECT || commandType == SqlFactoryType.DELETE || commandType == SqlFactoryType.UPDATE)
            {
                command = Regex.Replace(command, @"\+\+\+", $"{index}");
                command = Regex.Replace(command, "---", $"{val} AND +++ = ---");
            }      

            return this;
        }

        /// <summary> Set a JOIN inside the command, valid for SELECT type </summary>
        public SqlFactory SetJoin(SqlFactoryJoin joinType, string tableName, string columnSX, string columnDX)
        {
            if(commandType == SqlFactoryType.INSERT)
                throw new Exception("Can't use SetJoin inside a INSERT SqlString");
            if(commandType == SqlFactoryType.MANUAL)
                throw new Exception("Can't use SetJoin inside a MANUAL SqlString");
            if(commandType == SqlFactoryType.DELETE)
                throw new Exception("Can't use SetJoin inside a DELETE SqlString");
            if(commandType == SqlFactoryType.UPDATE)
                throw new Exception("Can't use SetJoin inside a UPDATE SqlString");

            string join = "";

            if(joinType == SqlFactoryJoin.INNER)
                 join = "INNER";

            if(commandType == SqlFactoryType.SELECT)
                command = Regex.Replace(command, @"!!!", $"{join} JOIN {tableName} ON {table}.{columnSX} = {tableName}.{columnDX} !!!");
        
            return this;
        }


        /// <summary> Returns the command clean of all temporary constructs and ready to be inserted into a database </summary>
        public string GetCommand()
        {
            string finalCommand = command;
            
            if(commandType == SqlFactoryType.INSERT)
            {
                finalCommand = Regex.Replace(finalCommand, ", ###", "");
                finalCommand = Regex.Replace(finalCommand, ", @@@", "");

                finalCommand = Regex.Replace(finalCommand, "###", "");
                finalCommand = Regex.Replace(finalCommand, "@@@", "");
            }    

            if(commandType == SqlFactoryType.UPDATE)
            {
                if(finalCommand.Contains(" SET @@@ = ###"))
                    throw new Exception("sqlString of type UPDATE dosen't include any SET values, use SetParam before calling GetCommand");

                finalCommand = Regex.Replace(finalCommand, ", @@@ = ###", "");
            }   

            if(commandType == SqlFactoryType.SELECT)
            {
                if(finalCommand.Contains(", ***"))
                    finalCommand = Regex.Replace(finalCommand, @", \*\*\*", "");
                else
                    finalCommand = Regex.Replace(finalCommand, @"\*\*\*", "*");

                finalCommand = Regex.Replace(finalCommand, @"!!!", "");
            }

            if(commandType == SqlFactoryType.SELECT || commandType == SqlFactoryType.UPDATE || commandType == SqlFactoryType.DELETE)
            {
                finalCommand = Regex.Replace(finalCommand, @"WHERE \+\+\+ = ---", "");
                finalCommand = Regex.Replace(finalCommand, @" AND \+\+\+ = ---", "");
            }

            return finalCommand;
        }
    }
}
