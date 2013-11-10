using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Template for dynamic views
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class ViewTemplate
	{
		public static string RenderItem(string DBase, DBTable table)
		{
			StringBuilder output = new StringBuilder();
			ArrayList lookupCols = new ArrayList(); 

			foreach(DBColumn col in table.SortedColumns().Values)
			{
				if (col.LookupTable != null && col.LookupColumn != null)
					lookupCols.Add(col);
			}

			if (lookupCols.Count > 0)
			{
				int lineCnt = 0;
				output.Append(@"
DROP VIEW IF EXISTS `" + DBase + @"`.`av_" + table.TableName + @"`;

-- ------------------------------------------------------------------
-- AV_" + table.TableName + @" generated View
-- ------------------------------------------------------------------

CREATE VIEW `av_" + table.TableName + @"` AS 
(
	SELECT
		`" + table.TableName + @"`.`ID` AS `ID`");

				// this tables cols
				foreach(DBColumn col in table.SortedColumns().Values)
				{
					if (lineCnt >= 0) output.Append(@",
");
					output.Append(@"		`" + table.TableName + @"`.`" + col.ColName + @"` AS `" + col.ColName + "`");
					lineCnt++;
				}
				// add all lookup tables
				foreach(DBColumn col in lookupCols)
				{
					if (lineCnt > 0) output.Append(@",
");
					output.Append(@"		`" + col.LookupTable.TableName + @"`.`" + col.LookupColumn.ColName + @"` AS `" + col.ColName.Replace("ID",string.Empty) + "`");
					lineCnt++;
				}
				output.Append(@"
	FROM `" + table.TableName + @"` 
");
				foreach(DBColumn col in lookupCols)
				{
					output.Append(@"	INNER JOIN `" + col.LookupTable.TableName + @"` ON `" + table.TableName + @"`.`" + col.ColName + @"` = `" + col.LookupTable.TableName + @"`.ID
");
				}
				output.Append(@");

");
			}

			return output.ToString();
		}
	}
}
