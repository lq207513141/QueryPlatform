using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryPlatform.Core.Model
{
    /// <summary>
    /// 挡车工每日统计
    /// </summary>
    public class LoomWorkerDay
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int iRowid { get; set; }
        /// <summary>
        /// 工人
        /// </summary>
        public string sWorker { get; set; }
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

        /// <summary>
        /// 米数比较，最大2，最小1
        /// </summary>
        public int iCompare1 { get; set; }

        /// <summary>
        /// 效率比较，最大2，最小1
        /// </summary>
        public int iCompare2 { get; set; }
    }
}