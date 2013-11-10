using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace RolGenerator
{
	#region Connection Object
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// The database Schema
	/// </summary>
	/////////////////////////////////////////////////////////////////
	[Serializable]
	public class Connection
	{
		private string _type;
		private string _server;
		private string _username;
		private string _password;
		private string _port;

		public Connection(string type, string server, string username, string password,string port)
		{
            _type = type;
			_server = server;
			_username = username;
			_password = password;
			_port = port;
		}

		/// <summary>
		/// COnnection Type
		/// </summary>
		public string Type
		{
			set {_type = value;}
            get { return _type; }
		}

		/// <summary>
		/// Server
		/// </summary>
		public string Server
		{
			set {_server = value;}
			get {return _server;}
		}

		/// <summary>
		/// Username
		/// </summary>
		public string Username
		{
			set {_username = value;}
			get {return _username;}
		}

		/// <summary>
		/// Password
		/// </summary>
		public string Password
		{
			set {_password = value;}
			get {return _password;}
		}

		/// <summary>
		/// Connection Port
		/// </summary>
		public string Port
		{
			set {_port = value;}
			get {return _port;}
		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Save the connection to disk
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public void Save(string filepath)
		{

			Stream s = File.Open(filepath, FileMode.Create, FileAccess.ReadWrite);
			BinaryFormatter b = new BinaryFormatter();
			b.Serialize( s, this );
			s.Close();
		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Load the schema from disk
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static Connection Load(string filepath)
		{
			Stream s = null;

			Connection conn = null;

			try
			{
				if (File.Exists(filepath))
				{
					s = File.Open(filepath, FileMode.Open, FileAccess.Read);

					// Deserialize the content of the XML file
					BinaryFormatter b = new BinaryFormatter();
					conn = (Connection) b.Deserialize(s);
				}
			}
			catch( FileNotFoundException )
			{
				// Do nothing if the file does not exists
			}
			finally
			{
				if ( s != null ) s.Close();
			}

			return conn;
		}
	}
	#endregion
}
