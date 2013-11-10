
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
	/// Unit tests for SYSTEMUSER
	/// </summary>
	//////////////////////////////////////////////////////////////////////////////////// 
	[TestFixture]
	public class SYSTEMUSERTests
	{

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
			SYSTEMUSERList dataSource = new SYSTEMUSERList(Connection.GetConnection());
			
			// Perform a simple search
			dataSource.Search(string.Empty, string.Empty);
			pass = (dataSource.Items.Count > 0);
			
			// Return Test Result
			Assert.IsTrue(pass);
		}
		#endregion


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
			SYSTEMUSER dataSource = new SYSTEMUSER(Connection.GetConnection());	
			
			// Assign Values
			
			dataSource.EMAIL = "Test";
			dataSource.LASTLOGIN = DateTime.Now;
			dataSource.PASSWORD = "Test";
			dataSource.USERNAME = "Test";
			
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
			SYSTEMUSER dataSource = new SYSTEMUSER(Connection.GetConnection());	
			
			// Assign Values
			
			dataSource.EMAIL = "Test";
			dataSource.LASTLOGIN = DateTime.Now;
			dataSource.PASSWORD = "Test";
			dataSource.USERNAME = "Test";
			
			// Attempt Save
			pass = dataSource.Save();

			// Edit values
			if (pass)
			{
				
			dataSource.EMAIL = "Test";
			dataSource.LASTLOGIN = DateTime.Now;
			dataSource.PASSWORD = "Test";
			dataSource.USERNAME = "Test";

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
			SYSTEMUSER dataSource = new SYSTEMUSER(Connection.GetConnection());	
			
			// Assign Values
			
			dataSource.EMAIL = "Test";
			dataSource.LASTLOGIN = DateTime.Now;
			dataSource.PASSWORD = "Test";
			dataSource.USERNAME = "Test";
			
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
			SYSTEMUSER dataSource = new SYSTEMUSER(Connection.GetConnection());	
			
			// Assign Values
			
			dataSource.EMAIL = "Test";
			dataSource.LASTLOGIN = DateTime.Now;
			dataSource.PASSWORD = "Test";
			dataSource.USERNAME = "Test";
			
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

	}
}
