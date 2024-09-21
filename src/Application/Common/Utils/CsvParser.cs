using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace ConvenienceStoreApi.Application.Common.Utils;

public static class CsvParser
{
    public static List<T> ParseCsvFile<T>(string csvFilePath)
    {
        List<T> data = new List<T>();

        using (var reader = new StreamReader(csvFilePath, Encoding.GetEncoding("iso-8859-1")))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.Configuration.HeaderValidated = null;
            csv.Configuration.MissingFieldFound = null;
            data = csv.GetRecords<T>().ToList();
        }
        return data;
    }
}