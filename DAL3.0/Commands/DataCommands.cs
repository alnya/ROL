using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Diagnostics;
using System.Text;

namespace DAL
{
	///////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for dataCommands.
	/// </summary>
	///////////////////////////////////////////////////////////////////////
    public class DataCommands
    {
        #region Variables
        ConnectionSettings settings;
        DbCommand cmd;
        DbDataAdapter adapter;
        DbDataReader reader;
        #endregion

        #region Constructor
        ///////////////////////////////////////////////////////////////////////
		/// <summary>
		///  Create a new data Commands object, depending on database type 
		///  (sql / mysql)
		/// </summary>
		///////////////////////////////////////////////////////////////////////
		public DataCommands (ConnectionSettings conn)
		{
            settings = conn;

            cmd = settings.Factory.CreateCommand();
            cmd.Connection = settings.DBConnection;

            adapter = settings.Factory.CreateDataAdapter();
        }
        #endregion

        #region Load
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Load a specific row
        /// </summary>
        /// <param name="table">Database table</param>
        /// <param name="ID">ID of the row</param>
        /// <returns>loaded</returns>
        ///////////////////////////////////////////////////////////////////////
        public bool Load(string table, string cols, int ID, out DataSet data, out string Message)
        {
            StringBuilder sql = new StringBuilder();
            Message = string.Empty;
            DataSet dataSet = new DataSet();
            data = dataSet;

            sql.Append("SELECT ID,");
            sql.Append(cols);
            sql.Append(" FROM ");
            sql.Append(table);
            sql.Append(" WHERE ID = ");
            sql.Append(ID);

            try
            {
                cmd.CommandText = sql.ToString();
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                if (settings.TransactionsEnabled)
                    cmd.Transaction = settings.Transaction;

                adapter.SelectCommand = cmd;
                adapter.Fill(data);

                return true;
            }
            catch (Exception e)
            {
                // Error
                Message = "There was an error loading this record (ID: " + ID + ") - " + e.Message;
                return false;
            }
        }
        #endregion

        #region Insert
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Insert a record
        /// </summary>
        /// <param name="table">Database Table</param>
        /// <param name="items">Collection of columns</param>
        /// <param name="ID">ID of the row</param>
        /// <returns>inserted</returns>
        ///////////////////////////////////////////////////////////////////////
        public bool Insert(string table, NameValueCollection items, out int id, out string Message)
        {
            StringBuilder sql = new StringBuilder();
            Message = string.Empty;
            id = 0;

            sql.Append("INSERT INTO ");
            sql.Append(table);
            sql.Append(" (");

            // get columns
            foreach (string key in items.AllKeys)
            {
                sql.Append(key);
                sql.Append(",");
            }

            // remove the last pasky coma
            sql.Remove(sql.Length - 1, 1);
            sql.Append(") VALUES (");

            // get values
            foreach (string key in items.AllKeys)
            {
                sql.Append(items.Get(key));
                sql.Append(",");
            }

            // remove the last pasky coma
            sql.Remove(sql.Length - 1, 1);
            sql.Append(")");

            try
            {
                cmd.CommandText = sql.ToString();
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                if (settings.TransactionsEnabled)
                    cmd.Transaction = settings.Transaction;

                int success = cmd.ExecuteNonQuery();
                // if succesful, return the ID
                sql.Length = 0;
                sql.Append("SELECT ID FROM ");
                sql.Append(table);
                sql.Append(" ORDER BY ID DESC");

                cmd.CommandText = sql.ToString();
                reader = cmd.ExecuteReader();
                while (reader.Read() && id == 0)
                    id = int.Parse(reader["ID"].ToString());
                return true;
            }
            catch (Exception e)
            {
                // Error
                Message = "There was an error inserting this record - " + e.Message;
                return false;
            }
        }
        #endregion

        #region Update
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Update an existing row
        /// </summary>
        /// <param name="table">Database Table</param>
        /// <param name="items">Collection of Columns</param>
        /// <param name="ID">IF of the row</param>
        /// <returns>updated</returns>
        ///////////////////////////////////////////////////////////////////////
        public bool Update(string table, NameValueCollection items, int ID, out string Message)
        {
            StringBuilder sql = new StringBuilder();
            Message = string.Empty;

            sql.Append("UPDATE ");
            sql.Append(table);
            sql.Append(" SET ");

            // get columns and values
            foreach (string key in items.AllKeys)
            {
                sql.Append(key);
                sql.Append(" = ");
                sql.Append(items.Get(key));
                sql.Append(",");
            }

            // remove the last pasky coma
            sql.Remove(sql.Length - 1, 1);
            sql.Append(" WHERE ID=");
            sql.Append(ID);

            try
            {
                cmd.CommandText = sql.ToString();
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                if (settings.TransactionsEnabled)
                    cmd.Transaction = settings.Transaction;

                int success = cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                // Error
                Message = "There was an error updating this record (ID: " + ID + ") - " + e.Message;
                return false;
            }
        }
        #endregion

