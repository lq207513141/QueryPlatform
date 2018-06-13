using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryPlatform.Core.Model
{
    /// <summary>
    /// 品种每日统计
    /// </summary>
    public class LoomProductNoDay
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int iRowid { get; set; }
        /// <summary>
        /// 品种
        /// </summary>
        public string sProductNo { get; set; }
        /// <summary>
        /// 机台数
        /// </summary>
        public int iCount { get; set; }
        /// <summary>
        /// 平均效率
        /// </summary>
        public decimal nbancieff { get; set; }
        /// <summary>
        /// 平均车速
        /// </summary>
        public int nbanciSpeed { get; set; }
        /// <summary>
        /// 总长度
        /// </summary>
        public int nLength { get; set; }
        /// <summary>
        /// 总停台次数
        /// </summary>
        public int iAllStopCount { get; set; }
        /// <summary>
        /// 总停台时间
        /// </summary>
        public int iAllStopTime { get; set; }
        /// <summary>
        /// 总停台时间
        /// </summary>
        public string sAllStopTime { get; set; }
    }
}