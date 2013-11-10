using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for BuildViews.
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class BuildViews
	{
		string _outputPath;

		public BuildViews(string outputPath)
		{
			_outputPath = outputPath;
		}
		
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Itterate thru the schema and apply the template
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public void Build(string DBName, ProgressBar pb,DBSchema schema, out string errorMessages)
		{
			errorMessages = string.Empty;

			// Create File
			string fileName = "generatedViews.sql";

			StreamWriter outputFile = new StreamWriter(_outputPath + @"\" + fileName,false);

			foreach(DBTable table in schema.Tables.Values)
			{
				// only process tables
				if (!table.IsView)
				{
					outputFile.Write(ViewTemplate.RenderItem(DBName,table));
				}

				// progress marches on
				pb.PerformStep();
			}

			outputFile.Close();
		}
	}
}
