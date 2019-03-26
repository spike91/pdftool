using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfTool.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PdfTool.Helpers
{
    public class PdfHelper
    {
        public string ExportPdf(IEnumerable<Indicator> values, string filename)
        {
            var path = System.AppContext.BaseDirectory;
            filename = $"{filename}_{DateTime.Now.Date.Day}-{DateTime.Now.Date.Month}-{DateTime.Now.Date.Year}_{DateTime.Now.TimeOfDay.Hours}-{DateTime.Now.TimeOfDay.Minutes}-{DateTime.Now.TimeOfDay.Seconds}-{DateTime.Now.TimeOfDay.Milliseconds}";
            System.IO.FileStream fs = new FileStream($"{path}/App_Data/Temp/{filename}.pdf", FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            
            document.AddCreator("PdfTool app");
            document.AddSubject("Export Data");
            document.AddTitle("Indicators Report");

            document.Open();
            
            document.Close();
            writer.Close();
            fs.Close();

            return path;
        }
    }
}