using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryPlatform.Core.Model
{
    public class OpMachine
    {
        public int iIden { get; set; }
        public int iMachineID { get; set; }
        public string sMachineName { get; set; }
        public string sProductInfo { get; set; }
        public int iPlanSpeed { get; set; }
        public int iCurSpeed { get; set; }
        public decimal nCurEfficiency { get; set; }
    }
}