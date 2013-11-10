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
	public class BuildProcs
	{
		string _outputPath;

		public BuildProcs(string outputPath)
		{
			_outputPath = outputPath;
		}
		
		/////////////////////////////////////////////////////////////////
		/// <summary>
		/// Itterate thru the schema and apply the template
		/// </summary>
		/////////////////////////////////////////////////////////////////
		public void Build(ProgressBar pb,DBSchema schema, out string errorMessages)
		{
			errorMessages = string.Empty;

			// Create File
			string fileName = "package.sql";

			StreamWriter outputFile = new StreamWriter(_outputPath + @"\" + fileName,false);

			foreach(DBTable table in schema.Tables.Values)
			{
				// output SQL for table
				outputFile.Write(ProcTemplate.RenderItem(table));

				// progress marches on
				pb.PerformStep();
			}

			outputFile.Close();
		}
	}
}
