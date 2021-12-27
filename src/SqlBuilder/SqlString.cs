using System;
using System.Text.RegularExpressions;

namespace UtilityPack
{
    /// <summary> Sql strings type </summary>
    public enum SqlStringType
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

    public enum DbJoinType
    {
        INNER
    }


    /// <summary> Class to easly build sql strings </summary>
    public class SqlString
    {
        private string table;
        private string command;
        private SqlStringType? commandType = null;

        /// <summary> Class to easly build sql strings </summary>
        private SqlString()
        {}

        #region    CREATE METHODS

        /// <summary> Create from zero a sqlString </summary>
        public static SqlString CreateManual(string text)
        {
            SqlString sql = new SqlString();

            sql.command     = text;
            sql.commandType = SqlStringType.MANUAL;

            return sql;
        }

        /// <summary> Create a sqlString with an INSERT template </summary>
        public static SqlString CreateInsert(string tableName)
        {
            SqlString sql = new SqlString();

            sql.commandType = SqlStringType.INSERT;
            sql.command     = $@"INSERT INTO {tableName} (@@@) values (###)";
            sql.table = tableName;

            return sql;
        }

        /// <summary> Create a sqlString with an UPDATE template </summary>
        public static SqlString CreateUpdate(string tableName)
        {
            SqlString sql = new SqlString();

            sql.commandType = SqlStringType.UPDATE;
            sql.command     = $@"UPDATE {tableName} SET @@@ = ### WHERE +++ = ---";
            sql.table = tableName;

            return sql;
        }

        /// <summary> Create a sqlString with an DELETE template </summary>
        public static SqlString CreateDelete(string tableName)
        {
            SqlString sql = new SqlString();

            sql.commandType = SqlStringType.DELETE;
            sql.command     = $@"DELETE FROM {tableName} WHERE +++ = ---";
            sql.table = tableName;

            return sql;
        }

        /// <summary> Create a sqlString with an SELECT template </summary>
        public static SqlString CreateSelect(string tableName)
        {
            SqlString sql = new SqlString();

            sql.commandType = SqlStringType.SELECT;
            sql.command     = $@"SELECT *** FROM {tableName} !!! WHERE +++ = ---";
            sql.table = tableName;

            return sql;
        }

        #endregion

        private string ConvertValue(object value)
        {
            string val = "";

            if (value is string)
                val = $"'{ ((string)value).Replace("'", "") }'";
               
            if (value is float)
                val = $"{ ((float)value).ToString().Replace(",", ".") }";
                      
            if (value is int)
                val = $"{(int)value}";

            if (value is double)
                val = $"{ ((double)value).ToString().Replace(",", ".") }";

            if (value is null)
                val = $"NULL";

            return val;
        }
  

        /// <summary> Set a select option inside the command, valid for SELECT type </summary>
        public SqlString SetSelect(string index)
        {
            if(commandType == SqlStringType.MANUAL)
                throw new Exception("Can't use SetSelect inside a SELECT SqlString");
            if(commandType == SqlStringType.INSERT)
                throw new Exception("Can't use SetSelect inside a SELECT SqlString");
            if(commandType == SqlStringType.UPDATE)
                throw new Exception("Can't use SetSelect inside a UPDATE SqlString");
            if(commandType == SqlStringType.DELETE)
                throw new Exception("Can't use SetSelect inside a DELETE SqlString");

            if(commandType == SqlStringType.SELECT)
                 command = Regex.Replace(command, @"\*\*\*", $"{index}, ***");     
            
            return this;
        }

