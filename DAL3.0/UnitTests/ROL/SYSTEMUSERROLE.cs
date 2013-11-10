
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DAL;

namespace RelationalObjectsLayer
{
	
	///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for SYSTEMUSERROLE data Item.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
	[Serializable]
	public class SYSTEMUSERROLE : DataObject
	{
		// attribute data (note that ID is inherited)
		private int _roleid;
		private int _systemuserid;

		public int ROLEID
		{
			get {return _roleid;}
			set { _roleid = value;}
		}
		
		public int SYSTEMUSERID
		{
			get {return _systemuserid;}
			set { _systemuserid = value;}
		}
		
		//////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Constructor
		/// </summary>
		//////////////////////////////////////////////////////////////////////
		public SYSTEMUSERROLE(ConnectionSettings conn)
		{
			ConnectionSettings = conn;
			DataBaseTable = "SYSTEMUSERROLE";
            
            #region Procedure Setup
			LoadProcedureName = "SYSTEMUSERROLE_LOAD";
			AddProcedureName = "SYSTEMUSERROLE_ADD";
			EditProcedureName = "SYSTEMUSERROLE_EDIT";
			DeleteProcedureName = "SYSTEMUSERROLE_DELETE";
            #endregion
		}

		public SYSTEMUSERROLE()
		{
			DataBaseTable = "SYSTEMUSERROLE";

            #region Procedure Setup
			LoadProcedureName = "SYSTEMUSERROLE_LOAD";
			AddProcedureName = "SYSTEMUSERROLE_ADD";
			EditProcedureName = "SYSTEMUSERROLE_EDIT";
			DeleteProcedureName = "SYSTEMUSERROLE_DELETE";
            #endregion
		}
	}

	///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for SYSTEMUSERROLE data list.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
	[Serializable]
	public class SYSTEMUSERROLEList : DataObjectList
	{
        private List<SYSTEMUSERROLE> _typedList; 

        #region Constructors
		public SYSTEMUSERROLEList(ConnectionSettings conn)
		{
			ConnectionSettings = conn;
			DataBaseTable = "SYSTEMUSERROLE";

            #region Procedure Setup
			SearchProcedureName = "SYSTEMUSERROLE_SEARCH";
			DeleteAllProcedureName = "SYSTEMUSERROLE_DELETEALL";	
            #endregion		
		}

		public SYSTEMUSERROLEList()
		{
			DataBaseTable = "SYSTEMUSERROLE";

            #region Procedure Setup
			SearchProcedureName = "SYSTEMUSERROLE_SEARCH";
			DeleteAllProcedureName = "SYSTEMUSERROLE_DELETEALL";	
            #endregion		
		}
        #endregion

        #region List Object
	    ////////////////////////////////////////////////////////////////
	    /// <summary>
	    /// Strongly-typed list
	    /// </summary>
	    ////////////////////////////////////////////////////////////////
        public List<SYSTEMUSERROLE> List
        {
            get 
            {
                if (_typedList == null)
                {
                    _typedList = new List<SYSTEMUSERROLE>();
                    foreach (DataObject obj in Items)
                        _typedList.Add((SYSTEMUSERROLE)obj);
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
			return new SYSTEMUSERROLE(ConnectionSettings);
		}
	}
}