using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryPlatform.Core.Model
{
    /// <summary>
    /// 蒸箱
    /// </summary>
    public class Steamer
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
        /// 品种
        /// </summary>
        public string sProductNo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int iState { get; set; }
        /// <summary>
        /// 保温时间
        /// </summary>
        public int iHoldingTime { get; set; }
        /// <summary>
        /// 温度
        /// </summary>
        public int iTemperature { get; set; }
        /// <summary>
        /// 当前循环次数
        /// </summary>
        public int iRoundCount { get; set; }
        /// <summary>
        /// 当前恒温时间
        /// </summary>
        public int iConstantTime { get; set; }
        /// <summary>
        /// 当前设定温度
        /// </summary>
        public int iTemperatureSet { get; set; }
    }
}