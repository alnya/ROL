using System;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

namespace RolGenerator
{
	/////////////////////////////////////////////////////////////////
	/// <summary>
	/// Template for stored procedures
	/// </summary>
	/////////////////////////////////////////////////////////////////
	public class ProcTemplate
	{
		public static string RenderItem(DBTable table)
		{
			StringBuilder output = new StringBuilder();

			output.Append(@"
-- -------------------------------------------------------------------------
-- STORED PROCEDURES FOR TABLES AND VIEWS
-- Code generated on " + DateTime.Now.ToString("dd MMM yyyy") + @" at " + DateTime.Now.ToString("HH:mm") + @"
-- -------------------------------------------------------------------------

");

			string searchProcName = table.TableName + "_SEARCH";
			string loadProcName = table.TableName + "_LOAD";
			string addProcName = table.TableName + "_ADD";
			string editProcName = table.TableName + "_EDIT";
			string delProcName = table.TableName + "_DELETE";
			string delallProcName = table.TableName + "_DELETEALL";

			#region Search Procedure
			// Search proc
			output.Append(@"
-- -------------------------------------------------------------------------
--  Search " + table.TableName + @"
-- -------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'dbo." + searchProcName + @"') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo." + searchProcName + @"
GO

CREATE PROCEDURE dbo." + searchProcName + @"
    @where nvarchar(500),
    @orderBy nvarchar(256)
AS
    DECLARE @sql nvarchar(500)

    SET @sql = 'SELECT ");
			foreach(DBColumn col in table.SortedColumns().Values)
			{
				output.Append(col.ColName + ",");
			}
					
			output.Append(@"ID FROM " + table.TableName + @"'

    IF @where <> ''
        SET @sql = @sql + '  WHERE ' + @where
    
    IF @orderBy <> ''
        SET @sql = @sql + ' ORDER BY ' + @orderBy

    EXEC(@sql)
GO

");
			#endregion

			#region Load Record Procedure
			// Search proc
			output.Append(@"
-- -------------------------------------------------------------------------
--  Select " + table.TableName + @"
-- -------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'dbo." + loadProcName + @"') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo." + loadProcName + @"
GO

CREATE PROCEDURE dbo." + loadProcName + @"
  @ID Int
AS
    SELECT");
			foreach(DBColumn col in table.SortedColumns().Values)
			{
				output.Append(@"
		" + col.ColName + ",");
			}
					
			output.Append(@"
		ID
	FROM 
		" + table.TableName + @"
    WHERE 
		ID = @ID
GO

");
			#endregion

			if (!table.IsView)
			{
				#region Add Procedure
				output.Append(@"
-- -------------------------------------------------------------------------
--  Add " + table.TableName + @"
-- -------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'dbo." + addProcName + @"') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo." + addProcName + @"
GO

CREATE PROCEDURE dbo." + addProcName);
				foreach(DBColumn col in table.SortedColumns().Values)
				{
					output.Append(@"
	@" + col.ColName + " " + col.ColDBType + WriteLength(col) + @",");
				}
					
				output.Append(@"
    @ID int out
AS
	--- Insert the new row
    INSERT INTO " + table.TableName + @"
    (");
				foreach(DBColumn col in table.SortedColumns().Values)
				{
					output.Append(@"
		" + col.ColName + ",");
				}

				output.Remove(output.Length-1,1); // horrible hack to remove the last comma
					
				output.Append(@"
    )
    VALUES
    (");
				foreach(DBColumn col in table.SortedColumns().Values)
				{
					output.Append(@"
		@" + col.ColName + ",");
				}

				output.Remove(output.Length-1,1); // horrible hack to remove the last comma
					
				output.Append(@"
    );

	--- Get the ID
    SELECT @ID = @@IDENTITY
GO

");
				#endregion

				#region Edit Procedure
				output.Append(@"
-- -------------------------------------------------------------------------
--  Edit " + table.TableName + @"
-- -------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'dbo." + editProcName + @"') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo." + editProcName + @"
GO

CREATE PROCEDURE dbo." + editProcName);
				foreach(DBColumn col in table.SortedColumns().Values)
				{
					output.Append(@"
	@" + col.ColName + " " + col.ColDBType + WriteLength(col) + @",");
				}
					
				output.Append(@"
    @ID int,
	@RowsUpdated int out
AS
	--- Update the table
    UPDATE " + table.TableName + @"
    SET");
				foreach(DBColumn col in table.SortedColumns().Values)
				{
					output.Append(@"
		" + col.ColName + " = @" + col.ColName + ",");
				}

				output.Remove(output.Length-1,1); // horrible hack to remove the last comma
					
				output.Append(@"
	WHERE 
		ID = @ID;

     --- Find out how many rows were updated. 
    SELECT @RowsUpdated = @@ROWCOUNT;
GO

");
				#endregion
					
				#region Delete Procedure
				output.Append(@"
-- -------------------------------------------------------------------------
--  Delete " + table.TableName + @"
-- -------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'dbo." + delProcName + @"') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo." + delProcName + @"
GO

CREATE PROCEDURE dbo." + delProcName + @"
    @ID int,
    @RowsUpdated int out
AS
    --- Attempt the deletion.
    DELETE 
		FROM  " + table.TableName + @"
    WHERE
		ID = @ID;

    --- Find out how many rows were deleted
    SELECT @RowsUpdated = @@ROWCOUNT;

GO

");
				#endregion

				#region Delete All Procedure
				output.Append(@"
-- -------------------------------------------------------------------------
--  Delete All " + table.TableName + @"
-- -------------------------------------------------------------------------

if exists (select * from dbo.sysobjects where id = object_id(N'dbo." + delallProcName + @"') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo." + delallProcName + @"
GO

CREATE PROCEDURE dbo." + delallProcName + @"
    @where nvarchar(255),
    @RowsUpdated int OUT
AS
    DECLARE @sql nvarchar(500)

    SET @sql = 'DELETE FROM " + table.TableName + @"'

    IF @where <> ''
        SET @sql = @sql + '  WHERE ' + @where

    EXEC(@sql)

    SELECT @RowsUpdated = @@ROWCOUNT
GO

");
				#endregion
			}

			return output.ToString();
		}

		private static string WriteLength(DBColumn col)
		{
			switch(col.ColDBType)
			{
				case "nvarchar":
				case "varchar":
				case "char":
					return "(" + col.Length + ")";
			}

			return string.Empty;
		}
	}
}
