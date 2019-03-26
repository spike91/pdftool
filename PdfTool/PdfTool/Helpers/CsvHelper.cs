using PdfTool.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace PdfTool.Helpers
{
    public class CsvHelper
    {
        public IEnumerable<Indicator> ReadFile(string filePath) {
            List<Indicator> result = new List<Indicator>();
            using (var reader = new StreamReader(filePath))
            {
                if(!reader.EndOfStream)
                    reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    var indicator = new Indicator();
                    indicator.ServerName = values[0];
                    indicator.TagName = values[1];
                    indicator.Value = values[2];
                    indicator.Date = DateTime.ParseExact(values[3], "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    indicator.Questionable = string.Equals(values[4], "true", StringComparison.OrdinalIgnoreCase);
                    indicator.Annotated = values[5];
                    indicator.Substituted = string.Equals(values[6], "true", StringComparison.OrdinalIgnoreCase);

                    result.Add(indicator);
                }
            }

            return result;
        }
    }
}