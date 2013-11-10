
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DAL;

namespace RelationalObjectsLayer
{
	
	///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for ROLE data Item.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
	[Serializable]
	public class ROLE : DataObject
	{
		// attribute data (note that ID is inherited)
		private string _name;

		public string NAME
		{
			get {return _name;}
			set { _name = value;}
		}
		
		//////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Constructor
		/// </summary>
		//////////////////////////////////////////////////////////////////////
		public ROLE(ConnectionSettings conn)
		{
			ConnectionSettings = conn;
			DataBaseTable = "ROLE";
            
            #region Procedure Setup
			LoadProcedureName = "ROLE_LOAD";
			AddProcedureName = "ROLE_ADD";
			EditProcedureName = "ROLE_EDIT";
			DeleteProcedureName = "ROLE_DELETE";
            #endregion
		}

		public ROLE()
		{
			DataBaseTable = "ROLE";

            #region Procedure Setup
			LoadProcedureName = "ROLE_LOAD";
			AddProcedureName = "ROLE_ADD";
			EditProcedureName = "ROLE_EDIT";
			DeleteProcedureName = "ROLE_DELETE";
            #endregion
		}
	}

	///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for ROLE data list.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
	[Serializable]
	public class ROLEList : DataObjectList
	{
        private List<ROLE> _typedList; 

        #region Constructors
		public ROLEList(ConnectionSettings conn)
		{
			ConnectionSettings = conn;
			DataBaseTable = "ROLE";

            #region Procedure Setup
			SearchProcedureName = "ROLE_SEARCH";
			DeleteAllProcedureName = "ROLE_DELETEALL";	
            #endregion		
		}

		public ROLEList()
		{
			DataBaseTable = "ROLE";

            #region Procedure Setup
			SearchProcedureName = "ROLE_SEARCH";
			DeleteAllProcedureName = "ROLE_DELETEALL";	
            #endregion		
		}
        #endregion

        #region List Object
	    ////////////////////////////////////////////////////////////////
	    /// <summary>
	    /// Strongly-typed list
	    /// </summary>
	    ////////////////////////////////////////////////////////////////
        public List<ROLE> List
        {
            get 
            {
                if (_typedList == null)
                {
                    _typedList = new List<ROLE>();
                    foreach (DataObject obj in Items)
                        _typedList.Add((ROLE)obj);
                }
                return _typedList;
            }
            set 
            {
                _typedList = value;
            }
        }
        #endregion

		public override DataObject CreateDataObject()
		{
			return new ROLE(ConnectionSettings);
		}
	}
}