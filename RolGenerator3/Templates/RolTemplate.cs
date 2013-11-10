using System;
using System.Collections;
using System.Text;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Template for ROL items - Header, ItemHeader, Item and Footer
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class RolTemplate
	{
		#region Render Header
        public static string RenderHeader(DBTable table, string nameSpace)
        {
            string tableName = table.TableName;

            return String.Format(@"
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using {0}DAL;

namespace {0}RelationalObjectsLayer
{{
	
	///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for {1} data Item.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
	[Serializable]
	public class {1} : DataObject
	{{
		// attribute data (note that ID is inherited)", nameSpace, tableName);
        }
		#endregion

		#region Render Header Item
		public static string RenderHeaderItem(string columnName, string columnType)
		{

			return @"
		private " + columnType + " _" + columnName.ToLower() + ";";

		}
		#endregion

		#region Render Item
		public static string RenderItem(DBColumn col)
		{
			string columnName = col.ColName;
			string columnType = col.Type;

			StringBuilder output = new StringBuilder();

			output.Append(@"
		public " + columnType + " "+ columnName + @"
		{
			get {return _" + columnName.ToLower() + @";}
			set { _" + columnName.ToLower() + @" = value;}
		}
		");

			// Write lookup function
			if (col.LookupTable != null && col.LookupColumn != null)
			{
				output.Append(@"

		//////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Get the value from the lookup table
		/// </summary>
		//////////////////////////////////////////////////////////////////////
		[XmlIgnore]
		public " + col.LookupColumn.Type + " " + col.ColName + @"Value
		{
			get {
				" + col.LookupTable.TableName + @" lookup = new " + col.LookupTable.TableName + @"(ConnectionSettings);
				if (lookup.Load(" + col.ColName + @"))
					return lookup." + col.LookupColumn.ColName + @";
				else
					return null;
			}
		}

		");
			}

			return output.ToString();
		}
		#endregion

		#region Render Footer
        public static string RenderFooter(DBTable table)
        {
            string tableName = table.TableName;
            return String.Format(@"
		//////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Constructor
		/// </summary>
		//////////////////////////////////////////////////////////////////////
		public {0}(ConnectionSettings conn)
		{{
			ConnectionSettings = conn;
			DataBaseTable = ""{0}"";
            
            #region Procedure Setup
			LoadProcedureName = ""{0}_LOAD"";
			AddProcedureName = ""{0}_ADD"";
			EditProcedureName = ""{0}_EDIT"";
			DeleteProcedureName = ""{0}_DELETE"";
            #endregion
		}}

		public {0}()
		{{
			DataBaseTable = ""{0}"";

            #region Procedure Setup
			LoadProcedureName = ""{0}_LOAD"";
			AddProcedureName = ""{0}_ADD"";
			EditProcedureName = ""{0}_EDIT"";
			DeleteProcedureName = ""{0}_DELETE"";
            #endregion
		}}
	}}

	///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for {0} data list.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
	[Serializable]
	public class {0}List : DataObjectList
	{{
        private List<{0}> _typedList; 

        #region Constructors
		public {0}List(ConnectionSettings conn)
		{{
			ConnectionSettings = conn;
			DataBaseTable = ""{0}"";

            #region Procedure Setup
			SearchProcedureName = ""{0}_SEARCH"";
			DeleteAllProcedureName = ""{0}_DELETEALL"";	
            #endregion		
		}}

		public {0}List()
		{{
			DataBaseTable = ""{0}"";

            #region Procedure Setup
			SearchProcedureName = ""{0}_SEARCH"";
			DeleteAllProcedureName = ""{0}_DELETEALL"";	
            #endregion		
		}}
        #endregion

        #region List Object
	    ////////////////////////////////////////////////////////////////
	    /// <summary>
	    /// Strongly-typed list
	    /// </summary>
	    ////////////////////////////////////////////////////////////////
        public List<{0}> List
        {{
            get 
            {{
                if (_typedList == null)
                {{
                    _typedList = new List<{0}>();
                    foreach (DataObject obj in Items)
                        _typedList.Add(({0})obj);
                }}
                return _typedList;
            }}
            set 
            {{
                _typedList = value;
            }}
        }}
        #endregion

		public override DataObject CreateDataObject()
		{{
			return new {0}(ConnectionSettings);
		}}
	}}
}}", tableName);
        }
		#endregion
	}
}
