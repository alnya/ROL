
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using DAL;

namespace RelationalObjectsLayer
{
	
	///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for SITE data Item.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
	[Serializable]
	public class SITE : DataObject
	{
		// attribute data (note that ID is inherited)
		private int _active;
		private int _gmtoffset;
		private string _latitude;
		private string _longitude;
		private string _name;
		private int _parentsiteid;

		public int ACTIVE
		{
			get {return _active;}
			set { _active = value;}
		}
		
		public int GMTOFFSET
		{
			get {return _gmtoffset;}
			set { _gmtoffset = value;}
		}
		
		public string LATITUDE
		{
			get {return _latitude;}
			set { _latitude = value;}
		}
		
		public string LONGITUDE
		{
			get {return _longitude;}
			set { _longitude = value;}
		}
		
		public string NAME
		{
			get {return _name;}
			set { _name = value;}
		}
		
		public int PARENTSITEID
		{
			get {return _parentsiteid;}
			set { _parentsiteid = value;}
		}
		
		//////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Constructor
		/// </summary>
		//////////////////////////////////////////////////////////////////////
		public SITE(ConnectionSettings conn)
		{
			ConnectionSettings = conn;
			DataBaseTable = "SITE";
            
            #region Procedure Setup
			LoadProcedureName = "SITE_LOAD";
			AddProcedureName = "SITE_ADD";
			EditProcedureName = "SITE_EDIT";
			DeleteProcedureName = "SITE_DELETE";
            #endregion
		}

		public SITE()
		{
			DataBaseTable = "SITE";

            #region Procedure Setup
			LoadProcedureName = "SITE_LOAD";
			AddProcedureName = "SITE_ADD";
			EditProcedureName = "SITE_EDIT";
			DeleteProcedureName = "SITE_DELETE";
            #endregion
		}
	}

	///////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for SITE data list.
	/// </summary>
	///////////////////////////////////////////////////////////////////////////
	[Serializable]
	public class SITEList : DataObjectList
	{
        private List<SITE> _typedList; 

        #region Constructors
		public SITEList(ConnectionSettings conn)
		{
			ConnectionSettings = conn;
			DataBaseTable = "SITE";

            #region Procedure Setup
			SearchProcedureName = "SITE_SEARCH";
			DeleteAllProcedureName = "SITE_DELETEALL";	
            #endregion		
		}

		public SITEList()
		{
			DataBaseTable = "SITE";

            #region Procedure Setup
			SearchProcedureName = "SITE_SEARCH";
			DeleteAllProcedureName = "SITE_DELETEALL";	
            #endregion		
		}
        #endregion

        #region List Object
	    ////////////////////////////////////////////////////////////////
	    /// <summary>
	    /// Strongly-typed list
	    /// </summary>
	    ////////////////////////////////////////////////////////////////
        public List<SITE> List
        {
            get 
            {
                if (_typedList == null)
                {
                    _typedList = new List<SITE>();
                    foreach (DataObject obj in Items)
                        _typedList.Add((SITE)obj);
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
			return new SITE(ConnectionSettings);
		}
	}
}