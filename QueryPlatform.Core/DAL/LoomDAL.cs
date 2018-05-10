using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using huansi.assistantApi.Context;
using QueryPlatform.Core.Model;
using QueryPlatform.Core.Helper;

namespace QueryPlatform.Core.DAL
{
    public class LoomDAL
    {
        /// <summary>
        /// 获取织机实时状态
        /// </summary>
        /// <returns></returns>
        public List<LoomStateCoordinate> GetLoomStateQuery()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC dbo.sppbGetLoomStateQuery");
            List<LoomStateCoordinate> list = TableListChange.TableToList<LoomStateCoordinate>(data);
            return list;
        }

        /// <summary>
        /// 获取织机数据
        /// </summary>
        /// <returns></returns>
        public List<Loom> GetLoomStateNowListQuery(string query, string status, string field, string sort)
        {
            //拼接where条件
            string where = "";
            if (!String.IsNullOrEmpty(query))
                where = " WHERE X.iMachineID=" + query;
            if (!String.IsNullOrEmpty(status))
            {
                if (where == "")
                {
                    where = " WHERE C.iStatusID=" + status;
                }
                else
                {
                    where+= " AND C.iStatusID=" + status;
                }
            }
            //拼接排序
            string order = "";
            if (!String.IsNullOrEmpty(field) && !String.IsNullOrEmpty(sort))
                order = " ORDER BY " + field + " " + sort;
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT X.iMachineID,X.iPickCount
,nBancieff=CONVERT(DECIMAL(18, 2),CASE WHEN (X.iRunTime + X.iAllStopTime)=0 THEN 0 ELSE X.iRunTime * 100.0/ (X.iRunTime + X.iAllStopTime)END)
,sBancieff=CONVERT(NVARCHAR(50),CONVERT(DECIMAL(18,2), CASE WHEN (X.iRunTime + X.iAllStopTime)=0 THEN 0 ELSE X.iRunTime * 100.0/ (X.iRunTime + X.iAllStopTime)END))+'%'
,nBanciSpeed=CASE WHEN X.iRunTime =0 THEN 0 ELSE  CONVERT(DECIMAL(18,0),(X.iPickCount*60.0)/iRunTime) END
,iStatusID=C.iStatusID
,sRunTime=CONVERT(NVARCHAR(50),X.iRunTime/3600)+'小时'+CONVERT(NVARCHAR(50),X.iRunTime%3600/60)+'分'
,sAllStopTime=CONVERT(NVARCHAR(50),X.iAllStopTime/3600)+'小时'+CONVERT(NVARCHAR(50),X.iAllStopTime%3600/60)+'分'
FROM(SELECT A.iMachineID
     ,iRunTime=SUM(ISNULL(B.iRunTime, 0))
     ,iAllStopTime=SUM(ISNULL(B.iAllStopTime, 0))
     ,iPickCount=SUM(ISNULL(B.iPickCount, 0))
	 FROM dbo.OpMachine A(NOLOCK) 
	 LEFT JOIN dbo.OpClassDataList B(NOLOCK) ON B.iMachineID = A.iMachineID AND B.iClassListID = dbo.fnzzGetClassid('NOW')
     GROUP BY A.iMachineID)X
LEFT JOIN dbo.vwMachineMap C(NOLOCK) ON C.iMachineID=X.iMachineID" + where + order);
            List<Loom> list = TableListChange.TableToList<Loom>(data);
            return list;
        }

        /// <summary>
        /// 获取织机状态
        /// </summary>
        public DataTable GetLoomState()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT iStatusID,sStatusType FROM dbo.OpStatusType(NOLOCK)");
            return data;
        }
    }
}
