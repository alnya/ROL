using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Summary description for BuildSession.
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class BuildSession
	{
		string _outputPath;

		public BuildSession(string outputPath)
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

			// Create File
			string fileName = "SessionBag.cs";

			StreamWriter outputFile = new StreamWriter(_outputPath + @"\" + fileName,false);

			outputFile.Write(SessionTemplate.RenderHeader());

			outputFile.Write(SessionTemplate.RenderHeaderItems(schema));
			outputFile.Write(SessionTemplate.RenderPublicItems(schema));
			outputFile.Write(SessionTemplate.RenderClear(schema));

			outputFile.Write(SessionTemplate.RenderFooter());

			outputFile.Close();

			// progress marches on
			pb.PerformStep();
		}
	}
}
