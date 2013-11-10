using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Data;
using SourceGrid3;

namespace RolGenerator
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainStage : System.Windows.Forms.Form
	{
		#region Form Designer Elements
		private System.Windows.Forms.Button generateButton;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox chkViews;
		private System.Windows.Forms.CheckBox chkSession;
		private System.Windows.Forms.CheckBox chkControls;
		private System.Windows.Forms.CheckBox chkROL;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.TextBox outputPath;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox databaseTables;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox connectionPort;
        private System.Windows.Forms.Button connectButton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox connectionPassword;
		private System.Windows.Forms.TextBox connectionUsername;
		private System.Windows.Forms.TextBox connectionServer;
		private System.Windows.Forms.TabPage tabTableDetails;
		private System.Windows.Forms.TreeView tablesTree;
		private SourceGrid3.Grid ColumnGrid;
		private System.Windows.Forms.CheckBox chkProcs;
		private System.Windows.Forms.CheckBox chkTest;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox savedConnections;
		#endregion
        private System.Windows.Forms.CheckBox chkWebServices;
        private GroupBox groupBox3;
        private TextBox txtNameSpace;
        private Label label8;
        private ComboBox databaseType;
        private IContainer components;

		public MainStage()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			PopulatePreviousConnections();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("No database selected");
            this.generateButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.outputPath = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtNameSpace = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkWebServices = new System.Windows.Forms.CheckBox();
            this.chkTest = new System.Windows.Forms.CheckBox();
            this.chkProcs = new System.Windows.Forms.CheckBox();
            this.chkViews = new System.Windows.Forms.CheckBox();
            this.chkSession = new System.Windows.Forms.CheckBox();
            this.chkControls = new System.Windows.Forms.CheckBox();
            this.chkROL = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.databaseTables = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.databaseType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.connectionPort = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.connectionPassword = new System.Windows.Forms.TextBox();
            this.connectionUsername = new System.Windows.Forms.TextBox();
            this.connectionServer = new System.Windows.Forms.TextBox();
            this.savedConnections = new System.Windows.Forms.ComboBox();
            this.tabTableDetails = new System.Windows.Forms.TabPage();
            this.ColumnGrid = new SourceGrid3.Grid();
            this.tablesTree = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabTableDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(440, 312);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.TabIndex = 12;
            this.generateButton.Text = "Generate";
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // outputPath
            // 
            this.errorProvider1.SetIconPadding(this.outputPath, 70);
            this.outputPath.Location = new System.Drawing.Point(112, 240);
            this.outputPath.Name = "outputPath";
            this.outputPath.ReadOnly = true;
            this.outputPath.Size = new System.Drawing.Size(176, 20);
            this.outputPath.TabIndex = 18;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(8, 316);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(416, 16);
            this.progressBar.Step = 1;
            this.progressBar.TabIndex = 13;
            this.progressBar.Value = 1;
            this.progressBar.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabTableDetails);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(531, 304);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.browseButton);
            this.tabPage1.Controls.Add(this.outputPath);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.databaseTables);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(523, 278);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Database";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtNameSpace);
            this.groupBox3.Location = new System.Drawing.Point(368, 220);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(144, 43);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Namespace";
            // 
            // txtNameSpace
            // 
            this.txtNameSpace.Location = new System.Drawing.Point(8, 16);
            this.txtNameSpace.Name = "txtNameSpace";
            this.txtNameSpace.Size = new System.Drawing.Size(128, 20);
            this.txtNameSpace.TabIndex = 0;
            this.txtNameSpace.Text = "RelationalObjectLayer";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkWebServices);
            this.groupBox2.Controls.Add(this.chkTest);
            this.groupBox2.Controls.Add(this.chkProcs);
            this.groupBox2.Controls.Add(this.chkViews);
            this.groupBox2.Controls.Add(this.chkSession);
            this.groupBox2.Controls.Add(this.chkControls);
            this.groupBox2.Controls.Add(this.chkROL);
            this.groupBox2.Location = new System.Drawing.Point(368, 16);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(144, 198);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Generate";
            // 
            // chkWebServices
            // 
            this.chkWebServices.Location = new System.Drawing.Point(8, 168);
            this.chkWebServices.Name = "chkWebServices";
            this.chkWebServices.Size = new System.Drawing.Size(96, 24);
            this.chkWebServices.TabIndex = 6;
            this.chkWebServices.Text = "Web Services";
            // 
            // chkTest
            // 
            this.chkTest.Location = new System.Drawing.Point(8, 144);
            this.chkTest.Name = "chkTest";
            this.chkTest.Size = new System.Drawing.Size(96, 24);
            this.chkTest.TabIndex = 5;
            this.chkTest.Text = "Unit Tests";
            // 
            // chkProcs
            // 
            this.chkProcs.Location = new System.Drawing.Point(8, 120);
            this.chkProcs.Name = "chkProcs";
            this.chkProcs.Size = new System.Drawing.Size(96, 24);
            this.chkProcs.TabIndex = 4;
            this.chkProcs.Text = "Procedures";
            // 
            // chkViews
            // 
            this.chkViews.Location = new System.Drawing.Point(8, 94);
            this.chkViews.Name = "chkViews";
            this.chkViews.Size = new System.Drawing.Size(128, 24);
            this.chkViews.TabIndex = 3;
            this.chkViews.Text = "Foreign Key Views";
            // 
            // chkSession
            // 
            this.chkSession.Location = new System.Drawing.Point(8, 70);
            this.chkSession.Name = "chkSession";
            this.chkSession.Size = new System.Drawing.Size(80, 24);
            this.chkSession.TabIndex = 2;
            this.chkSession.Text = "Session";
            // 
            // chkControls
            // 
            this.chkControls.Location = new System.Drawing.Point(8, 46);
            this.chkControls.Name = "chkControls";
            this.chkControls.Size = new System.Drawing.Size(80, 24);
            this.chkControls.TabIndex = 1;
            this.chkControls.Text = "Pages";
            // 
            // chkROL
            // 
            this.chkROL.Checked = true;
            this.chkROL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkROL.Location = new System.Drawing.Point(8, 22);
            this.chkROL.Name = "chkROL";
            this.chkROL.Size = new System.Drawing.Size(80, 24);
            this.chkROL.TabIndex = 0;
            this.chkROL.Text = "ROL";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(16, 240);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "Output Folder:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // browseButton
            // 
            this.browseButton.Location = new System.Drawing.Point(296, 240);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(56, 23);
            this.browseButton.TabIndex = 19;
            this.browseButton.Text = "Browse";
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click_1);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 208);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 16);
            this.label2.TabIndex = 17;
            this.label2.Text = "Database:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // databaseTables
            // 
            this.databaseTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.databaseTables.Location = new System.Drawing.Point(112, 208);
            this.databaseTables.Name = "databaseTables";
            this.databaseTables.Size = new System.Drawing.Size(176, 21);
            this.databaseTables.TabIndex = 16;
            this.databaseTables.SelectedIndexChanged += new System.EventHandler(this.databaseTables_SelectedIndexChanged_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.databaseType);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.connectionPort);
            this.groupBox1.Controls.Add(this.connectButton);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.connectionPassword);
            this.groupBox1.Controls.Add(this.connectionUsername);
            this.groupBox1.Controls.Add(this.connectionServer);
            this.groupBox1.Controls.Add(this.savedConnections);
            this.groupBox1.Location = new System.Drawing.Point(16, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(336, 184);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection Settings";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 16);
            this.label8.TabIndex = 24;
            this.label8.Text = "Product:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // databaseType
            // 
            this.databaseType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.databaseType.FormattingEnabled = true;
            this.databaseType.Items.AddRange(new object[] {
            "MySQL",
            "SQL Server",
            "Oracle"});
            this.databaseType.Location = new System.Drawing.Point(88, 155);
            this.databaseType.Name = "databaseType";
            this.databaseType.Size = new System.Drawing.Size(121, 21);
            this.databaseType.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 16);
            this.label7.TabIndex = 14;
            this.label7.Text = "Saved Connections:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(8, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 16);
            this.label6.TabIndex = 13;
            this.label6.Text = "Port:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // connectionPort
            // 
            this.connectionPort.Location = new System.Drawing.Point(88, 128);
            this.connectionPort.Name = "connectionPort";
            this.connectionPort.Size = new System.Drawing.Size(48, 20);
            this.connectionPort.TabIndex = 12;
            this.connectionPort.Text = "3306";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(248, 152);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 11;
            this.connectButton.Text = "Connect";
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 8;
            this.label4.Text = "Password:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(8, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "Username:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Server:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // connectionPassword
            // 
            this.connectionPassword.Location = new System.Drawing.Point(88, 104);
            this.connectionPassword.Name = "connectionPassword";
            this.connectionPassword.PasswordChar = '*';
            this.connectionPassword.Size = new System.Drawing.Size(120, 20);
            this.connectionPassword.TabIndex = 4;
            // 
            // connectionUsername
            // 
            this.connectionUsername.Location = new System.Drawing.Point(88, 80);
            this.connectionUsername.Name = "connectionUsername";
            this.connectionUsername.Size = new System.Drawing.Size(120, 20);
            this.connectionUsername.TabIndex = 3;
            // 
            // connectionServer
            // 
            this.connectionServer.Location = new System.Drawing.Point(88, 56);
            this.connectionServer.Name = "connectionServer";
            this.connectionServer.Size = new System.Drawing.Size(120, 20);
            this.connectionServer.TabIndex = 1;
            // 
            // savedConnections
            // 
            this.savedConnections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.savedConnections.Location = new System.Drawing.Point(128, 22);
            this.savedConnections.Name = "savedConnections";
            this.savedConnections.Size = new System.Drawing.Size(198, 21);
            this.savedConnections.TabIndex = 22;
            this.savedConnections.SelectedIndexChanged += new System.EventHandler(this.savedConnections_SelectedIndexChanged);
            // 
            // tabTableDetails
            // 
            this.tabTableDetails.Controls.Add(this.ColumnGrid);
            this.tabTableDetails.Controls.Add(this.tablesTree);
            this.tabTableDetails.Location = new System.Drawing.Point(4, 22);
            this.tabTableDetails.Name = "tabTableDetails";
            this.tabTableDetails.Size = new System.Drawing.Size(523, 278);
            this.tabTableDetails.TabIndex = 1;
            this.tabTableDetails.Text = "Table Details";
            // 
            // ColumnGrid
            // 
            this.ColumnGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ColumnGrid.GridToolTipActive = true;
            this.ColumnGrid.Location = new System.Drawing.Point(152, 0);
            this.ColumnGrid.Name = "ColumnGrid";
            this.ColumnGrid.Size = new System.Drawing.Size(371, 278);
            this.ColumnGrid.SpecialKeys = ((SourceGrid3.GridSpecialKeys)(((((((SourceGrid3.GridSpecialKeys.Arrows | SourceGrid3.GridSpecialKeys.Tab)
                        | SourceGrid3.GridSpecialKeys.PageDownUp)
                        | SourceGrid3.GridSpecialKeys.Enter)
                        | SourceGrid3.GridSpecialKeys.Escape)
                        | SourceGrid3.GridSpecialKeys.Control)
                        | SourceGrid3.GridSpecialKeys.Shift)));
            this.ColumnGrid.StyleGrid = null;
            this.ColumnGrid.TabIndex = 1;
            // 
            // tablesTree
            // 
            this.tablesTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.tablesTree.Location = new System.Drawing.Point(0, 0);
            this.tablesTree.Name = "tablesTree";
            treeNode2.Name = "";
            treeNode2.Text = "No database selected";
            this.tablesTree.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.tablesTree.Size = new System.Drawing.Size(152, 278);
            this.tablesTree.TabIndex = 0;
            this.tablesTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tablesTree_AfterSelect);
            // 
            // MainStage
            // 
            this.AcceptButton = this.generateButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(531, 344);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.generateButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainStage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generator";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabTableDetails.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new MainStage());
		}

		DBSchema schema = new DBSchema();
		DBTable currentTable =null;
		Connection conn = null;

		#region Validation Events
		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Validate Server
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void connectionServer_Validated(object sender, System.EventArgs e)
		{
			ValidateItem(connectionServer,"Please enter the server to connect to");
		}
		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Validate Uername
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void connectionUsername_Validated(object sender, System.EventArgs e)
		{
			ValidateItem(connectionUsername,"Please enter the username");
		}
		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Validate Password
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void connectionPassword_Validated(object sender, System.EventArgs e)
		{
			ValidateItem(connectionPassword,"Please enter your password");
		}
		#endregion

		#region ValidationFunctions
		
		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Validate Control
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private bool ValidateItem(TextBox o, string message)
		{
			if (o.Text == string.Empty)
			{
				errorProvider1.SetError (o,message);
				return false;
			}
			else
			{
				errorProvider1.SetError (o,string.Empty);
				return true;
			}
		}
		
		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Validate Form
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private bool ValidateForm(bool requireConnection)
		{
			bool valid = true;

			if(valid)
				valid = ValidateItem(connectionServer,"Please enter the server to connect to");
			if(valid)
				valid = ValidateItem(connectionUsername,"Please enter the username");
			if(valid)
				valid = ValidateItem(connectionPassword,"Please enter your password");
			
			// running output
			if (requireConnection)
			{
				if(valid)
					valid = ValidateItem(outputPath,"Please browse for an output folder");
				if(valid)
				{
					// check database
					if ((string)databaseTables.SelectedItem == string.Empty)
					{
						MessageBox.Show("Please select an output database");
						valid = false;
					}
				}
			}

			return valid;
		}
		#endregion

		#region Functions
		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Get Lookups
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void LoadDatabase()
		{
			// get databases on server
			string errorMessage;

			Cursor.Current = Cursors.WaitCursor;

			try
			{
                conn = new Connection(databaseType.SelectedItem.ToString(), connectionServer.Text, connectionUsername.Text, connectionPassword.Text, connectionPort.Text);

				DBFunctions dbFunc = new DBFunctions(conn);
					
				databaseTables.DataSource = dbFunc.GetDatabases(out errorMessage);

				if (errorMessage != string.Empty)
					MessageBox.Show(errorMessage);

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			Cursor.Current = Cursors.Default;
		}

		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Load Table data
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void LoadDatabaseTableList()
		{
			string errorMessage;

			Cursor.Current = Cursors.WaitCursor;

			try
			{
				DBFunctions dbFunc = new DBFunctions(conn);
					
				// Populate collection from db
                string databaseTable = string.Empty;
                if (databaseTables.Enabled)
                    databaseTable = databaseTables.SelectedItem.ToString();

                schema = dbFunc.GetSchema(databaseTable, out errorMessage);		

				if (errorMessage != string.Empty)
				{
					MessageBox.Show(errorMessage);
				}
				else
				{

					// create the connection directory
					if (!Directory.Exists(Application.StartupPath + @"\Connections"))
						Directory.CreateDirectory(Application.StartupPath + @"\Connections");

					// save connection
					conn.Save(Application.StartupPath + @"\Connections\" + 
						conn.Server.Replace("\\",string.Empty) + "_" + conn.Username + ".xml");

					// create the schema directory
					if (!Directory.Exists(Application.StartupPath + @"\Schema"))
						Directory.CreateDirectory(Application.StartupPath + @"\Schema");

					// try a load
                    string schemaName = conn.Username;
                    if (schema.DatabaseName != null && schema.DatabaseName != string.Empty)
                        schemaName = schema.DatabaseName;

                    DBSchema savedSchema = DBSchema.Load(Application.StartupPath + @"\Schema\" + schemaName + ".xml");

					// If we have a saved schema, do a comparison
					if (savedSchema != null)
						schema = dbFunc.CompareSchemas(schema,savedSchema);
				
					// save schema
                    schema.Save(Application.StartupPath + @"\Schema\" + schemaName + ".xml");

					// Get Lookups
					dbFunc.GetLookupTables(schema);

					tablesTree.Nodes.Clear();
					TreeNode parentNode = new TreeNode("Tables");
					parentNode.Expand();
					tablesTree.Nodes.Add(parentNode);

					foreach(DBTable table in schema.SortedTables().Values)
						parentNode.Nodes.Add(new TreeNode(table.TableName));
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			Cursor.Current = Cursors.Default;
		}

		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Load table into viewer grid
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void LoadDatabaseTable(string tableName)
		{
			DBTable table = (DBTable)schema.Tables[tableName];

			if (table != null)
			{
				AmendSchema(schema);
				currentTable = table;

				ColumnGrid.Rows.Clear();
				ColumnGrid.BorderStyle = BorderStyle.FixedSingle;
				ColumnGrid.ColumnsCount = 4;
				ColumnGrid.FixedRows = 1;
				ColumnGrid.Rows.Insert(0);
				ColumnGrid[0,0] = new SourceGrid3.Cells.Real.ColumnHeader("Name");
				ColumnGrid[0,1] = new SourceGrid3.Cells.Real.ColumnHeader("Type");
				ColumnGrid[0,2] = new SourceGrid3.Cells.Real.ColumnHeader("Lookup Table");
				ColumnGrid[0,3] = new SourceGrid3.Cells.Real.ColumnHeader("Lookup Column");
				int r = 1;
				foreach (DBColumn col in table.SortedColumns().Values)
				{
					ColumnGrid.Rows.Insert(r);
					ColumnGrid[r,0] = new SourceGrid3.Cells.Real.Cell(col.ColName, typeof(string));
					ColumnGrid[r,1] = new SourceGrid3.Cells.Real.Cell(col.ColDBType, typeof(string));
					if (col.LookupTable != null)
					{
						ColumnGrid[r,2] = new SourceGrid3.Cells.Real.Cell(col.LookupTable.TableName, typeof(string));

						ArrayList lookupList = new ArrayList();
						foreach(DBColumn lcol in col.LookupTable.Columns.Values)
							lookupList.Add(lcol.ColName);
						SourceGrid3.Cells.Editors.ComboBox comboLookup = new SourceGrid3.Cells.Editors.ComboBox(typeof(string), lookupList, false);
						
						string selected = string.Empty;
						if (col.LookupColumn != null) selected = col.LookupColumn.ColName;

						ColumnGrid[r, 3] = new SourceGrid3.Cells.Real.Cell(selected, comboLookup);
					}
					r++;
				}
				ColumnGrid.AutoSize();
				
			}
		}

		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Get the current table and amend lookup details
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void AmendSchema(DBSchema schema)
		{
			if (currentTable != null)
			{
				DBTable table = (DBTable)schema.Tables[currentTable.TableName];
				
				if (table != null)
				{
					for(int i = 1; i<ColumnGrid.RowsCount;i++)
					{
						string colName = (string)ColumnGrid[i,0].Value;
						DBColumn column = (DBColumn)table.Columns[colName];

						if (column.LookupTable != null)
						{
							if (ColumnGrid[i,3] != null)
							{
								string lookupColName = (string)ColumnGrid[i,3].Value;
								column.LookupColumn = (DBColumn)column.LookupTable.Columns[lookupColName];
							}
						}
					}
				}
			}

			// finally, save the schema
            string schemaName = conn.Username;
            if (schema.DatabaseName != null && schema.DatabaseName != string.Empty)
                schemaName = schema.DatabaseName;

            schema.Save(Application.StartupPath + @"\Schema\" + schemaName + ".xml");
		}

		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Generate output
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void Generate()
		{
			Cursor.Current = Cursors.WaitCursor;

            // namespace
            string nameSpace = txtNameSpace.Text;

			// get databases on server
			string errorMessage = string.Empty;

			// Setup Progress Bar
			progressBar.Value = 1;
			progressBar.Visible = true;

			try
			{				
				if (schema == null)
				{
					MessageBox.Show("Please select a database");
				}
				else
				{
					AmendSchema(schema);

					progressBar.Maximum = 0;

					if (chkROL.Checked)
						progressBar.Maximum+=(schema.Tables.Count);
					if (chkViews.Checked)
						progressBar.Maximum+=(schema.Tables.Count);
					if (chkControls.Checked)
						progressBar.Maximum+=(schema.Tables.Count);
					if (chkProcs.Checked)
						progressBar.Maximum+=(schema.Tables.Count);
					if (chkTest.Checked)
						progressBar.Maximum+=(schema.Tables.Count);
					if (chkWebServices.Checked)
						progressBar.Maximum+=(schema.Tables.Count);
					if (chkSession.Checked)
						progressBar.Maximum+=1;
						
					// Create ROL
					if (chkROL.Checked)
					{
						BuildRol builder = new BuildRol(outputPath.Text + @"\ROL\");
						builder.Build(progressBar,schema, nameSpace, out errorMessage);
					}

					// Create Views
					if (chkViews.Checked)
					{
						BuildViews builder = new BuildViews(outputPath.Text + @"\");
						builder.Build(databaseTables.SelectedItem.ToString(),progressBar,schema, out errorMessage);
					}

					// Create Pages
					if (chkControls.Checked)
					{
						BuildControls builder = new BuildControls(outputPath.Text + @"\Pages\");
						builder.Build(progressBar,schema, out errorMessage);
					}

					// Create Stored Procedures
					if (chkProcs.Checked)
					{
						BuildProcs builder = new BuildProcs(outputPath.Text + @"\");
						builder.Build(progressBar,schema, out errorMessage);
					}

					// Create Stored Procedures
					if (chkTest.Checked)
					{
						BuildTests builder = new BuildTests(outputPath.Text + @"\UnitTests\");
						builder.Build(progressBar,schema, out errorMessage);
					}

					// Create Web Services
					if (chkWebServices.Checked)
					{
						BuildWebServices builder = new BuildWebServices(outputPath.Text + @"\WebServices\");
						builder.Build(progressBar,schema, nameSpace, out errorMessage);
					}

					// Create Session bag
					if (chkSession.Checked)
					{
						BuildSession builder = new BuildSession(outputPath.Text + @"\Session");
						builder.Build(progressBar,schema, out errorMessage);
					}
				}

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			Cursor.Current = Cursors.Default;
		}
		#endregion

		private void PopulatePreviousConnections()
		{
			if (Directory.Exists(Application.StartupPath + @"\Connections\"))
			{
				string[] files = Directory.GetFiles(Application.StartupPath + @"\Connections\","*.xml");
					
				foreach(string filepath in files)
				{
					FileInfo file = new FileInfo(filepath);

					savedConnections.Items.Add(file.Name.Replace(".xml",string.Empty));
				}
			}
		}

		#region Event Handers
		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Build the ROL
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void generateButton_Click(object sender, System.EventArgs e)
		{
			// Generate ROL
			if (ValidateForm(true))
			{
				Generate();				
			}
		}

		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Open the Database connection and get tables
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void connectButton_Click(object sender, System.EventArgs e)
		{
			if (ValidateForm(false))
			{
				LoadDatabase();	
			
				// can only generate stored procedures in SQL Server
                chkProcs.Enabled = (databaseType.SelectedItem.ToString() == "SQL Server");
                databaseTables.Enabled = databaseType.SelectedItem.ToString() != "Oracle";

                if (databaseType.SelectedItem.ToString() == "Oracle")
                {
                    LoadDatabaseTableList(); // no multiple databases
                }
			}
		}

		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Open path chooser
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void browseButton_Click_1(object sender, System.EventArgs e)
		{
			if((folderBrowserDialog1.ShowDialog() == DialogResult.OK))
			{
				outputPath.Text = folderBrowserDialog1.SelectedPath;
			}
		}

		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Load the table browser
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void databaseTables_SelectedIndexChanged_1(object sender, System.EventArgs e)
		{
			LoadDatabaseTableList();
		}

		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Select a table within a database and load into the grid
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void tablesTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			LoadDatabaseTable(e.Node.Text);
		}

		///////////////////////////////////////////////////////////////////
		/// <summary>
		/// Find a saved connection and populate the fields
		/// </summary>
		///////////////////////////////////////////////////////////////////
		private void savedConnections_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Connection conn = Connection.Load(Application.StartupPath + @"\Connections\" + savedConnections.SelectedItem + ".xml");

			if (conn != null)
			{
                databaseType.SelectedItem = conn.Type;
				connectionServer.Text = conn.Server;
				connectionUsername.Text = conn.Username;
				connectionPassword.Text = conn.Password;
				connectionPort.Text = conn.Port;
			}
		}
		#endregion
	}
}
