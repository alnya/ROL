using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;

namespace RolGenerator
{
	#region DB Schema
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// The database Schema
	/// </summary>
	/////////////////////////////////////////////////////////////////
	[Serializable]
	public class DBSchema
	{
		string _databaseName;
		SortedList _sortedList = new SortedList();
		Hashtable _tables = new Hashtable();

		public string DatabaseName
		{
			set {_databaseName = value;}
			get {return _databaseName;}
		}

		[XmlArray ("Tables"), XmlArrayItem("Table", typeof(DBTable))]
		public Hashtable Tables
		{
			set {_tables = value;}
			get {return _tables;}
		}

		public void Add(DBTable table)
		{
			_tables.Add(table.TableName,table);

		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// A sorted list
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public SortedList SortedTables()
		{
			if (_sortedList.Count == 0)
				foreach(DBTable table in _tables.Values)
					_sortedList.Add(table.TableName, table);
			
			return _sortedList;
		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Save the schema to disk
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
		public static DBSchema Load(string filepath)
		{
			Stream s = null;

			DBSchema schema = null;

			try
			{
				if (File.Exists(filepath))
				{
					s = File.Open(filepath, FileMode.Open, FileAccess.Read);

					// Deserialize the content of the XML file
					BinaryFormatter b = new BinaryFormatter();
					schema = (DBSchema) b.Deserialize(s);
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

			return schema;
		}
	}
	#endregion

	#region DB Table
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// A database Table
	/// </summary>
	/////////////////////////////////////////////////////////////////
	[Serializable]
	public class DBTable
	{
		string _tableName;
		SortedList _sortedList = new SortedList();
		Hashtable _columns = new Hashtable();
		bool _isView;

		public string TableName
		{
			set {_tableName = value;}
			get {return _tableName;}
		}

		[XmlArray ("Columns"), XmlArrayItem("Column", typeof(DBColumn))]
		public Hashtable Columns
		{
			set {_columns = value;}
			get {return _columns;}
		}

		public bool IsView
		{
			set {_isView = value;}
			get {return _isView;}
		}

		public void AddColumn(DBColumn col)
		{
			_columns.Add(col.ColName,col);
		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// A sorted list
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public SortedList SortedColumns()
		{
			if (_sortedList.Count == 0)
				foreach(DBColumn col in _columns.Values)
					_sortedList.Add(col.ColName, col);
			
			return _sortedList;
		}
	}
	#endregion

	#region DB Column
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// A database Column
	/// </summary>
	/////////////////////////////////////////////////////////////////
	[Serializable]	
	public class DBColumn
	{
		string _colName;
		string _colDBType;
		string _type;
		bool _nullable;
		int _length = 0;
		DBTable _lookupTable = null;
		DBColumn _lookupColumn = null;

		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Name of column
		/// </summary>
		/////////////////////////////////////////////////////////////
		public string ColName
		{
			set {_colName = value;}
			get {return _colName;}
		}

		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Database Type
		/// </summary>
		/////////////////////////////////////////////////////////////
		public string ColDBType
		{
			set {_colDBType = value;}
			get {return _colDBType;}
		}

		/////////////////////////////////////////////////////////////
		/// <summary>
		/// C# Type
		/// </summary>
		/////////////////////////////////////////////////////////////
		public string Type
		{
			set {_type = value;}
			get {return _type;}
		}

		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Can be null
		/// </summary>
		/////////////////////////////////////////////////////////////
		public bool Nullable
		{
			set {_nullable = value;}
			get {return _nullable;}
		}

		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Maximum Length
		/// </summary>
		/////////////////////////////////////////////////////////////
		public int Length
		{
			set {_length = value;}
			get {return _length;}
		}

		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Lookup Table
		/// </summary>
		/////////////////////////////////////////////////////////////
		public DBTable LookupTable
		{
			set {_lookupTable = value;}
			get {return _lookupTable;}
		}

		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Lookup Column
		/// </summary>
		/////////////////////////////////////////////////////////////
		public DBColumn LookupColumn
		{
			set {_lookupColumn = value;}
			get {return _lookupColumn;}
		}
	}
	#endregion
}
