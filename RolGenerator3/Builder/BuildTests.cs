using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for BuildTests.
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class BuildTests
	{
		string _outputPath;

		public BuildTests(string outputPath)
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
				// Create File
				string fileName = table.TableName + ".cs";

				StreamWriter outputFile = new StreamWriter(_outputPath + @"\" + fileName,false);

				outputFile.Write(NUnitTemplate.RenderHeader(table));
				outputFile.WriteLine();

				outputFile.Write(NUnitTemplate.RenderSearchTest(table));
				outputFile.WriteLine();

				if (!table.IsView)
				{
					outputFile.Write(NUnitTemplate.RenderAddTest(table));
					outputFile.WriteLine();

					outputFile.Write(NUnitTemplate.RenderEditTest(table));
					outputFile.WriteLine();

					outputFile.Write(NUnitTemplate.RenderLoadTest(table));
					outputFile.WriteLine();

					outputFile.Write(NUnitTemplate.RenderDeleteTest(table));
					outputFile.WriteLine();
				}

				outputFile.Write(NUnitTemplate.RenderFooter());
				outputFile.WriteLine();

				outputFile.Close();

				// progress marches on
				pb.PerformStep();
			}
		}
	}
}
