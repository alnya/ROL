
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DAL;

namespace RelationalObjectsLayer
{
	
	///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for SYSTEMUSER data Item.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
	[Serializable]
	public class SYSTEMUSER : DataObject
	{
		// attribute data (note that ID is inherited)
		private string _email;
		private DateTime _lastlogin;
		private string _password;
		private string _username;

		public string EMAIL
		{
			get {return _email;}
			set { _email = value;}
		}
		
		public DateTime LASTLOGIN
		{
			get {return _lastlogin;}
			set { _lastlogin = value;}
		}
		
		public string PASSWORD
		{
			get {return _password;}
			set { _password = value;}
		}
		
		public string USERNAME
		{
			get {return _username;}
			set { _username = value;}
		}
		
		//////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Constructor
		/// </summary>
		//////////////////////////////////////////////////////////////////////
		public SYSTEMUSER(ConnectionSettings conn)
		{
			ConnectionSettings = conn;
			DataBaseTable = "SYSTEMUSER";
            
            #region Procedure Setup
			LoadProcedureName = "SYSTEMUSER_LOAD";
			AddProcedureName = "SYSTEMUSER_ADD";
			EditProcedureName = "SYSTEMUSER_EDIT";
			DeleteProcedureName = "SYSTEMUSER_DELETE";
            #endregion
		}

		public SYSTEMUSER()
		{
			DataBaseTable = "SYSTEMUSER";

            #region Procedure Setup
			LoadProcedureName = "SYSTEMUSER_LOAD";
			AddProcedureName = "SYSTEMUSER_ADD";
			EditProcedureName = "SYSTEMUSER_EDIT";
			DeleteProcedureName = "SYSTEMUSER_DELETE";
            #endregion
		}
	}

	///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for SYSTEMUSER data list.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
	[Serializable]
	public class SYSTEMUSERList : DataObjectList
	{
        private List<SYSTEMUSER> _typedList; 

        #region Constructors
		public SYSTEMUSERList(ConnectionSettings conn)
		{
			ConnectionSettings = conn;
			DataBaseTable = "SYSTEMUSER";

            #region Procedure Setup
			SearchProcedureName = "SYSTEMUSER_SEARCH";
			DeleteAllProcedureName = "SYSTEMUSER_DELETEALL";	
            #endregion		
		}

		public SYSTEMUSERList()
		{
			DataBaseTable = "SYSTEMUSER";

            #region Procedure Setup
			SearchProcedureName = "SYSTEMUSER_SEARCH";
			DeleteAllProcedureName = "SYSTEMUSER_DELETEALL";	
            #endregion		
		}
        #endregion

        #region List Object
	    ////////////////////////////////////////////////////////////////
	    /// <summary>
	    /// Strongly-typed list
	    /// </summary>
	    ////////////////////////////////////////////////////////////////
        public List<SYSTEMUSER> List
        {
            get 
            {
                if (_typedList == null)
                {
                    _typedList = new List<SYSTEMUSER>();
                    foreach (DataObject obj in Items)
                        _typedList.Add((SYSTEMUSER)obj);
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
			return new SYSTEMUSER(ConnectionSettings);
		}
	}
}