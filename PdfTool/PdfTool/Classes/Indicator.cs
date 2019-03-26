﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PdfTool.Classes
{
    public class Indicator
    {
        public string ServerName { get; set; }
        public string TagName { get; set; }
        public double? Value { get; set; }
        public bool Questionable { get; set; }
        public bool Substituted { get; set; }
        public string Annotated { get; set; }
        public DateTime Date { get; set; }
    }
}