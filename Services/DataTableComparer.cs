using System.Data;

namespace Avrhil.BigCommerce.API.Services;

public class DataTableComparer
{
    public static DataTable GetDifferences(DataTable source, DataTable target, string[] keyColumns)
    {
        var differences = source.Clone();

        foreach (DataRow sourceRow in source.Rows)
        {
            var targetRow = FindRow(target, sourceRow, keyColumns);
            
            if (targetRow == null || !RowsAreEqual(sourceRow, targetRow))
            {
                differences.ImportRow(sourceRow);
            }
        }

        return differences;
    }

    public static DataTable GetNewRows(DataTable source, DataTable target, string[] keyColumns)
    {
        var newRows = source.Clone();

        foreach (DataRow sourceRow in source.Rows)
        {
            if (FindRow(target, sourceRow, keyColumns) == null)
            {
                newRows.ImportRow(sourceRow);
            }
        }

        return newRows;
    }

    public static DataTable GetUpdatedRows(DataTable source, DataTable target, string[] keyColumns)
    {
        var updatedRows = source.Clone();

        foreach (DataRow sourceRow in source.Rows)
        {
            var targetRow = FindRow(target, sourceRow, keyColumns);
            
            if (targetRow != null && !RowsAreEqual(sourceRow, targetRow))
            {
                updatedRows.ImportRow(sourceRow);
            }
        }

        return updatedRows;
    }

    public static DataTable GetDeletedRows(DataTable source, DataTable target, string[] keyColumns)
    {
        var deletedRows = target.Clone();

        foreach (DataRow targetRow in target.Rows)
        {
            if (FindRow(source, targetRow, keyColumns) == null)
            {
                deletedRows.ImportRow(targetRow);
            }
        }

        return deletedRows;
    }

    private static DataRow FindRow(DataTable table, DataRow row, string[] keyColumns)
    {
        foreach (DataRow tableRow in table.Rows)
        {
            bool match = true;
            foreach (var key in keyColumns)
            {
                if (!tableRow[key].Equals(row[key]))
                {
                    match = false;
                    break;
                }
            }
            if (match) return tableRow;
        }
        return null;
    }

    private static bool RowsAreEqual(DataRow row1, DataRow row2)
    {
        for (int i = 0; i < row1.Table.Columns.Count; i++)
        {
            if (!row1[i].Equals(row2[i]))
                return false;
        }
        return true;
    }
}
