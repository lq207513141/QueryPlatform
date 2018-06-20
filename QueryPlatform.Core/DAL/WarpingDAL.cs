using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using huansi.assistantApi.Context;
using QueryPlatform.Core.Model;
using QueryPlatform.Core.Helper;

namespace QueryPlatform.Core.DAL
{
    public class WarpingDAL
    {
        /// <summary>
        /// 获取设备实时数据
        /// </summary>
        /// <returns></returns>
        public List<Warping> GetStateNowListQuery()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC dbo.sppbGetWarpingNow");
            List<Warping> list = TableListChange.TableToList<Warping>(data);
            return list;
        }
    }
}
