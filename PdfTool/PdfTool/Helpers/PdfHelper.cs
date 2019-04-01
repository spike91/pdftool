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
        public byte[] ExportPdf(string gname, int items, string reportPeriod, string maxVal, string minVal, string avgVal, string mostFrequentVal)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.AddCreator("PdfTool app");
                document.AddSubject("Export Data");
                document.AddTitle("Report of generator temperatures");

                document.Open();
                Paragraph elem = new Paragraph("Report");
                elem.Alignment = Element.ALIGN_CENTER;
                document.Add(elem);
                elem = new Paragraph("of generator temperatures");
                elem.Alignment = Element.ALIGN_CENTER;
                elem.SpacingAfter = 40;
                document.Add(elem);
                elem = new Paragraph($"Generator name: {gname}");
                elem.Alignment = Element.ALIGN_LEFT;
                elem.SpacingAfter = 10;
                document.Add(elem);
                elem = new Paragraph($"Report date: {DateTime.Now.ToShortDateString()}");
                elem.Alignment = Element.ALIGN_LEFT;
                elem.SpacingAfter = 10;
                document.Add(elem);
                elem = new Paragraph($"Selection period: {reportPeriod}");
                elem.Alignment = Element.ALIGN_LEFT;
                elem.SpacingAfter = 10;
                document.Add(elem);
                elem = new Paragraph($"Number of entries: {items}");
                elem.Alignment = Element.ALIGN_LEFT;
                elem.SpacingAfter = 10;
                document.Add(elem);

                elem = new Paragraph($"Report data");
                elem.Alignment = Element.ALIGN_CENTER;
                elem.SpacingAfter = 10;
                elem.SpacingBefore = 120;
                document.Add(elem);

                PdfPTable table = new PdfPTable(2);
                table.DefaultCell.Border = iTextSharp.text.Rectangle.ALIGN_CENTER;
                table.TotalWidth = 300;
                table.SetWidths(new int[] { 5, 10 });
                table.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell cell;

                //
                cell = new PdfPCell();
                cell.PaddingBottom = 5;
                cell.PaddingLeft = 10;
                cell.AddElement(new Paragraph("Min value"));
                table.AddCell(cell);
                cell = new PdfPCell();
                cell.PaddingBottom = 5;
                cell.PaddingLeft = 10;
                cell.AddElement(new Paragraph(minVal));
                table.AddCell(cell);

                //
                cell = new PdfPCell();
                cell.PaddingBottom = 5;
                cell.PaddingLeft = 10;
                cell.AddElement(new Paragraph("Max value"));
                table.AddCell(cell);
                cell = new PdfPCell();
                cell.PaddingBottom = 5;
                cell.PaddingLeft = 10;
                cell.AddElement(new Paragraph(maxVal));
                table.AddCell(cell);

                //
                cell = new PdfPCell();
                cell.PaddingBottom = 5;
                cell.PaddingLeft = 10;
                cell.AddElement(new Paragraph("Average value"));
                table.AddCell(cell);
                cell = new PdfPCell();
                cell.PaddingBottom = 5;
                cell.PaddingLeft = 10;
                cell.AddElement(new Paragraph(avgVal));
                table.AddCell(cell);

                //
                cell = new PdfPCell();
                cell.PaddingBottom = 5;
                cell.PaddingLeft = 10;
                cell.AddElement(new Paragraph("Most frequent value"));
                table.AddCell(cell);
                cell = new PdfPCell();
                cell.PaddingBottom = 5;
                cell.PaddingLeft = 10;
                cell.AddElement(new Paragraph(mostFrequentVal));
                table.AddCell(cell);

                table.SpacingAfter = 10;
                table.SpacingBefore = 10;
                document.Add(table);

                document.Close();
                writer.Close();

                return ms.GetBuffer();
            }
        }
    }
}