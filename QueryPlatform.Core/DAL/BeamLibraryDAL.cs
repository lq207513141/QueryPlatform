using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using huansi.assistantApi.Context;
using QueryPlatform.Core.Model;
using QueryPlatform.Core.Helper;

namespace QueryPlatform.Core.DAL
{
    public class BeamLibraryDAL
    {
        /// <summary>
        /// 获取轴库品名
        /// </summary>
        public DataTable GetMaterialNo()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT DISTINCT(sMaterialNo) 
FROM dbo.vwZhouKu(NOLOCK)
WHERE ISNULL(sMaterialNo, 0x) <> 0x
ORDER BY sMaterialNo");
            return data;
        }

        /// <summary>
        /// 获取轴库明细数据
        /// </summary>
        /// <returns></returns>
        public List<BeamLibrary> GetBeamLibraryQuery(string query, string sMaterialNo, string field, string sort)
        {
            //拼接where条件
            string where = "";
            if (!String.IsNullOrEmpty(query))
                where += " AND A.kuwei=" + query;
            if (!String.IsNullOrEmpty(sMaterialNo))
                where += " AND A.sMaterialNo='" + sMaterialNo + "'";
            //拼接排序
            string order = "";
            if (!String.IsNullOrEmpty(field) && !String.IsNullOrEmpty(sort) && (field == "kuwei" || field == "sMaterialNo" || field == "nLength" || field == "tDate"))
            {
                if (field == "kuwei")
                {
                    order = " ORDER BY CONVERT(INT,kuwei)" + sort;
                }
                else
                {
                    order = " ORDER BY " + field + " " + sort;
                }
            }
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT A.*
FROM dbo.vwZhouKu A(NOLOCK) 
WHERE A.kuwei<>'实时显示'" + where + order);
            List<BeamLibrary> list = TableListChange.TableToList<BeamLibrary>(data);
            return list;
        }

        /// <summary>
        /// 获取轴库明细统计
        /// </summary>
        /// <returns></returns>
        public List<BeamLibraryCount> GetBeamLibraryCountQuery(string field, string sort)
        {
            //拼接排序
            string order = "";
            if (!String.IsNullOrEmpty(field) && !String.IsNullOrEmpty(sort) && (field == "count" || field == "sMaterialNo" || field == "nLength"))
                    order = " ORDER BY " + field + " " + sort;
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT sMaterialNo,[count]=COUNT(1),nlength=SUM(CONVERT(DECIMAL(18,2),CASE nlength WHEN 0x THEN 0 ELSE nlength END))
FROM dbo.vwZhouKu A(NOLOCK) 
WHERE A.kuwei<>'实时显示'
GROUP BY sMaterialNo" + order);
            List<BeamLibraryCount> list = TableListChange.TableToList<BeamLibraryCount>(data);
            return list;
        }
    }
}
