using System;
using System.Collections;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Oracle.DataAccess.Client;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Functions for getting database stuff
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class DBFunctions
	{
		string _connectionString;
		string _connectionType;

		#region Constructor
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Create Database Functions object
		/// </summary>
		/// <param name="oledb">is OLE?</param>
		/// <param name="server">server path</param>
		/// <param name="username">username</param>
		/// <param name="password">Password</param>
		/////////////////////////////////////////////////////////////////
		public DBFunctions(Connection conn)
		{
            if (conn.Type == "SQL Server")
			{
                _connectionType = "Oledb";
				_connectionString = "server="+
					conn.Server + ";uid=" + 
					conn.Username + ";pwd=" +
					conn.Password;
			}
            else if (conn.Type == "MySQL")
			{
                _connectionType = "ODBC";
				_connectionString = "DRIVER={MySQL ODBC 3.51 Driver};SERVER=" + 
					conn.Server + ";UID=" + 
					conn.Username + ";PASSWORD=" +
					conn.Password + ";OPTION=3;port=" + 
					conn.Port;
			}
            else if (conn.Type == "Oracle")
            {
                _connectionType = "ODP";
                _connectionString = "Data Source=" +
                    conn.Server + ";User Id=" +
                    conn.Username + ";Password=" +
                    conn.Password;
            }
		}
		#endregion

		#region Get Databases from the connection string
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Return databases on that server
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public ArrayList GetDatabases(out string errorMessage)
		{
			ArrayList output = new ArrayList();
			errorMessage = string.Empty;

            switch (_connectionType)
            {
                case "Oledb":

                    SqlConnection sqlConn = new SqlConnection(_connectionString);

                    try
                    {
                        sqlConn.Open();

                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = sqlConn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_databases";

                        SqlDataReader reader;
                        reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string table = reader.GetString(0);
                            output.Add(table);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                    finally
                    {
                        sqlConn.Close();
                    }
                    break;

                case "ODBC":
                    OdbcConnection odbcConn = new OdbcConnection(_connectionString);

                    try
                    {
                        odbcConn.Open();

                        OdbcCommand cmd = new OdbcCommand();
                        cmd.Connection = odbcConn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "show databases";

                        OdbcDataReader dataRdr;
                        dataRdr = cmd.ExecuteReader();

                        while (dataRdr.Read())
                        {
                            string table = dataRdr.GetString(0);
                            output.Add(table);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                    finally
                    {
                        odbcConn.Close();
                    }
                    break;

                case "ODP":

                    // no need, oracle is wierd like that
                    //OracleConnection odpCon = new OracleConnection(_connectionString);

                    //try
                    //{
                    //    odpCon.Open();

                    //    OracleCommand cmd = new OracleCommand();
                    //    cmd.Connection = odpCon;
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.CommandText = "sp_databases";

                    //    OracleDataReader reader;
                    //    reader = cmd.ExecuteReader();

                    //    while (reader.Read())
                    //    {
                    //        string table = reader.GetString(0);
                    //        output.Add(table);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    errorMessage = ex.Message;
                    //}
                    //finally
                    //{
                    //    odpCon.Close();
                    //}
                    break;
            }
			
			return output;
		}
		#endregion

		#region Get Tables
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Return databases on that server
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public DBSchema GetSchema(string database, out string errorMessage)
		{
			errorMessage = string.Empty;
			DBSchema output = new DBSchema();
			output.DatabaseName = database;

            switch (_connectionType)
            {
                case "Oledb":
                    #region SQL Server
                    SqlConnection sqlConn = new SqlConnection(_connectionString);

                    try
                    {
                        sqlConn.Open();
                        sqlConn.ChangeDatabase(database);

                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = sqlConn;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "sp_tables";

                        SqlDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string owner = reader.GetString(1);
                            string _type = reader.GetString(3);
                            string _table = reader.GetString(2);
                            if (owner.ToLower() == "dbo" && (_type.ToLower() == "table" || _type.ToLower() == "view")
                                && (_table != "dtproperties" && _table != "sysconstraints" && _table != "syssegments"))
                            {
                                // User defined table
                                DBTable table = new DBTable();
                                table.TableName = _table;

                                table.IsView = (_type.ToLower() == "view");

                                // Get Columns
                                SqlConnection colConn = new SqlConnection(_connectionString);
                                colConn.Open();
                                colConn.ChangeDatabase(database);

                                SqlCommand colCmd = new SqlCommand();
                                colCmd.Connection = colConn;
                                colCmd.CommandType = CommandType.StoredProcedure;
                                colCmd.CommandText = "sp_columns";
                                colCmd.Parameters.Add("@table_name", _table);

                                SqlDataReader colReader = colCmd.ExecuteReader();

                                while (colReader.Read())
                                {
                                    DBColumn col = new DBColumn();
                                    col.ColName = colReader.GetString(3);
                                    col.ColDBType = colReader.GetString(5);
                                    if (colReader.GetInt16(10) == 1)
                                        col.Nullable = true;
                                    col.Length = colReader.GetInt32(7);
                                    col.Type = GetColumnType(col);

                                    // Add to collection
                                    if (col.ColName.ToLower() != "id")
                                        table.AddColumn(col);
                                }

                                colConn.Close();

                                // Add to collection
                                output.Add(table);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                    finally
                    {
                        sqlConn.Close();
                    }
                    #endregion
                    break;

                case "ODBC":
                    #region MySQL

                    OdbcConnection oleConn = new OdbcConnection(_connectionString + ";DATABASE=" + database);

                    try
                    {
                        oleConn.Open();

                        OdbcCommand cmd = new OdbcCommand();
                        cmd.Connection = oleConn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "show full tables";

                        OdbcDataReader dataRdr;
                        dataRdr = cmd.ExecuteReader();

                        while (dataRdr.Read())
                        {
                            string _table = MakeFirstUpper(dataRdr.GetString(0));
                            string _type = dataRdr.GetString(1);

                            // User defined table
                            DBTable table = new DBTable();
                            table.TableName = _table;

                            table.IsView = (_type.ToLower() == "view");

                            // Add to collection
                            output.Add(table);
                        }

                        foreach (DBTable table in output.Tables.Values)
                        {
                            OdbcConnection colConn = new OdbcConnection(_connectionString + ";DATABASE=" + database);
                            colConn.Open();

                            // Get Columns
                            OdbcCommand colCmd = new OdbcCommand();
                            colCmd.Connection = colConn;
                            colCmd.CommandType = CommandType.Text;
                            colCmd.CommandText = "show columns from `" + table.TableName + "`";

                            OdbcDataReader colReader = colCmd.ExecuteReader();

                            while (colReader.Read())
                            {
                                DBColumn col = new DBColumn();
                                col.ColName = MakeFirstUpper(colReader.GetString(0));
                                col.ColDBType = colReader.GetString(1);
                                col.Nullable = colReader.GetBoolean(2);

                                if (col.ColDBType.IndexOf("(") > 0)
                                {
                                    if (col.ColDBType.IndexOf(",") > 0)
                                    {
                                        // get between the brackets and ","
                                        col.Length = int.Parse(col.ColDBType.Substring(col.ColDBType.IndexOf("(") + 1,
                                            col.ColDBType.IndexOf(",") - (col.ColDBType.IndexOf("(") + 1)));
                                    }
                                    else
                                    {
                                        // get between the brackets
                                        col.Length = int.Parse(col.ColDBType.Substring(col.ColDBType.IndexOf("(") + 1,
                                            col.ColDBType.IndexOf(")") - (col.ColDBType.IndexOf("(") + 1)));
                                    }
                                }
                                col.Type = GetColumnType(col);

                                // Add to collection
                                if (col.ColName.ToLower() != "id")
                                    table.AddColumn(col);
                            }

                            colConn.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                    finally
                    {
                        oleConn.Close();
                    }
                    #endregion
                    break;

                case "ODP":
                    #region Oracle
                    OracleConnection odpConn = new OracleConnection(_connectionString);

                    try
                    {
                        odpConn.Open();

                        OracleCommand cmd = new OracleCommand();
                        cmd.Connection = odpConn;
                        cmd.CommandText = "select * from tab";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string _type = reader.GetString(1);
                            string _table = reader.GetString(0);
                            if ((_type.ToLower() == "table" || _type.ToLower() == "view") && !_table.Contains("$"))
                            {
                                // User defined table
                                DBTable table = new DBTable();
                                table.TableName = _table;

                                table.IsView = (_type.ToLower() == "view");

                                // Get Columns
                                OracleConnection colConn = new OracleConnection(_connectionString);
                                colConn.Open();

                                OracleCommand colCmd = new OracleCommand();
                                colCmd.Connection = colConn;
                                colCmd.CommandText = "SELECT * FROM user_tab_cols WHERE table_name = '" + _table + "'";

                                OracleDataReader colReader = colCmd.ExecuteReader();

                                while (colReader.Read())
                                {
                                    DBColumn col = new DBColumn();
                                    col.ColName = colReader.GetString(1);
                                    col.ColDBType = colReader.GetString(2);

                                    col.Length = int.Parse(colReader.GetOracleValue(5).ToString());
                                    col.Type = GetColumnType(col);

                                    // Add to collection
                                    if (col.ColName.ToLower() != "id")
                                        table.AddColumn(col);
                                }

                                colConn.Close();

                                // Add to collection
                                output.Add(table);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                    finally
                    {
                        odpConn.Close();
                    }
                    #endregion
                    break;
            }
			
			return output;
		}
		#endregion

		#region Utilities
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Laborious function to get all lookup references
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public void GetLookupTables(DBSchema schema)
		{
			foreach(DBTable table in schema.Tables.Values)
			{
				foreach(DBColumn col in table.Columns.Values)
				{
					string columnType = col.Type;
					string columnName = col.ColName;

					if ((columnType == "int") && (columnName.ToUpper().EndsWith("ID")))
					{
						// Could be a lookup - check schema
						string lookupTableName = columnName.Replace("ID",string.Empty);

						if (lookupTableName != string.Empty)
						{
							col.LookupTable = (DBTable)schema.Tables[lookupTableName];
							if (col.LookupTable != null)
							{
								if (col.LookupTable.Columns["Name"] != null)
									col.LookupColumn = (DBColumn)col.LookupTable.Columns["Name"];

							}
						}
					}
				}
			}
		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Compair two schemas and return a combination
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public DBSchema CompareSchemas(DBSchema newSchema, DBSchema oldSchema)
		{
			DBSchema output = newSchema;
			foreach(DBTable oldTable in oldSchema.Tables.Values)
			{
				DBTable newTable = (DBTable)newSchema.Tables[oldTable.TableName];
				
				if (newTable != null)
				{
					foreach(DBColumn oldColumn in oldTable.Columns.Values)
					{
						DBColumn newColumn = (DBColumn)newTable.Columns[oldColumn.ColName];

						if(newColumn != null && oldColumn.ColDBType == newColumn.ColDBType)
						{
							// Set saved values
							if (oldColumn.LookupTable != null)
								newColumn.LookupTable = oldColumn.LookupTable;
							if (oldColumn.LookupColumn != null)
								newColumn.LookupColumn = oldColumn.LookupColumn;
						}
					}
				}
			}

			return output;
		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Capitalize the name of the column - this bugs me no end!
		/// </summary>
		/////////////////////////////////////////////////////////////////
		private string MakeFirstUpper( string name )
		{
			if ( name.Length <= 1) return name.ToUpper();
			Char[] letters = name.ToCharArray();
			letters[0] = Char.ToUpper( letters[0] );
			return new string( letters );
		}
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Get the C# type for a database column
		/// </summary>
		/////////////////////////////////////////////////////////////////
		private string GetColumnType(DBColumn col)
		{
			string colType = col.ColDBType.ToLower();

			// MySql appends the length, irritatingly
			if (colType.IndexOf("(")>0)
				colType = colType.Substring(0,colType.IndexOf("("));

            switch (colType)
            {
                case "nvarchar":
                case "varchar2":
                case "varchar":
                case "text":
                case "mediumtext":
                case "nvarchar(max)":
                case "ntext":
                case "nchar":
                    return "string";

                case "int":
                case "int identity":
                case "tinyint":
                case "smallint":
                    return "int";

                case "bigint":
                    return "Int64";

                case "bit":
                    return "bool";

                case "decimal":
                case "integer": // oracle
                case "float": // oracle
                case "number": // oracle
                case "unsignedinteger": //oracle
                case "real":
                case "money":
                    return "decimal";

                case "double":
                    return "double";

                case "date":
                case "time":
                case "datetime":
                    return "DateTime";

                case "varbinary":
                    return "Byte[]";

            }
			return string.Empty;
		}
		#endregion
	}
}
