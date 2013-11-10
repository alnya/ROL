using System;
using System.Configuration;
using DAL;

namespace UnitTests
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Connection Settings
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class Connection
	{
		public static ConnectionSettings GetConnection()
		{
			ConnectionSettings conn = new ConnectionSettings(ConfigurationSettings.AppSettings["ConnectionType"].ToString());

            conn.Server = ConfigurationSettings.AppSettings["ConnectionServer"].ToString();
            conn.InitialCatalogue = ConfigurationSettings.AppSettings["ConnectionDataBase"].ToString();
            conn.Port = ConfigurationSettings.AppSettings["ConnectionPort"].ToString();
            conn.UserID = ConfigurationSettings.AppSettings["ConnectionUserID"].ToString();
            conn.Password = ConfigurationSettings.AppSettings["ConnectionPassword"].ToString();

            conn.UseStoredProcedures = false;

			return conn;
		}
	}
}