        #region Delete
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Delete an existing row
        /// </summary>
        /// <param name="table">Database table</param>
        /// <param name="ID">ID of the row</param>
        /// <returns>deleted</returns>
        ///////////////////////////////////////////////////////////////////////
        public bool Delete(string table, int ID, out string Message)
        {
            StringBuilder sql = new StringBuilder();
            Message = string.Empty;

            sql.Append("DELETE FROM ");
            sql.Append(table);
            sql.Append(" WHERE ID = ");
            sql.Append(ID);

            try
            {
                cmd.CommandText = sql.ToString();
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                if (settings.TransactionsEnabled)
                    cmd.Transaction = settings.Transaction;

                int success = cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                // Error
                Message = "There was an error deleting this record (ID: " + ID + ") - " + e.Message;
                return false;
            }
        }
        #endregion

        #region Delete All
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Delete all
        /// </summary>
        /// <param name="table">Database table</param>
        /// <param name="where">where</param>
        /// <returns>deleted</returns>
        ///////////////////////////////////////////////////////////////////////
        public bool DeleteAll(string table, string where, out string Message)
        {
            StringBuilder sql = new StringBuilder();
            Message = string.Empty;

            sql.Append("DELETE FROM ");
            sql.Append(table);
            sql.Append(" WHERE ");
            sql.Append(where);

            try
            {
                cmd.CommandText = sql.ToString();
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                if (settings.TransactionsEnabled)
                    cmd.Transaction = settings.Transaction;

                int success = cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                // Error
                Message = "There was an error - " + e.Message;
                return false;
            }
        }
        #endregion

        #region Search
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Seach a table
        /// </summary>
        /// <param name="table">Database table</param>
        /// <param name="where">where creteria</param>
        /// <param name="orderBy">order by criteria (eg "ID ASC")</param>
        /// <param name="list">list of results</param>
        /// <returns>search successful</returns>
        ///////////////////////////////////////////////////////////////////////
        public bool Search(string table, string cols, string where, string orderBy, out DataSet data, out string Message)
        {
            StringBuilder sql = new StringBuilder();
            Message = string.Empty;
            data = new DataSet();

            sql.Append("SELECT ID,");
            sql.Append(cols);
            sql.Append(" FROM ");
            sql.Append(table);

            if (where != string.Empty)
            {
                sql.Append(" WHERE ");
                sql.Append(where);
            }

            if (orderBy != String.Empty)
            {
                sql.Append(" ORDER BY ");
                sql.Append(orderBy);
            }

            return FillDataSet(sql.ToString(), out data, out Message);
        }
        #endregion

        #region Execute Stored Procedure
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Execute a stored procedure
        /// </summary>
        /// <param name="procedure">Name of the procedure</param>
        /// <param name="where">where creteria</param>
        /// <param name="orderBy">order by criteria (eg "ID ASC")</param>
        /// <param name="data">list of results</param>
        /// <returns>search successful?</returns>
        ///////////////////////////////////////////////////////////////////////
        public bool ExecuteProcedure(string procedureName, List<DbParameter> paramaters, out DataSet data, out string Message)
        {
            Message = string.Empty;
            data = new DataSet();

            try
            {
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                foreach(DbParameter param in paramaters)
                    cmd.Parameters.Add(param);

                adapter.SelectCommand = cmd;
                adapter.Fill(data);
                return true;
            }
            catch (Exception e)
            {
                // Error
                Message = "There was an error executing the procedure " + procedureName + " - " + e.Message;
                return false;
            }

        }

        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Execute a stored procedure
        /// </summary>
        /// <param name="procedure">Name of the procedure</param>
        /// <param name="where">where creteria</param>
        /// <param name="orderBy">order by criteria (eg "ID ASC")</param>
        /// <returns>search successful?</returns>
        ///////////////////////////////////////////////////////////////////////
        public bool ExecuteProcedure(string procedureName, ref List<DbParameter> paramaters, out string Message)
        {
            Message = string.Empty;

            try
            {
                cmd.CommandText = procedureName;
                cmd.CommandType = CommandType.StoredProcedure;

                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                foreach (DbParameter param in paramaters)
                    cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                // Error
                Message = "There was an error executing the procedure " + procedureName + " - " + e.Message;
                return false;
            }

        }
        #endregion

        #region Fill a dataset with a multi-table complex search
        ///////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Perform a complex search
        /// </summary>
        /// <param name="where">where creteria</param>
        /// <returns>search successful</returns>
        ///////////////////////////////////////////////////////////////////////
        public bool FillDataSet(string where, out DataSet data, out string Message)
        {
            Message = string.Empty;
            data = new DataSet();

            try
            {
                cmd.CommandText = where;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                adapter.SelectCommand = cmd;
                adapter.Fill(data);
                return true;
            }
            catch (Exception e)
            {
                // Error
                Message = "There was an error in the search - " + e.Message;
                return false;
            }
        }
        #endregion
    }
}
