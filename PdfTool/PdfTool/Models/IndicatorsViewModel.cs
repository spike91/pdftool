using PdfTool.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PdfTool.Models
{
    public class IndicatorsViewModel
    {
        public IEnumerable<Temperature> TemperatureValues { get; set; }
        public IEnumerable<SelectListItem> Generators { get; set; }
        public string SelectedGenerator { get; set; }
    }
}