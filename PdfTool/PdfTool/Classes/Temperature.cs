using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PdfTool.Classes
{
    public class Temperature
    {
        public string ServerName { get; set; }
        public string TagName { get; set; }
        public string Value { get; set; }
        public bool Questionable { get; set; }
        public bool Substituted { get; set; }
        public string Annotated { get; set; }
        public string Date { get; set; }
    }
}