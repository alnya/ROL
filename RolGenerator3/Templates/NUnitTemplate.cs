using System;
using System.Collections;
using System.Text;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Template for nUnit items - Seach, Add, Edit, Load and Delete
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class NUnitTemplate
	{
		#region Header and Footer
		public static string RenderHeader(DBTable table)
		{
			return @"
using System;
using System.Collections;
using System.Web;

using DAL;
using NUnit.Framework;
using RelationalObjectsLayer;

namespace UnitTests
{
	////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Unit tests for " + table.TableName + @"
	/// </summary>
	//////////////////////////////////////////////////////////////////////////////////// 
	[TestFixture]
	public class " + table.TableName + @"Tests
	{";
		}

		public static string RenderFooter()
		{
			return @"	}
}";
		}
		#endregion

		#region Search
		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns an nunit test to search a database table or view
		/// </summary>
		/////////////////////////////////////////////////////////////
		public static string RenderSearchTest(DBTable table)
		{
			return @"
		#region Basic Tests: Search
		////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test to return a list
		/// </summary>
		//////////////////////////////////////////////////////////////////////////////////// 
		[Test]
		public void SearchTest()
		{
			bool pass = false;
			" + table.TableName + @"List dataSource = new " + table.TableName + @"List(Connection.GetConnection());
			
			// Perform a simple search
			dataSource.Search(string.Empty, string.Empty);
			pass = (dataSource.Items.Count > 0);
			
			// Return Test Result
			Assert.IsTrue(pass);
		}
		#endregion
";
		
		}
		#endregion

		#region Add
		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns an nunit test to add a new record
		/// </summary>
		/////////////////////////////////////////////////////////////
		public static string RenderAddTest(DBTable table)
		{
			return @"
		#region Basic Tests: Insert
		////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test to insert a record into the database
		/// </summary>
		//////////////////////////////////////////////////////////////////////////////////// 
		[Test]
		public void AddTest()
		{
			bool pass = false;
			" + table.TableName + @" dataSource = new " + table.TableName + @"(Connection.GetConnection());	
			
			// Assign Values
			" + GetValues(table) + @"
			
			// Attempt Save
			pass = dataSource.Save();
			
			// Clearup - Delete if save successful
			if (pass)
			{
				dataSource.Delete();
			}
			
			// Return Test Result
			Assert.IsTrue(pass);
		}
		#endregion
";
		}
		#endregion

		#region Edit
		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns an nunit test to edit a record
		/// </summary>
		/////////////////////////////////////////////////////////////
		public static string RenderEditTest(DBTable table)
		{
			return @"
		#region Basic Tests: Update
		////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test to edit a record from the database
		/// </summary>
		//////////////////////////////////////////////////////////////////////////////////// 
		[Test]
		public void EditTest()
		{
			bool pass = false;
			" + table.TableName + @" dataSource = new " + table.TableName + @"(Connection.GetConnection());	
			
			// Assign Values
			" + GetValues(table) + @"
			
			// Attempt Save
			pass = dataSource.Save();

			// Edit values
			if (pass)
			{
				" + GetValues(table) + @"

				// Attempt Save
				pass = dataSource.Save();
			}
			
			// Clearup - Delete if save successful
			if (pass)
			{
				dataSource.Delete();
			}
			
			// Return Test Result
			Assert.IsTrue(pass);
		}
		#endregion
";
		}
		#endregion

		#region Load
		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns an nunit test to load a database row
		/// </summary>
		/////////////////////////////////////////////////////////////
		public static string RenderLoadTest(DBTable table)
		{
			return @"
		#region Basic Tests: Load
		////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test to load a record from the database
		/// </summary>
		//////////////////////////////////////////////////////////////////////////////////// 
		[Test]
		public void LoadTest()
		{
			bool pass = false;
			" + table.TableName + @" dataSource = new " + table.TableName + @"(Connection.GetConnection());	
			
			// Assign Values
			" + GetValues(table) + @"
			
			// Attempt Save
			pass = dataSource.Save();

			// Attempt Load
			if (pass)
			{
				int ID = dataSource.ID;
				pass = dataSource.Load(ID);
			}
			
			// Clearup - Delete if save successful
			if (pass)
			{
				dataSource.Delete();
			}
			
			// Return Test Result
			Assert.IsTrue(pass);
		}
		#endregion
";

		}
		#endregion

		#region Delete
		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Returns an nunit test to delete a database row
		/// </summary>
		/////////////////////////////////////////////////////////////
		public static string RenderDeleteTest(DBTable table)
		{
			return @"
		#region Basic Tests: Delete
		////////////////////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Test to delete a record from the database
		/// </summary>
		//////////////////////////////////////////////////////////////////////////////////// 
		[Test]
		public void DeleteTest()
		{
			bool pass = false;
			" + table.TableName + @" dataSource = new " + table.TableName + @"(Connection.GetConnection());	
			
			// Assign Values
			" + GetValues(table) + @"
			
			// Attempt Save
			pass = dataSource.Save();
	
			// Clearup - Delete if save successful
			if (pass)
			{
				pass = dataSource.Delete();
			}
			
			// Return Test Result
			Assert.IsTrue(pass);
		}
		#endregion
";
		}
		#endregion

		#region GetValues
		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Get values and cols for a table and set default test data
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private static string GetValues(DBTable table)
		{
			StringBuilder sb = new StringBuilder();

			foreach(DBColumn col in table.SortedColumns().Values)
			{
				sb.Append(@"
			dataSource.");
				sb.Append(col.ColName);
				sb.Append(" = ");

				switch(col.Type.ToLower())
				{
					case "string":
						sb.Append(@"""Test"";");
						break;

					case "Int64":
					case "int":
						sb.Append(@"1;");
						break;

					case "bool":
						sb.Append(@"true;");
						break;

					case "decimal":
						sb.Append(@"1.1M;");
						break;

					case "double":
						sb.Append(@"5.5;");
						break;

					case "datetime":
						sb.Append(@"DateTime.Now;");
						break;
				}
			}

			return sb.ToString();
		}
		#endregion
	}
}
