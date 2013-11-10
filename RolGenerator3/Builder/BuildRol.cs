using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for BuildROL.
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class BuildRol
	{
		string _outputPath;

		public BuildRol(string outputPath)
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
		public void Build(ProgressBar pb,DBSchema schema, string nameSpace, out string errorMessages)
		{
			errorMessages = string.Empty;

            if (nameSpace != null && nameSpace != string.Empty)
                if (!nameSpace.EndsWith("."))
                    nameSpace += ".";

			foreach(DBTable table in schema.Tables.Values)
			{
				// Create File
				string fileName = table.TableName + ".cs";

				StreamWriter outputFile = new StreamWriter(_outputPath + @"\" + fileName,false);

				outputFile.Write(RolTemplate.RenderHeader(table, nameSpace));

				foreach(DBColumn col in table.SortedColumns().Values)
				{
					outputFile.Write(RolTemplate.RenderHeaderItem(col.ColName,col.Type));
				}

				// Add a line break
				outputFile.WriteLine();

				foreach(DBColumn col in table.SortedColumns().Values)
				{
					outputFile.Write(RolTemplate.RenderItem(col));
				}

				outputFile.Write(RolTemplate.RenderFooter(table));

				outputFile.Close();

				// progress marches on
				pb.PerformStep();
			}
		}
	}
}
