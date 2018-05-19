using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryPlatform.Core.Model
{
    public class BarData
    {
        public string name { get; set; }
        public string type { get; set; }
        public string stack { get; set; }
        public Lable label { get; set; }
        public int[] data { get; set; }
    }
}