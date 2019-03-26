using PdfTool.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PdfTool.Models
{
    public class TemperatureValueViewModel
    {
        public IEnumerable<Indicator> Values { get; set; }
    }
}