        /// <summary> Set a parameter inside the command, valid for MANUAL and INSERT type </summary>
        public SqlString SetParam(string index, object value)
        {
            if(commandType == SqlStringType.SELECT)
                throw new Exception("Can't use SetParam inside a SELECT SqlString");
            if(commandType == SqlStringType.DELETE)
                throw new Exception("Can't use SetParam inside a DELETE SqlString");

            string val = ConvertValue(value);

            if(commandType == SqlStringType.MANUAL)
                command = Regex.Replace(command, @"\B" + index + @"\b", val);


            if(commandType == SqlStringType.INSERT)
            {
                command = Regex.Replace(command, "@@@", $"{index}, @@@");
                command = Regex.Replace(command, "###", $"{val}, ###");
            }       

            if(commandType == SqlStringType.UPDATE)
            {
                command = Regex.Replace(command, "@@@", $"{index}");
                command = Regex.Replace(command, "###", $"{val}, @@@ = ###");
            }  

            return this;
        }

        /// <summary> Set where conditions inside the command, valid for SELECT and UPDATE type </summary>
        public SqlString SetWhere(string index, object value)
        {
            if(commandType == SqlStringType.INSERT)
                throw new Exception("Can't use SetWhere inside a INSERT SqlString");
            if(commandType == SqlStringType.MANUAL)
                throw new Exception("Can't use SetWhere inside a MANUAL SqlString");

            string val = ConvertValue(value);

            if(commandType == SqlStringType.SELECT || commandType == SqlStringType.DELETE || commandType == SqlStringType.UPDATE)
            {
                command = Regex.Replace(command, @"\+\+\+", $"{index}");
                command = Regex.Replace(command, "---", $"{val} AND +++ = ---");
            }      

            return this;
        }

        /// <summary> Set a JOIN inside the command, valid for SELECT type </summary>
        public SqlString SetJoin(DbJoinType joinType, string tableName, string columnSX, string columnDX)
        {
            if(commandType == SqlStringType.INSERT)
                throw new Exception("Can't use SetJoin inside a INSERT SqlString");
            if(commandType == SqlStringType.MANUAL)
                throw new Exception("Can't use SetJoin inside a MANUAL SqlString");
            if(commandType == SqlStringType.DELETE)
                throw new Exception("Can't use SetJoin inside a DELETE SqlString");
            if(commandType == SqlStringType.UPDATE)
                throw new Exception("Can't use SetJoin inside a UPDATE SqlString");

            string join = "";

            if(joinType == DbJoinType.INNER)
                 join = "INNER";

            if(commandType == SqlStringType.SELECT)
                command = Regex.Replace(command, @"!!!", $"{join} JOIN {tableName} ON {table}.{columnSX} = {tableName}.{columnDX} !!!");
        
            return this;
        }


        /// <summary> Returns the command clean of all temporary constructs and ready to be inserted into a database </summary>
        public string GetCommand()
        {
            string finalCommand = command;
            
            if(commandType == SqlStringType.INSERT)
            {
                finalCommand = Regex.Replace(finalCommand, ", ###", "");
                finalCommand = Regex.Replace(finalCommand, ", @@@", "");

                finalCommand = Regex.Replace(finalCommand, "###", "");
                finalCommand = Regex.Replace(finalCommand, "@@@", "");
            }    

            if(commandType == SqlStringType.UPDATE)
            {
                if(finalCommand.Contains(" SET @@@ = ###"))
                    throw new Exception("sqlString of type UPDATE dosen't include any SET values, use SetParam before calling GetCommand");

                finalCommand = Regex.Replace(finalCommand, ", @@@ = ###", "");
            }   

            if(commandType == SqlStringType.SELECT)
            {
                if(finalCommand.Contains(", ***"))
                    finalCommand = Regex.Replace(finalCommand, @", \*\*\*", "");
                else
                    finalCommand = Regex.Replace(finalCommand, @"\*\*\*", "*");

                finalCommand = Regex.Replace(finalCommand, @"!!!", "");
            }

            if(commandType == SqlStringType.SELECT || commandType == SqlStringType.UPDATE || commandType == SqlStringType.DELETE)
            {
                finalCommand = Regex.Replace(finalCommand, @"WHERE \+\+\+ = ---", "");
                finalCommand = Regex.Replace(finalCommand, @" AND \+\+\+ = ---", "");
            }

            return finalCommand;
        }
    }
}
