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
        public byte[] ExportPdf(IEnumerable<Indicator> values)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.AddCreator("PdfTool app");
                document.AddSubject("Export Data");
                document.AddTitle("Indicators Report");

                document.Open();
                document.Add(new Paragraph("Hello World!"));
                document.Close();
                writer.Close();

                return ms.GetBuffer();
            }
        }
    }
}