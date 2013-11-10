using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for BuildControls.
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class BuildControls
	{
		string _outputPath;

		public BuildControls(string outputPath)
		{
			_outputPath = outputPath;

			if (!System.IO.Directory.Exists(outputPath))
				Directory.CreateDirectory(outputPath);
		}
		
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Itterate thru the schema and apply the template
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public void Build(ProgressBar pb,DBSchema schema, out string errorMessages)
		{
			errorMessages = string.Empty;

			foreach(DBTable table in schema.Tables.Values)
			{
				/////////////////////////////////////////////////////
				// Build List Page in front
				/////////////////////////////////////////////////////
				string fileName = table.TableName + "List.aspx";

				StreamWriter outputFile = new StreamWriter(_outputPath + @"\" + fileName,false);

				outputFile.Write(ControlTemplate.RenderListPageInfront(table));

				outputFile.Close();

				/////////////////////////////////////////////////////
				// Build List Page behind
				/////////////////////////////////////////////////////
				fileName = table.TableName + "List.aspx.cs";

				outputFile = new StreamWriter(_outputPath + @"\" + fileName,false);

				outputFile.Write(ControlTemplate.RenderListPageBehind(table));

				outputFile.Close();

				if(!table.IsView)
				{
					/////////////////////////////////////////////////////
					// Page Infront
					/////////////////////////////////////////////////////
					fileName = table.TableName + ".aspx";

					outputFile = new StreamWriter(_outputPath + @"\" + fileName,false);

					outputFile.Write(ControlTemplate.RenderPageInfrontHeader(table,false));

					foreach(DBColumn col in table.Columns.Values)
					{
						// no one cares about ID but me
						if (col.ColName.ToLower() != "id")
							outputFile.Write(ControlTemplate.RenderPageInfrontItem(col));
					}

					outputFile.Write(ControlTemplate.RenderPageInfrontFooter(table));

					outputFile.Close();

					/////////////////////////////////////////////////////
					// Code Behind
					/////////////////////////////////////////////////////
					fileName = table.TableName + ".aspx.cs";

					outputFile = new StreamWriter(_outputPath + @"\" + fileName,false);

					outputFile.Write(ControlTemplate.RenderCodeBehindHeader(table));

					outputFile.Write(ControlTemplate.RenderCodeBehindCopyToForm(table));
					outputFile.Write(ControlTemplate.RenderCodeBehindCopyFromForm(table));

					outputFile.Write(ControlTemplate.RenderCodeBehindFooter(table));

					outputFile.Close();
				}

				// progress marches on
				pb.PerformStep();
			}
		}
	}
}
