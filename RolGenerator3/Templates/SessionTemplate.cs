using System;
using System.Collections;
using System.Text;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Template for session items - Header, ItemHeader, Item and Footer
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class SessionTemplate
	{
		public static string RenderHeader()
		{
			return @"
using System.Web;
using System.Data;
using System.Web.SessionState;
using System.Security.Principal;
using System.Threading;
using RelationalObjectsLayer;

namespace SessionBag
{
	/////////////////////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Used to access properties that constitute the user's session state.
	/// </summary>
	/////////////////////////////////////////////////////////////////////////////////////
	public class UserSession
	{
		private System.Web.SessionState.HttpSessionState session;

		public UserSession(HttpContext context)
		{
			session = context.Session;
		}

		private object GetValue(string key)
		{
			return session[key];
		}

		private void SetValue(string key,object value)
		{
			session[key] = value;
		}
";

		}

		public static string RenderHeaderItems(DBSchema schema)
		{
			StringBuilder output = new StringBuilder();
			
			output.Append(@"
		/////////////////////////////////////////////////////////////////////////////////
		// Constants for the item keys. This is extra work but is worth it to ensure
		// that we don't use duplicate item keys.
		/////////////////////////////////////////////////////////////////////////////////
		private const string LoggedInUserKey			= ""LoggedInUserKey"";
		private const string LoggedInUserRolesKey		= ""LoggedInUserRolesKey"";
		
");
			foreach(DBTable table in schema.Tables.Values)
			{
				string tableName = table.TableName;

				output.Append(@"
		private const string ");
				output.Append(tableName);
				output.Append(@"Key		= """);
				output.Append(tableName);
				output.Append(@"Key"";
		private const string ");
				output.Append(tableName);
				output.Append(@"ListKey		= """);
				output.Append(tableName);
				output.Append(@"ListKey"";");
			}			
			
			return output.ToString();
		}

		public static string RenderPublicItems(DBSchema schema)
		{
			StringBuilder output = new StringBuilder();
			
			output.Append(@"
		/////////////////////////////////////////////////////////////////////////////////
		// Logged In User
		/////////////////////////////////////////////////////////////////////////////////
		public Systemuser LoggedInUser
		{
			get { return (Systemuser)GetValue(LoggedInUserKey);	}
			set { SetValue(LoggedInUserKey, value);				}
		}

		/////////////////////////////////////////////////////////////////////////////////
		// Logged In User Roles
		/////////////////////////////////////////////////////////////////////////////////
		public SystemuserroleList LoggedInUserRoles
		{
			get { return (SystemuserroleList)GetValue(LoggedInUserRolesKey);	}
			set { SetValue(LoggedInUserRolesKey, value);			}
		}
		
");
			foreach(DBTable table in schema.Tables.Values)
			{
				string tableName = table.TableName;

				output.Append(@"
		/////////////////////////////////////////////////////////////////////////////////
		// " + tableName + @"
		/////////////////////////////////////////////////////////////////////////////////
		public " + tableName + @" " + tableName + @"
		{
			get { return (" + tableName + @")GetValue(" + tableName + @"Key);	}
			set { SetValue(" + tableName + @"Key, value);				}
		}

		/////////////////////////////////////////////////////////////////////////////////
		// " + tableName + @" List
		/////////////////////////////////////////////////////////////////////////////////
		public " + tableName + @"List " + tableName + @"List
		{
			get { return (" + tableName + @"List)GetValue(" + tableName + @"ListKey);	}
			set { SetValue(" + tableName + @"ListKey, value);				}
		}
");
			}			
			
			return output.ToString();
		}

		public static string RenderClear(DBSchema schema)
		{
			StringBuilder output = new StringBuilder();
			
			output.Append(@"
		/////////////////////////////////////////////////////////////////////////////////
		// Clear Session
		/////////////////////////////////////////////////////////////////////////////////
		public void Clear()
		{
			");

			foreach(DBTable table in schema.Tables.Values)
			{
				string tableName = table.TableName;

				output.Append(@"
			SetValue(" + tableName + @"Key, null);
			SetValue(" + tableName + @"ListKey, null);");
			
			}

			output.Append(@"
		}
		");

			return output.ToString();
		}


		public static string RenderFooter()
		{
			return @"
	}
}
";
		}
	}
}
