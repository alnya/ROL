using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.OleDb;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace DAL
{
	///////////////////////////////////////////////////////////////////////////
	/// <summary>
	///  Base class for all database objects
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
    [Serializable]
	public class DataObjectList
    {
        #region Variables
        // access data
		private string dbTable;
        private List<DataObject> items = new List<DataObject>();
		private ConnectionSettings connectionSettings;
        private string errormessage;

        // procedure name properties
        [XmlIgnore]
        public string SearchProcedureName = string.Empty;
        [XmlIgnore]
        public string DeleteAllProcedureName = string.Empty;

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// The Database Table of the object.
		/// </summary>
		/////////////////////////////////////////////////////////////////////
        [XmlIgnore]
		public string DataBaseTable
		{
			get { return dbTable;	}
			set { dbTable = value;	}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// The Connection Settings of the object.
		/// </summary>
		/////////////////////////////////////////////////////////////////////
        [XmlIgnore]
		public ConnectionSettings ConnectionSettings
		{
			get { return connectionSettings;	}
			set { connectionSettings = value;	}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Any error messages
		/// </summary>
		/////////////////////////////////////////////////////////////////////
        public string ErrorMessage
		{
			get { return errormessage;	}
			set { errormessage = value;	}
		}


		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// The Database Object Type.
		/// </summary>
		/////////////////////////////////////////////////////////////////////
		public virtual DataObject CreateDataObject()
		{
			return new DataObject();
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// The items in the list
		/// </summary>
		/////////////////////////////////////////////////////////////////////
		public List<DataObject> Items
		{
			get { return items;	}
			set { items = value;}
        }
        #endregion

        #region Search
        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Search for items (where list)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////
        public bool Search(WhereList where, string orderby)
        {
            DataObject dataObject = CreateDataObject();

            StringBuilder cols = new StringBuilder();

            // get all attributes with the "_" prefix (these should map to database columns)
            MemberInfo[] datamembers = dataObject.GetType().FindMembers(MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic, Type.FilterName, "_*");

            // set name / value for each column
            foreach (FieldInfo field in datamembers)
            {
                if (cols.Length > 0)
                    cols.Append(",");
                cols.Append(field.Name.Replace("_", string.Empty));
            }

            return Search(cols.ToString(), where.ToString(connectionSettings.ConnectionType), orderby);
        }
        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Search for items (all columns)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////
        public bool Search(string where, string orderby)
        {
            DataObject dataObject = CreateDataObject();

            StringBuilder cols = new StringBuilder();

            // get all attributes with the "_" prefix (these should map to database columns)
            MemberInfo[] datamembers = dataObject.GetType().FindMembers(MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic, Type.FilterName, "_*");

            // set name / value for each column
            foreach (FieldInfo field in datamembers)
            {
                if (cols.Length > 0)
                    cols.Append(",");
                cols.Append(field.Name.Replace("_",string.Empty));
            }

            return Search(cols.ToString(), where, orderby);
        }

        ///////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Search for items (specific columns - comma seperated (ID included by default)
		/// </summary>
		///////////////////////////////////////////////////////////////////////////
		public bool Search(string cols, string where, string orderby)
		{
			DataSet data = new DataSet();

            bool success = false;

            DataCommands cmd = new DataCommands(connectionSettings);

            // Are we using stored procedures, or building up the SQL
            if (this.ConnectionSettings.UseStoredProcedures)
            {
                List<DbParameter> paramaters = new List<DbParameter>();

                DbParameter param1 = connectionSettings.Factory.CreateParameter();
                param1.DbType = DbType.String;
                param1.ParameterName = "@where";
                param1.Direction = ParameterDirection.Input;
                param1.Value = where;
                paramaters.Add(param1);

                DbParameter param2 = connectionSettings.Factory.CreateParameter();
                param2.DbType = DbType.String;
                param2.ParameterName = "@orderBy";
                param2.Direction = ParameterDirection.Input;
                param2.Value = orderby;
                paramaters.Add(param2);

                success = cmd.ExecuteProcedure(SearchProcedureName, paramaters, out data, out errormessage);
            }
            else
            {
                success = cmd.Search(dbTable, cols, where, orderby, out data, out errormessage);
            }

            if (success)
			{
				// for each row, create a new object and fill it
				foreach (DataRow dr in data.Tables[0].Rows)
				{
					DataObject dataObject = CreateDataObject();

					// firstly, set the ID 
					dataObject.ID = (int)dr["ID"];
					dataObject.IsNew = false;
		
					// get all attributes with the "_" prefix (these should map to database columns)
					MemberInfo[] datamembers = dataObject.GetType().FindMembers(MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic, Type.FilterName, "_*");
			
					// set name / value for each column
					foreach (FieldInfo field in datamembers)
					{
						string mappedDataColumnName;

						object[] attribs = field.GetCustomAttributes(typeof(DataColumnAttribute), true);
						if (attribs.GetLength(0) == 0) 
							mappedDataColumnName = field.Name.Substring(1).ToLower(); 
						else 
						{ 
							DataColumnAttribute changeAttrib = (DataColumnAttribute)attribs[0];
							mappedDataColumnName = changeAttrib.name;
						}
						// get value from dataset and apply to variable name
                        if ((data.Tables[0].Columns.Contains(mappedDataColumnName)) &&
                            (dr[mappedDataColumnName] != DBNull.Value))
                        {
                            object mappedDataColumnValue = dr[mappedDataColumnName];

                            if (field.FieldType.ToString() == "System.String" && mappedDataColumnValue != DBNull.Value)
                            {
                                mappedDataColumnValue = Format.FromDBString((string)mappedDataColumnValue);
                            }

                            field.SetValue(dataObject, dr[mappedDataColumnName]);
                        }
					}
				
					items.Add(dataObject);
				}
				return true;
			}
			else
			{
				// big bada boom
				return false;
			}
        }
        #endregion

        #region Delete All
        ///////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Delete a list of items
		/// </summary>
		/// <returns></returns>
		///////////////////////////////////////////////////////////////////////////
		public bool DeleteAll(string where)
		{
            DataCommands cmd = new DataCommands(connectionSettings);

            bool success = false;

            // Are we using stored procedures, or building up the SQL
            if (this.ConnectionSettings.UseStoredProcedures)
            {
                List<DbParameter> paramaters = new List<DbParameter>();

                DbParameter param1 = connectionSettings.Factory.CreateParameter();
                param1.DbType = DbType.String;
                param1.ParameterName = "@where";
                param1.Value = where;
                paramaters.Add(param1);

                DbParameter param2 = connectionSettings.Factory.CreateParameter();
                param2.DbType = DbType.Int16;
                param2.ParameterName = "@RowsUpdated";
                param2.Direction = ParameterDirection.Output;
                paramaters.Add(param2);

                success = cmd.ExecuteProcedure(DeleteAllProcedureName, ref paramaters, out errormessage);
            }
            else
            {
                success = cmd.DeleteAll(dbTable, where, out errormessage);
            }

            return success;
        }
        #endregion
    }
} 
