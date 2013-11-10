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
	public class DataObject
    {
        #region Variables
        // access data
		private string dbTable;
		private bool isNew;
		private int id;
		private ConnectionSettings connectionSettings;
        private string errormessage;

        // procedure name properties
        [XmlIgnore]
        public string LoadProcedureName = string.Empty;
        [XmlIgnore]
        public string AddProcedureName = string.Empty;
        [XmlIgnore]
        public string EditProcedureName = string.Empty;
        [XmlIgnore]
        public string DeleteProcedureName = string.Empty;

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
		/// Is new record.
		/// </summary>
		/////////////////////////////////////////////////////////////////////
		public bool IsNew
		{
			get { return isNew;	}
			set { isNew = value;}
		}

		/////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Identity
		/// </summary>
		/////////////////////////////////////////////////////////////////////
		public int ID
		{
			get {return id;}
			set {id = value;}
		}

        /////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Any error messages
        /// </summary>
        /////////////////////////////////////////////////////////////////////
        public string ErrorMessage
        {
            get { return errormessage; }
            set { errormessage = value; }
        }

		/// <summary>
		/// Constructor
		/// </summary>
		public DataObject()
		{
			// new object by default
			isNew = true;
        }
        #endregion

        #region Save Object
        ///////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Save the item
		/// </summary>
		/// <returns></returns>
		///////////////////////////////////////////////////////////////////////////
		public bool Save()
		{
			// build collection
            bool success = false;

			NameValueCollection nameValueCollection = new NameValueCollection();

            List<DbParameter> paramaters = new List<DbParameter>();

			// get all attributes with the "_" prefix (these should map to database columns)
			MemberInfo[] datamembers = GetType().FindMembers(MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic, Type.FilterName, "_*");
			
			// get name / value
			foreach (FieldInfo field in datamembers)
			{
				string mappedDataColumnName;
				string mappedDataColumnValue;

				object[] attribs = field.GetCustomAttributes(typeof(DataColumnAttribute), true);
				if (attribs.GetLength(0) == 0) 
					mappedDataColumnName = field.Name.Substring(1).ToLower(); 
				else 
				{ 
					DataColumnAttribute changeAttrib = (DataColumnAttribute)attribs[0];
					mappedDataColumnName = changeAttrib.name;
				}

                // create a parameter
                DbParameter param = connectionSettings.Factory.CreateParameter();
                param.DbType = Provider.GetType(field.FieldType);
                param.ParameterName = "@" + mappedDataColumnName;
                param.Direction = ParameterDirection.Input;

                if (field.GetValue(this) != null)
                {
                    mappedDataColumnValue = field.GetValue(this).ToString();
                    if (field.FieldType.ToString() == "System.DateTime")
                    {
                        // convert to universal date time
                        if (ConnectionSettings.ConnectionType.ToLower() == "oracle")
                            mappedDataColumnValue = Format.DBDateOracle(field.GetValue(this).ToString());
                        else
                        {
                            mappedDataColumnValue = Format.DBDate(field.GetValue(this).ToString());
                            mappedDataColumnValue = "'" + mappedDataColumnValue + "'";
                        }
                    }

                    if (field.FieldType.ToString() == "System.String")
                    {
                        mappedDataColumnValue = Format.DBString(mappedDataColumnValue);
                        mappedDataColumnValue = "'" + mappedDataColumnValue + "'";
                    }

                    param.Value = mappedDataColumnValue;

                    // add to collection
                    nameValueCollection.Add(mappedDataColumnName, mappedDataColumnValue);
                }
                else
                    param.Value = System.DBNull.Value.ToString();

                paramaters.Add(param);
			}

            DataCommands cmd = new DataCommands(connectionSettings);
            if (isNew)
            {
                if (this.ConnectionSettings.UseStoredProcedures)
                {
                    DbParameter param = connectionSettings.Factory.CreateParameter();
                    param.DbType = DbType.Int32;
                    param.ParameterName = "@ID";
                    param.Direction = ParameterDirection.Output;
                    paramaters.Add(param);

                    success = cmd.ExecuteProcedure(AddProcedureName, ref paramaters, out errormessage);
                    if (success)
                        id = (int)paramaters[paramaters.Count - 1].Value;
                }
                else
                {
                    success = cmd.Insert(dbTable, nameValueCollection, out id, out errormessage);
                }

                if (success)
                {
                    isNew = false;
                    return true;
                }
                else
                    return false;
            }
            else
            {
                if (this.ConnectionSettings.UseStoredProcedures)
                {
                    DbParameter param1 = connectionSettings.Factory.CreateParameter();
                    param1.DbType = DbType.String;
                    param1.ParameterName = "@ID";
                    param1.Value = id;
                    paramaters.Add(param1);

                    DbParameter param2 = connectionSettings.Factory.CreateParameter();
                    param2.DbType = DbType.Int16;
                    param2.ParameterName = "@RowsUpdated";
                    param2.Direction = ParameterDirection.Output;
                    paramaters.Add(param2);

                    success = cmd.ExecuteProcedure(EditProcedureName, ref paramaters, out errormessage);
                }
                else
                {
                    success = cmd.Update(dbTable, nameValueCollection, id, out errormessage);
                }

                return success; 
            }
        }
        #endregion

        #region Load Object
        ///////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Load the item (all columns)
        /// </summary>
        ///////////////////////////////////////////////////////////////////////////
        public bool Load(int ItemID)
        {
            StringBuilder cols = new StringBuilder();

            // get all attributes with the "_" prefix (these should map to database columns)
            MemberInfo[] datamembers = GetType().FindMembers(MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic, Type.FilterName, "_*");

            // set name / value for each column
            foreach (FieldInfo field in datamembers)
            {
                if (cols.Length > 0)
                    cols.Append(",");
                cols.Append(field.Name.Replace("_", string.Empty));
            }

            return Load(cols.ToString(), ItemID);
        }

		///////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Load the item (certain columns - comma seperated)
		/// </summary>
		/// <returns></returns>
		///////////////////////////////////////////////////////////////////////////
        public bool Load(string cols, int ItemID)
		{
            bool success = false;
			
            DataSet data = new DataSet();

            DataCommands cmd = new DataCommands(connectionSettings);
           
            // Are we using stored procedures, or building up the SQL
            if (this.ConnectionSettings.UseStoredProcedures)
            {
                List<DbParameter> paramaters = new List<DbParameter>();

                DbParameter param1 = connectionSettings.Factory.CreateParameter();
                param1.DbType = DbType.Int32;
                param1.ParameterName = "@ID";
                param1.Direction = ParameterDirection.Input;
                param1.Value = ItemID;
                paramaters.Add(param1);

                success = cmd.ExecuteProcedure(LoadProcedureName, paramaters, out data, out errormessage);
            }
            else
            {
                success = cmd.Load(dbTable, cols, ItemID, out data, out errormessage);
            }

            if (success)
			{
				// check that it has only one result
				if (data.Tables[0].Rows.Count == 1)
				{
					// build the object
					id = ItemID;
					isNew = false;

					// get all attributes with the "_" prefix (these should map to database columns)
					MemberInfo[] datamembers = GetType().FindMembers(MemberTypes.Field, BindingFlags.Instance | BindingFlags.NonPublic, Type.FilterName, "_*");
			
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
							(data.Tables[0].Rows[0][mappedDataColumnName] != DBNull.Value))
						{
							object mappedDataColumnValue = data.Tables[0].Rows[0][mappedDataColumnName];

                            if (field.FieldType.ToString() == "System.String" && mappedDataColumnValue != DBNull.Value)
							{
                                mappedDataColumnValue = Format.FromDBString((string)mappedDataColumnValue);
							}

							field.SetValue(this,mappedDataColumnValue);
						}
					}

					return true;
				}
			}

			// return error
			return false;
        }
        #endregion

        #region Delete Object
        ///////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Delete the item
		/// </summary>
		/// <returns></returns>
		///////////////////////////////////////////////////////////////////////////
		public bool Delete()
		{
            bool success = false;
            DataCommands cmd = new DataCommands(connectionSettings);

            // Are we using stored procedures, or building up the SQL
            if (this.ConnectionSettings.UseStoredProcedures)
            {
                List<DbParameter> paramaters = new List<DbParameter>();

                DbParameter param1 = connectionSettings.Factory.CreateParameter();
                param1.DbType = DbType.Int16;
                param1.ParameterName = "@ID";
                param1.Value = id;
                paramaters.Add(param1);

                DbParameter param2 = connectionSettings.Factory.CreateParameter();
                param2.DbType = DbType.Int16;
                param2.ParameterName = "@RowsUpdated";
                param2.Direction = ParameterDirection.Output;
                paramaters.Add(param2);

                success = cmd.ExecuteProcedure(DeleteProcedureName, ref paramaters, out errormessage);
            }
            else
            {
                success = cmd.Delete(dbTable, id, out errormessage);
            }

            return success;
        }
        #endregion
    }

    #region Data Column Override
    ///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// The purpose of this class is to override the default names for 
	/// DataObject descendants data memebers.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class DataColumnAttribute : System.Attribute 
	{
		private string mName;

		public DataColumnAttribute(string name)    
		{
			mName = name;
		}

		public string name
		{
			get { return mName;}
		}

    }
    #endregion
} 
