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
	public class BuildWebServices
	{
		string _outputPath;
		string _codeBehindOutputPath;

		public BuildWebServices(string outputPath)
		{
			_outputPath = outputPath;
            _codeBehindOutputPath = _outputPath;// +"\\App_code\\Services\\";

			if (!System.IO.Directory.Exists(outputPath))
				Directory.CreateDirectory(outputPath);

			if (!System.IO.Directory.Exists(_codeBehindOutputPath))
				Directory.CreateDirectory(_codeBehindOutputPath);
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
				/////////////////////////////////////////////////////
				// Build Service Page in front
				/////////////////////////////////////////////////////
				string fileName = table.TableName + "Service.asmx";

				StreamWriter outputFile = new StreamWriter(_outputPath + @"\" + fileName,false);

				outputFile.Write(WebServiceTemplate.RenderServiceInfront(table, nameSpace));

				outputFile.Close();

				/////////////////////////////////////////////////////
				// Build Service Page behind
				/////////////////////////////////////////////////////
				fileName = table.TableName + "Service.asmx.cs";

				outputFile = new StreamWriter(_codeBehindOutputPath + @"\" + fileName,false);

				outputFile.Write(WebServiceTemplate.RenderServiceBehind(table, nameSpace));

				outputFile.Close();

				// progress marches on
				pb.PerformStep();
			}
		}
	}
}
