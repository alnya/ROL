using System;
using System.Text;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Template for Controls
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class ControlTemplate
	{
		#region Page Infront
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Render header for page infront
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static string RenderPageInfrontHeader(DBTable table, bool IsReadOnly)
		{
			string tableName = table.TableName;
			if (IsReadOnly)
				tableName = tableName + "_RO";
			
			return @"
<%@ Page Language=""C#"" MasterPageFile=""~/Master/MasterPage.master"" AutoEventWireup=""true"" CodeFile=""" + tableName + @".aspx.cs"" Inherits=""" + tableName + @"_page"" Title=""Untitled Page"" %>
<%@ MasterType VirtualPath=""~/Master/MasterPage.master"" %>
<asp:Content ID=""Content1"" ContentPlaceHolderID=""MainContent"" Runat=""Server"">
	<h1>" + tableName + @"</h1>
		<fieldset>";

		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Render item for page infront
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static string RenderPageInfrontItem(DBColumn col)
		{
			string columnName = col.ColName;
			StringBuilder output = new StringBuilder();

			// Label
			output.Append(@"
			<div>
				<label for=""");
			
			output.Append(columnName);
			output.Append(@""">");
			output.Append(columnName);
			output.Append(@":</label>
				");
			
			// Control
			switch (col.Type)
			{
					////////////////////////////////////////
					// TEXT
					////////////////////////////////////////
				case "string":

					output.Append(@"<asp:TextBox ID=""");
					output.Append(columnName);
					output.Append(@""" Runat=""server"" ");
					if (col.Length < 500)
					{
						output.Append(@"MaxLength=""");
						output.Append(col.Length);
						output.Append(@"""");
					}
					else
						output.Append(@"TextMode=""MultiLine""");

					output.Append(@" class=""field"" />");
					break;

					////////////////////////////////////////
					// NUMBER
					////////////////////////////////////////
				case "int":
				case "decimal":
					if (col.LookupTable == null)
					{
						output.Append(@"<asp:TextBox ID=""");
						output.Append(columnName);
						output.Append(@""" Runat=""server"" MaxLength=""");
						output.Append(col.Length);
						output.Append(@""" class=""number"" />");
					}
					else
					{
						output.Append(@"<asp:DropDownList id=""");
						output.Append(columnName);
						output.Append(@""" runat=""server""/>");
					}
					break;

					////////////////////////////////////////
					// BOOLEAN
					////////////////////////////////////////
				case "bool":
					output.Append(@"<asp:CheckBox id=""");
					output.Append(columnName);
					output.Append(@""" runat=""server"" class=""checkbox""/>");
					break;

					////////////////////////////////////////
					// DATE
					////////////////////////////////////////
				case "DateTime":
					output.Append(@"<uc:DateTime runat=""server"" id=""");
					output.Append(columnName);
					output.Append(@""" EnableClientScript=""true"" Format="";day;/;month;/;year""/>");
					break;
			}			

			//Validation
			if (!col.Nullable)
			{
				output.Append(@"
				<asp:RequiredFieldValidator ID=""");

				output.Append(columnName);
				output.Append(@"RFV"" ControlToValidate=""");
				output.Append(columnName);
				output.Append(@""" Runat=""server"" Text=""*"" ErrorMessage=""");
				output.Append(columnName);
				output.Append(@" is required"" />");
			}

			output.Append(@"
			</div>");

			return output.ToString();

		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Render Footer for page in front
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static string RenderPageInfrontFooter(DBTable table)
		{
		
			return @"
			<div>
				<asp:Button ID=""submit"" cssclass=""button"" runat=""server"" Text=""Save"" OnClick=""Submit_Click""/>
			</div>
	</fieldset>
</asp:Content>";

		}
		#endregion

		#region Code Behind
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Render header for code behind
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static string RenderCodeBehindHeader(DBTable table)
		{
			string tableName = table.TableName;

			StringBuilder output = new StringBuilder();

			output.Append(@"
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using RelationalObjectsLayer;
using DAL;

namespace Pages
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	///	Page for " + tableName + @"
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public partial class " + tableName + @"_page : BasePage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				BindData();
				CopyToForm();
			}
		}

		#region BindData
		/////////////////////////////////////////////////////////////////
		/// <summary>
		///	Populate Lookup Lists
		/// </summary>
		/////////////////////////////////////////////////////////////////
		private void BindData()
		{
");
			foreach(DBColumn col in table.Columns.Values)
			{
				if (col.LookupTable != null && col.LookupColumn != null)
				{
					output.Append(@"
			");
					output.Append(col.ColName);
					output.Append(@".DataSource = dataSource.Get");
					output.Append(col.LookupTable.TableName);
					output.Append(@"LookupList(string.Empty,string.Empty);
			");
					output.Append(col.ColName);
					output.Append(@".DataTextField = """);
					output.Append(col.LookupColumn.ColName);
					output.Append(@""";
			");
					output.Append(col.ColName);
					output.Append(@".DataValueField = ""ID"";
			");
					output.Append(col.ColName);
					output.Append(@".DataBind();

");
				}
			}

			output.Append(@"
		}
		#endregion
");

			return output.ToString();
		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Render copy to form for code behind
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static string RenderCodeBehindCopyToForm(DBTable table)
		{
			StringBuilder output = new StringBuilder();

			output.Append(@"
		#region CopyToForm
		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Copy the object to the appropriate sub-controls
		/// </summary>
		/////////////////////////////////////////////////////////////
		public void CopyToForm()
		{
			int ID = 0;
			this.UserSession." + table.TableName + @" = new " + table.TableName + @"(this.Conn);

			if (Request[""ID""] != null)
			if (!int.TryParse(Request[""ID""], out ID))
                Utils.RaiseError(Context, ""Could not load " + table.TableName + @""");

			if (ID != 0)
			{

				" + table.TableName + @" dataSource = new " + table.TableName + @"(this.Conn);
				if (!dataSource.Load(ID))
					Utils.RaiseError(Context, ""Could not load " + table.TableName + @""");

				this.UserSession." + table.TableName + @" = dataSource;

");

			foreach(DBColumn col in table.Columns.Values)
			{
				string columnName = col.ColName;

				output.Append(@"
				// ");
				output.Append(columnName);
				output.Append(@"
				");

				// Control
				switch (col.Type)
				{
						////////////////////////////////////////
						// TEXT
						////////////////////////////////////////
					case "string":
						output.Append(columnName);
						output.Append(@".Text = dataSource.");
						output.Append(columnName);
						output.Append(@";");
						break;

						////////////////////////////////////////
						// NUMBER
						////////////////////////////////////////
					case "int":
					case "decimal":
						if (col.LookupTable == null)
						{
							output.Append(columnName);
							output.Append(@".Text = dataSource.");
							output.Append(columnName);
							output.Append(@".ToString();");
						}
						else
						{
							output.Append(columnName);
							output.Append(@".SelectedValue = dataSource.");
							output.Append(columnName);
							output.Append(@".ToString();");
						}
						break;

						////////////////////////////////////////
						// BOOLEAN
						////////////////////////////////////////
					case "bool":
						output.Append(columnName);
						output.Append(@".Checked = dataSource.");
						output.Append(columnName);
						output.Append(@";");
						break;

						////////////////////////////////////////
						// DATE
						////////////////////////////////////////
					case "DateTime":
						output.Append(columnName);
						output.Append(@".Date = dataSource.");
						output.Append(columnName);
						output.Append(@";");
						break;
				}			
			}

			output.Append(@"
			}
		}
		#endregion
");

			return output.ToString();
		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Render copy from form for code behind
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static string RenderCodeBehindCopyFromForm(DBTable table)
		{
			StringBuilder output = new StringBuilder();

			output.Append(@"
		#region CopyFromForm
		/////////////////////////////////////////////////////////////
		/// <summary>
		/// Save the object
		/// </summary>
		/////////////////////////////////////////////////////////////
		private void CopyFromForm()
		{
			" + table.TableName + @" dataSource = this.UserSession." + table.TableName + @";
");

			foreach(DBColumn col in table.Columns.Values)
			{
				string columnName = col.ColName;

				output.Append(@"
			// ");
				output.Append(columnName);
				output.Append(@"
			");

				// Control
				switch (col.Type)
				{
						////////////////////////////////////////
						// TEXT
						////////////////////////////////////////
					case "string":
						output.Append("dataSource.");
						output.Append(columnName);
						output.Append(@" = ");
						output.Append(columnName);
						output.Append(@".Text.Trim();");
						break;

						////////////////////////////////////////
						// NUMBER
						////////////////////////////////////////
					case "int":
					case "decimal":
						if (col.LookupTable == null)
						{
							output.Append("dataSource.");
							output.Append(columnName);
							output.Append(@" = ");
							output.Append(col.Type);
							output.Append(@".Parse(");
							output.Append(columnName);
							output.Append(@".Text.Trim());");
						}
						else
						{
							output.Append("dataSource.");
							output.Append(columnName);
							output.Append(@" = int.Parse(");
							output.Append(columnName);
							output.Append(@".SelectedValue);");
						}
						break;

						////////////////////////////////////////
						// BOOLEAN
						////////////////////////////////////////
					case "bool":
						output.Append("dataSource.");
						output.Append(columnName);
						output.Append(@" = ");
						output.Append(columnName);
						output.Append(@".Checked;");
						break;

						////////////////////////////////////////
						// DATE
						////////////////////////////////////////
					case "DateTime":
						output.Append("dataSource.");
						output.Append(columnName);
						output.Append(@" = ");
						output.Append(columnName);
						output.Append(@".Date;");
						break;
				}			
			}

			output.Append(@"
			
			if (!dataSource.Save())
				Utils.RaiseError(Context, dataSource.ErrorMessage);

		}
		#endregion
");

			return output.ToString();
		}

		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Render footer for code behind
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static string RenderCodeBehindFooter(DBTable table)
		{
			return @"

		protected void Submit_Click(object sender, EventArgs e)
		{
			if (Page.IsValid)
				CopyFromForm();
		}

	}
}";
		}
		#endregion

		#region List Page Infront
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Render header for list page infront
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static string RenderListPageInfront(DBTable table)
		{
			StringBuilder output = new StringBuilder();
			string tableName = table.TableName;

			output.Append(@"
<%@ Page Language=""C#"" MasterPageFile=""~/Master/MasterPage.master"" AutoEventWireup=""true"" CodeFile=""" + tableName + @"List.aspx.cs"" Inherits=""" + tableName + @"List_page"" Title=""Untitled Page"" %>
<%@ MasterType VirtualPath=""~/Master/MasterPage.master"" %>
<asp:Content ID=""Content1"" ContentPlaceHolderID=""MainContent"" Runat=""Server"">
	<h1>" + tableName + @" List</h1>
	<asp:Panel ID=""SearchPanel"" runat=""server"" DefaultButton=""SearchButton"">
		<fieldset>
			<p>
				<label for=""Search"">Search:</label><asp:TextBox ID=""Search"" runat=""server"" />
				<asp:Button Text=""Search"" ID=""SearchButton"" runat=""server"" OnClick=""SearchButton_Click"" />
			</p>
		        
		</fieldset>
	</asp:Panel>
    <asp:GridView id=""DatasourceList"" runat=""server"" CssClass=""DataGrid"" GridLines=""None""
        AutoGenerateColumns=""False"" AllowPaging=""true"" PageSize=""10""
        OnPageIndexChanging=""DatasourceList_PageIndexChanging"">
            <AlternatingRowStyle CssClass=""row"" />
            <RowStyle CssClass=""altrow"" />
            <HeaderStyle CssClass=""header"" />
            <EmptyDataTemplate>There were no results matching your search criteria</EmptyDataTemplate>
            <Columns>");

			foreach(DBColumn col in table.Columns.Values)
			{
				output.Append(@"
				");
       
				switch (col.Type)
				{
					case "string":
					case "bool":
					case "int":
					case "decimal":
						output.Append(@"<asp:BoundField DataField=""");
						output.Append(col.ColName);
						output.Append(@""" HeaderText=""");
						output.Append(col.ColName);
						output.Append(@""" />");
						break;
					
					case "DateTime":
						output.Append(@"<asp:BoundField HtmlEncode=""false"" DataField=""");
						output.Append(col.ColName);
						output.Append(@""" HeaderText=""");
						output.Append(col.ColName);
						output.Append(@"""  DataFormatString=""{0:dd/MMM/yyyy}"" />");
						break;
				}
			}

			output.Append(@"
				<asp:HyperLinkField DataNavigateUrlFields=""ID"" Text=""Edit"" ControlStyle-CssClass=""link"" 
                    DataNavigateUrlFormatString=""" + tableName + @".aspx?ID={0}"" />
            </Columns>
            <PagerStyle CssClass=""pager"" />
            <PagerSettings Mode=""NumericFirstLast"" Position=""Bottom"" />
	</asp:GridView>
</asp:Content>
");

		  return output.ToString();
		}
		#endregion

		#region List Page Behind
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Render header for list page behind
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public static string RenderListPageBehind(DBTable table)
		{
			StringBuilder output = new StringBuilder();



			return output.ToString();
		}
		#endregion

	}
}
