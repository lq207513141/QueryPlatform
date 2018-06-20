using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryPlatform.Core.Model
{
    /// <summary>
    /// 扦经机
    /// </summary>
    public class Warping
    {
        /// <summary>
        /// 机台号
        /// </summary>
        public int iMachineID { get; set; }
        /// <summary>
        /// 工人
        /// </summary>
        public string sWorkName { get; set; }
        /// <summary>
        /// 任务号
        /// </summary>
        public string sTaskNo { get; set; }
        /// <summary>
        /// 车速
        /// </summary>
        public decimal nSpeed { get; set; }
        /// <summary>
        /// 效率
        /// </summary>
        public decimal nEfficiency { get; set; }
        /// <summary>
        /// 品种
        /// </summary>
        public string sProductNo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int iState { get; set; }
        /// <summary>
        /// 当前纱长
        /// </summary>
        public int iYarnLength { get; set; }
        /// <summary>
        /// 断头个数
        /// </summary>
        public int iBrokenCount { get; set; }
        /// <summary>
        /// 当前条数
        /// </summary>
        public int iBarCount { get; set; }
        /// <summary>
        /// 设定纱长
        /// </summary>
        public int iYarnSet { get; set; }
        /// <summary>
        /// 设定条数
        /// </summary>
        public int iBarSet { get; set; }
    }
}