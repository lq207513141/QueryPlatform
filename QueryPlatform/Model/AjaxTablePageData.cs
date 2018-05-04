using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QueryPlatform
{
    public class AjaxTablePageData<T>
    {
        /// <summary>
        /// 列表数据
        /// </summary>
        public List<T> data { set; get; }

        /// <summary>
        /// 分页参数
        /// </summary>
        public AjaxTablePage meta { set; get; }
    }
}
