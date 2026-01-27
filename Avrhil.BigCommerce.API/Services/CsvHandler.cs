using System.Data;
using System.Text;

namespace Avrhil.BigCommerce.API.Services;

public class CsvHandler
{
    public static DataTable ReadCsv(string filePath, bool hasHeader = true)
    {
        var dataTable = new DataTable();
        var lines = File.ReadAllLines(filePath);
        
        if (lines.Length == 0) return dataTable;

        var headers = lines[0].Split(',');
        foreach (var header in headers)
        {
            dataTable.Columns.Add(hasHeader ? header.Trim() : $"Column{dataTable.Columns.Count}");
        }

        var startIndex = hasHeader ? 1 : 0;
        for (int i = startIndex; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');
            dataTable.Rows.Add(values.Select(v => v.Trim()).ToArray());
        }

        return dataTable;
    }

    public static void WriteCsv(DataTable dataTable, string filePath, bool includeHeader = true)
    {
        var csv = new StringBuilder();

        if (includeHeader)
        {
            csv.AppendLine(string.Join(",", dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName)));
        }

        foreach (DataRow row in dataTable.Rows)
        {
            csv.AppendLine(string.Join(",", row.ItemArray.Select(v => v?.ToString() ?? string.Empty)));
        }

        File.WriteAllText(filePath, csv.ToString());
    }

    public static DataTable ConvertToDataTable<T>(List<T> items)
    {
        var dataTable = new DataTable(typeof(T).Name);
        var properties = typeof(T).GetProperties();

        foreach (var prop in properties)
        {
            dataTable.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        }

        foreach (var item in items)
        {
            var values = properties.Select(p => p.GetValue(item)).ToArray();
            dataTable.Rows.Add(values);
        }

        return dataTable;
    }

    public static List<T> ConvertFromDataTable<T>(DataTable dataTable) where T : new()
    {
        var list = new List<T>();
        var properties = typeof(T).GetProperties();

        foreach (DataRow row in dataTable.Rows)
        {
            var item = new T();
            foreach (var prop in properties)
            {
                if (dataTable.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                {
                    prop.SetValue(item, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                }
            }
            list.Add(item);
        }

        return list;
    }
}
