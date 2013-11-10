using System;
using System.Text;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Template for Web Services
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class WebServiceTemplate
	{
		#region Page Infront
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Render page infront header
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static string RenderServiceInfront(DBTable table, string nameSpace)
		{
			string tableName = table.TableName + "Service";
			
			return String.Format(@"<%@ WebService Language=""C#"" CodeBehind=""{0}.asmx.cs"" Class=""{1}.{0}"" %>",tableName,nameSpace);
		}
		#endregion

		#region Service Behind
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Render service
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static string RenderServiceBehind(DBTable table, string nameSpace)
		{
			string tableName = table.TableName;
			
			StringBuilder output = new StringBuilder();

			output.Append(String.Format(@"
using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using {1}.DAL;
using {1}.RelationalObjectsLayer;

namespace {1}.WebServices
{{

[WebService(Namespace = ""http://tempuri.org/"")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class {0}Service : System.Web.Services.WebService
{{

	#region Constructor
    ConnectionSettings Conn = null;
    public {0}Service () 
	{{
        // Create a new connection object
        Conn = DataServices.GetConnection();
    }}
	#endregion
",tableName,nameSpace));

			// only load, edit, delete for tables, not views
			if (!table.IsView)
			{
				output.Append(String.Format(@"
	#region Data Item Services
    /////////////////////////////////////////////////////////////////
    /// <summary>
    /// Load one item from supplied ID
    /// </summary>
    /////////////////////////////////////////////////////////////////
    [WebMethod]
    public {0} Load(int ID)
    {{
        {0} dataSource = new {0}(Conn);
        dataSource.Load(ID);
        return dataSource;
    }}

    /////////////////////////////////////////////////////////////////
    /// <summary>
    /// Save one item or create new, depending on status
    /// </summary>
    /////////////////////////////////////////////////////////////////
    [WebMethod]
    public {0} Save({0} dataSource)
    {{
        dataSource.ConnectionSettings = Conn;
        dataSource.Save();
        return dataSource;
    }}

    /////////////////////////////////////////////////////////////////
    /// <summary>
    /// Delete an existing data item
    /// </summary>
    /////////////////////////////////////////////////////////////////
    [WebMethod]
    public string Delete({0} dataSource)
    {{
        dataSource.ConnectionSettings = Conn;
        if (dataSource.Delete())
            return ""Deleted successfully"";
        else
            return dataSource.ErrorMessage;

    }}
	#endregion
",tableName));
			}

			output.Append(String.Format(@"
	#region Data list Services
    /////////////////////////////////////////////////////////////////
    /// <summary>
    /// Search for data items which meet your criteria
    /// </summary>
    /////////////////////////////////////////////////////////////////
    [WebMethod]
    public {0}List Search(string where, string orderby)
    {{
        {0}List dataList = new {0}List(Conn);
        dataList.Search(where, orderby);
        return dataList;
    }}
	#endregion
}}}}
",tableName));

			return output.ToString();
		}
		#endregion
	}
}
