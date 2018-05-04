using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryPlatform
{
    public class AjaxTablePage
    {
        /// <summary>
        /// 第几页
        /// </summary>
        public int page { set; get; }

        /// <summary>
        /// 每页条数
        /// </summary>
        public int perpage { set; get; }

        /// <summary>
        /// 数据源总行数
        /// </summary>
        public int total { set; get; }
    }
}
