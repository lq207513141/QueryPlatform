using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryPlatform.Core.Model
{
    /// <summary>
    /// 轴库
    /// </summary>
    public class BeamLibrary
    {
        /// <summary>
        /// 库位
        /// </summary>
        public string kuwei { get; set; }
        /// <summary>
        /// 品名
        /// </summary>
        public string sMaterialNo { get; set; }
        /// <summary>
        /// 轴长
        /// </summary>
        public decimal nLength { get; set; }
        /// <summary>
        /// 入库时间
        /// </summary>
        public string tDate { get; set; }
    }
}