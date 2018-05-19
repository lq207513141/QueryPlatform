using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryPlatform.Core.Model
{
    /// <summary>
    /// 非织机
    /// </summary>
    public class Machine
    {
        /// <summary>
        /// 机台号
        /// </summary>
        public int iMachineID { get; set; }
        /// <summary>
        /// 状态1
        /// </summary>
        public int iStatusID1 { get; set; }
        /// <summary>
        /// 状态2
        /// </summary>
        public int iStatusID2 { get; set; }
        /// <summary>
        /// 班次车速1
        /// </summary>
        public int nSpeed1 { get; set; }
        /// <summary>
        /// 班次车速2
        /// </summary>
        public int nSpeed2 { get; set; }
        /// <summary>
        /// 班次效率1
        /// </summary>
        public decimal nEfficiency1 { get; set; }
        /// <summary>
        /// 班次效率1
        /// </summary>
        public decimal nEfficiency2 { get; set; }
    }
}