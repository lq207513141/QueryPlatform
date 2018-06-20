using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using huansi.assistantApi.Context;
using QueryPlatform.Core.Model;
using QueryPlatform.Core.Helper;

namespace QueryPlatform.Core.DAL
{
    public class SteamerDAL
    {
        /// <summary>
        /// 获取设备实时数据
        /// </summary>
        /// <returns></returns>
        public List<Steamer> GetStateNowListQuery()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC dbo.sppbGetSteamerNow");
            List<Steamer> list = TableListChange.TableToList<Steamer>(data);
            return list;
        }
    }
}
