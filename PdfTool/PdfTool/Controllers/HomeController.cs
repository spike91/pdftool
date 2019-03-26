using PdfTool.Classes;
using PdfTool.Helpers;
using PdfTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PdfTool.Controllers
{
    public class HomeController : Controller
    {
        private CsvHelper csvHelper;
        private static List<string> GGs = new List<string> { "GG1", "GG3", "GG10", "GG12" };
        private static List<Tuple<string, string>> GG_VALUES = new List<Tuple<string, string>> {
            new Tuple<string, string>("GG1", "ArchiveEditorListing_pi.vkg.ee_GGS5.GG1.TDT1.csv"),
            new Tuple<string, string>("GG3", "ArchiveEditorListing_pi.vkg.ee_GGS5.GG3.TDT3.csv"),
            new Tuple<string, string>("GG10", "ArchiveEditorListing_pi.vkg.ee_GGS5.GG10.TDT16.csv"),
            new Tuple<string, string>("GG12", "ArchiveEditorListing_pi.vkg.ee_GGS5.GG12.TDT8.csv")
        };

        public HomeController()
        {
            this.csvHelper = new CsvHelper();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TemperatureValue(string generatorName)
        {
            var path = AppContext.BaseDirectory;
            var gg = GG_VALUES.Find(g => g.Item1.Equals(generatorName, StringComparison.OrdinalIgnoreCase));
            var values = csvHelper.ReadFile($"{path}App_Data\\{gg.Item2}");            

            return PartialView("TemperatureValues", values); ;
        }

        public ActionResult Indicators()
        {
            var model = new IndicatorsViewModel();
            List<SelectListItem> items = new List<SelectListItem>();
            GGs.ForEach(g => items.Add(new SelectListItem { Text = g, Value = g }));
            model.Generators = items;
            model.TemperatureValues = new List<Indicator>();

            return View(model);
        }

        public ActionResult DownloadFile(IEnumerable<Indicator> data)
        {
            var date = DateTime.Now.Date;
            var time = DateTime.Now.TimeOfDay;
            string filename = "Export_Data";
            filename = $"{filename}_{date.Day}-{date.Month}-{date.Year}";
            filename = $"{filename}_{time.Hours}-{time.Minutes}-{time.Seconds}-{time.Milliseconds}";
            filename = $"{filename}.pdf";

            var helper = new PdfHelper();
            byte[] filedata = helper.ExportPdf(data);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, "pdf/application");
        }
    }
}