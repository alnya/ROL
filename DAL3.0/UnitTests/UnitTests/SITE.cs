
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
	/// Unit tests for SITE
	/// </summary>
	//////////////////////////////////////////////////////////////////////////////////// 
	[TestFixture]
	public class SITETests
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
			SITEList dataSource = new SITEList(Connection.GetConnection());
			
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
			SITE dataSource = new SITE(Connection.GetConnection());	
			
			// Assign Values
			
			dataSource.ACTIVE = 1;
			dataSource.GMTOFFSET = 1;
			dataSource.LATITUDE = "Test";
			dataSource.LONGITUDE = "Test";
			dataSource.NAME = "Test";
			dataSource.PARENTSITEID = 1;
			
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
			SITE dataSource = new SITE(Connection.GetConnection());	
			
			// Assign Values
			
			dataSource.ACTIVE = 1;
			dataSource.GMTOFFSET = 1;
			dataSource.LATITUDE = "Test";
			dataSource.LONGITUDE = "Test";
			dataSource.NAME = "Test";
			dataSource.PARENTSITEID = 1;
			
			// Attempt Save
			pass = dataSource.Save();

			// Edit values
			if (pass)
			{
				
			dataSource.ACTIVE = 1;
			dataSource.GMTOFFSET = 1;
			dataSource.LATITUDE = "Test";
			dataSource.LONGITUDE = "Test";
			dataSource.NAME = "Test";
			dataSource.PARENTSITEID = 1;

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
			SITE dataSource = new SITE(Connection.GetConnection());	
			
			// Assign Values
			
			dataSource.ACTIVE = 1;
			dataSource.GMTOFFSET = 1;
			dataSource.LATITUDE = "Test";
			dataSource.LONGITUDE = "Test";
			dataSource.NAME = "Test";
			dataSource.PARENTSITEID = 1;
			
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
			SITE dataSource = new SITE(Connection.GetConnection());	
			
			// Assign Values
			
			dataSource.ACTIVE = 1;
			dataSource.GMTOFFSET = 1;
			dataSource.LATITUDE = "Test";
			dataSource.LONGITUDE = "Test";
			dataSource.NAME = "Test";
			dataSource.PARENTSITEID = 1;
			
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
