using PdfTool.Classes;
using PdfTool.Helpers;
using PdfTool.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public ActionResult IndicatorValue(string generatorName)
        {
            var path = AppContext.BaseDirectory;
            var gg = GG_VALUES.Find(g => g.Item1.Equals(generatorName ?? "", StringComparison.OrdinalIgnoreCase));
            List<Temperature> values = new List<Temperature>();
            if (gg != null) {
                values = csvHelper.ReadFile($"{path}App_Data\\{gg.Item2}")?.ToList();
            }

            return Json(values, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Indicators()
        {
            var model = new IndicatorsViewModel();
            List<SelectListItem> items = new List<SelectListItem>();
            GGs.ForEach(g => items.Add(new SelectListItem { Text = g, Value = g }));
            model.Generators = items;
            model.TemperatureValues = new List<Temperature>();

            return View(model);
        }

        [HttpPost]
        public ActionResult DownloadFile(string generatorName)
        {
            var date = DateTime.Now.Date;
            var time = DateTime.Now.TimeOfDay;
            string filename = $"Export_Data_{generatorName}";
            filename = $"{filename}_{date.Day}-{date.Month}-{date.Year}";
            filename = $"{filename}_{time.Hours}-{time.Minutes}-{time.Seconds}-{time.Milliseconds}";
            filename = $"{filename}.pdf";

            var path = AppContext.BaseDirectory;
            var gg = GG_VALUES.Find(g => g.Item1.Equals(generatorName ?? "", StringComparison.OrdinalIgnoreCase));
            List<Temperature> data = new List<Temperature>();
            if (gg != null)
            {
                data = csvHelper.ReadFile($"{path}App_Data\\{gg.Item2}")?.ToList();
            }

            var helper = new PdfHelper();
            var dates = data.Select(v => v.Date).ToList();
            List<DateTime> datesVal = new List<DateTime>();
            dates?.ForEach(d => datesVal.Add(DateTime.ParseExact(d, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture)));
            var startDate = datesVal?.OrderBy(v => v).First();
            var startDateStr = startDate?.ToShortDateString() ?? "NA";
            var endDate = datesVal?.OrderByDescending(v => v).First();
            var endDateStr = endDate?.ToShortDateString() ?? "NA";

            List<double> values = data?.Select(v => {
                double val = 0;
                double.TryParse(v.Value, out val);
                return val;
            })?.ToList() ?? new List<double>();

            var minVal = (values?.OrderBy(v => v)?.First())?.ToString() ?? "NA";
            var maxVal = (values?.OrderBy(v => v)?.Last())?.ToString() ?? "NA";
            var avgVal = 0f;
            var idx = 0;
            data?.ForEach(item => {
                float.TryParse(item.Value, out float val);
                avgVal += val;
                idx++;
            });
            if (idx > 0)
            {
                avgVal /= idx;
            }
            var mostFreqVal = data?.GroupBy(v => v)?.Select(x => new { num = x, cnt = x.Count() })?.OrderByDescending(grp => grp.cnt)?.Select(g => g.num)?.First()?.Key?.Value?.ToString() ?? "NA";

            byte[] filedata = helper.ExportPdf(generatorName, data.Count, $"{startDateStr} - {endDateStr}", maxVal.ToString(), minVal.ToString(), avgVal.ToString(), mostFreqVal.ToString());

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