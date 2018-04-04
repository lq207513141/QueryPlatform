using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eChartData
{
    public class ChartData
    {
        public string name { get; set; }
        public string type { get; set; }
        public bool showAllSymbol { get; set; }
        public int symbolSize { get; set; }

        public bool smooth { get; set; }
        public object[] data { get; set; }
    }
}