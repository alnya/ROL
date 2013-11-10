using System;
using System.Data.Common;
using System.Text;

namespace DAL
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// An object to hold your connection settings in
	/// </summary>
	/////////////////////////////////////////////////////////////////
    public class ConnectionSettings
    {
        #region Private Variables
        private string _server;
		private string _initialCat;
		private string _userID;
		private string _password;
		private string _connectionType;
		private string _port;
        private bool _useStoredProcedures;

        DbConnection conn;
        DbTransaction trans;
        DbProviderFactory factory;
        private bool _transEnabled = false;
        #endregion

        #region Connection Settings
        /////////////////////////////////////////////////
		/// <summary>
		/// IP Address of the server
		/// </summary>
		/////////////////////////////////////////////////
		public string Server
		{
			get {return _server;}
			set {_server = value;}
		}

		/////////////////////////////////////////////////
		/// <summary>
		/// Port that the database is on (optional)
		/// </summary>
		/////////////////////////////////////////////////
		public string Port
		{
			get {return _port;}
			set {_port = value;}
		}

		/////////////////////////////////////////////////
		/// <summary>
		/// Database
		/// </summary>
		/////////////////////////////////////////////////
		public string InitialCatalogue
		{
            get { return _initialCat; }
            set { _initialCat = value; }
		}

		/////////////////////////////////////////////////
		/// <summary>
		/// User name for database access
		/// </summary>
		/////////////////////////////////////////////////
		public string UserID
		{
			get {return _userID;}
			set {_userID = value;}
		}

		/////////////////////////////////////////////////
		/// <summary>
		/// Password for database access
		/// </summary>
		/////////////////////////////////////////////////
		public string Password
		{
			get {return _password;}
			set {_password = value;}
		}
		
		/////////////////////////////////////////////////
		/// <summary>
		///  ODBC or OLEDB
		/// </summary>
		/////////////////////////////////////////////////
		public string ConnectionType
		{
			get {return _connectionType;}
			set {_connectionType = value;}
        }

        /////////////////////////////////////////////////
        /// <summary>
        /// Are transactions running
        /// </summary>
        /////////////////////////////////////////////////
        public bool TransactionsEnabled
        {
            get { return _transEnabled; }
        }

        /////////////////////////////////////////////////
        /// <summary>
        /// Are transactions running
        /// </summary>
        /////////////////////////////////////////////////
        public bool UseStoredProcedures
        {
            get { return _useStoredProcedures; }
            set { _useStoredProcedures = value; }
        }

        /////////////////////////////////////////////////
        /// <summary>
        /// The connection Object
        /// </summary>
        /////////////////////////////////////////////////
        public DbConnection DBConnection
        {
            get
            {
                if (conn.State == System.Data.ConnectionState.Closed && 
                    conn.ConnectionString == string.Empty)
                    conn.ConnectionString = this.ConnectionString;
                return conn;
            }
        }

        /////////////////////////////////////////////////
        /// <summary>
        /// The connection Object
        /// </summary>
        /////////////////////////////////////////////////
        public DbTransaction Transaction
        {
            get
            {
                return trans;
            }
        }
        #endregion

        #region Constructor
        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Constructor of the object
        /// </summary>
        /////////////////////////////////////////////////////////////
        public ConnectionSettings(string dbType)
        {
            this.ConnectionType = dbType;

            DbProviderFactory factory = DbProviderFactories.GetFactory(Provider.GetProvider(this));
            conn = factory.CreateConnection();
        }

        public ConnectionSettings()
        {
            throw new Exception("Connection Settings must have a connection type");
        }
        #endregion

        #region Transaction Handling
        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Start a transaction
        /// </summary>
        /////////////////////////////////////////////////////////////
        public void BeginDbTransaction(System.Data.IsolationLevel isolationLevel)
        {
            _transEnabled = true;
            trans = conn.BeginTransaction(isolationLevel);
        }

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Commit Transaction
        /// </summary>
        /////////////////////////////////////////////////////////////
        public void CommitDbTransaction()
        {
            trans.Commit();
            _transEnabled = false;
        }

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Rollback an in-progress transaction
        /// </summary>
        /////////////////////////////////////////////////////////////
        public void RollbackDbTransaction()
        {
            trans.Rollback();
            _transEnabled = false;
        }
        #endregion

        #region Connection Functions
        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Close the connection
        /// </summary>
        /////////////////////////////////////////////////////////////
        public void Close()
        {
            conn.Close();
        }

        /////////////////////////////////////////////////////////////
        /// <summary>
        /// Open the connection (if not open)
        /// </summary>
        /////////////////////////////////////////////////////////////
        public void Open()
        {
            conn.ConnectionString = this.ConnectionString;
            if (conn.State != System.Data.ConnectionState.Open)
                conn.Open();
        }
        #endregion

        #region Connection Information
        /////////////////////////////////////////////////////////////
        /// <summary>
        /// The connection string
        /// </summary>
        /////////////////////////////////////////////////////////////
        public string ConnectionString
        {
            get
            {
                StringBuilder dns = new StringBuilder();
                if (_connectionType.ToLower() == "oledb")
                {
                    dns.Append("Provider=sqloledb;Data Source=");
                    dns.Append(_server);
                    dns.Append(";Initial Catalog=");
                    dns.Append(_initialCat);
                    dns.Append(";User Id=");
                    dns.Append(_userID);
                    dns.Append(";Password=");
                    dns.Append(_password);
                }
                else if (_connectionType.ToLower() == "odbc")
                {
                    dns.Append("DRIVER={MySQL ODBC 3.51 Driver};SERVER=");
                    dns.Append(_server);
                    dns.Append(";DATABASE=");
                    dns.Append(_initialCat);
                    dns.Append(";UID=");
                    dns.Append(_userID);
                    dns.Append(";PASSWORD=");
                    dns.Append(_password);
                    dns.Append(";OPTION=3");
                    if (_port != string.Empty)
                    {
                        dns.Append(";port=");
                        dns.Append(_port);
                    }
                }
                else if (_connectionType.ToLower() == "oracle")
                {
                    dns.Append("Data Source=");
                    dns.Append(_server);
                    dns.Append(";User Id=");
                    dns.Append(_userID);
                    dns.Append(";Password=");
                    dns.Append(_password);
                }

                return dns.ToString();
            }
        }
        #endregion

        #region Factory Object
        /////////////////////////////////////////////////////////////
        /// <summary>
        /// The DBFactory Object for these connection settings
        /// </summary>
        /////////////////////////////////////////////////////////////
        public DbProviderFactory Factory
        {
            get
            {
                if (factory == null)
                    factory = DbProviderFactories.GetFactory(Provider.GetProvider(this));

                return factory;
            }
        }
        #endregion
    }
}
