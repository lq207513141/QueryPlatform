using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryPlatform.Core.Model
{
    /// <summary>
    /// 织机
    /// </summary>
    public class Loom
    {
        /// <summary>
        /// 机台号
        /// </summary>
        public int iMachineID { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int iStatusID { get; set; }
        /// <summary>
        /// 班次效率
        /// </summary>
        public string sBancieff { get; set; }
        /// <summary>
        /// 班次车速
        /// </summary>
        public int nBanciSpeed { get; set; }
        /// <summary>
        /// 打纬数
        /// </summary>
        public int iPickCount { get; set; }
        /// <summary>
        /// 运行时间
        /// </summary>
        public string sRunTime { get; set; }
        /// <summary>
        /// 总停机时间
        /// </summary>
        public string sAllStopTime { get; set; }
    }
}