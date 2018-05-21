using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QueryPlatform.Core.Model
{
    /// <summary>
    /// 轴库
    /// </summary>
    public class BeamLibraryCount
    {
        /// <summary>
        /// 品名
        /// </summary>
        public string sMaterialNo { get; set; }
        /// <summary>
        /// 个数
        /// </summary>
        public string count { get; set; }
        /// <summary>
        /// 轴长
        /// </summary>
        public decimal nLength { get; set; }
    }
}