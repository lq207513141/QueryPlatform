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
        public List<LoomStateCoordinate> GetLoomStateQuery(int machineVar)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC dbo.sppbGetLoomStateQuery @iType=:machineVar", machineVar);
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
            if (!String.IsNullOrEmpty(field) && !String.IsNullOrEmpty(sort)&& field== "iMachineID")
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

        /// <summary>
        /// 获取停机情况
        /// </summary>
        /// <returns></returns>
        public List<PieData> GetLoomPie1()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT value=COUNT(1),name='运转'
FROM dbo.OpMachine A(NOLOCK)
JOIN dbo.vwMachineMap B(NOLOCK) ON B.iMachineID = A.iMachineID
WHERE B.iStatusID = 0
UNION
SELECT count = COUNT(1), name = '停台'
FROM dbo.OpMachine A(NOLOCK)
JOIN dbo.vwMachineMap B(NOLOCK) ON B.iMachineID = A.iMachineID
WHERE B.iStatusID <> 0");
            List<PieData> list = TableListChange.TableToList<PieData>(data);
            return list;
        }

        /// <summary>
        /// 获取停机明细
        /// </summary>
        /// <returns></returns>
        public List<PieData> GetLoomPie2()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC dbo.spzzAnalyBanGetMacStatus @iDeptID = 'ALL'");
            List <PieData> list = new List<PieData>();
            foreach (DataRow row in data.Rows)
            {
                if((int)row["bGroup"] ==0)
                {
                    list.Add(new PieData { value = (int)row["iMacNum"], name = (string)row["sStatusType"] });
                }
            }
            return list;
        }

        /// <summary>
        /// 获取效率情况
        /// </summary>
        /// <returns></returns>
        public List<PieData> GetLoomPie3()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC dbo.spzzAnalyBanGetefficiencyFendang @iDeptID = 'ALL',@iClassListID = 'NOW'");
            List<PieData> list = new List<PieData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new PieData { value = (int)row["iMacNum"], name = (string)row["sStatusType"] });
            }
            return list;
        }

        /// <summary>
        /// 获取车速情况
        /// </summary>
        /// <returns></returns>
        public List<PieData> GetLoomPie4()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC spzzAnalyBanGetnSpeedFendang @iDeptID='ALL'");
            List<PieData> list = new List<PieData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new PieData { value=(int)row["iMacNum"], name=(string)row["sStatusType"] });
            }
            return list;
        }

        /// <summary>
        /// 获取停台次数分析
        /// </summary>
        /// <returns></returns>
        public List<PieData> LoomAnalysisPie1()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC  [dbo].[spzzAnalyMacGetMacStopStatusCountTime] @sMacNo='ALL'");
            List<PieData> list = new List<PieData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new PieData { value = (int)row["iStatusCount"], name = (string)row["sStatusType"] });
            }
            return list;
        }

        /// <summary>
        /// 获取停台时间分析
        /// </summary>
        /// <returns></returns>
        public List<PieData> LoomAnalysisPie2()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC  [dbo].[spzzAnalyMacGetMacStopStatusCountTime] @sMacNo='ALL'");
            List<PieData> list = new List<PieData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new PieData { value = Convert.ToInt32((decimal)row["iStatusTime"]/60), name = (string)row["sStatusType"] });
            }
            return list;
        }

        /// <summary>
        /// 获取低效率机台
        /// </summary>
        /// <returns></returns>
        public List<PieData> LoomAnalysisPie4()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC [dbo].[spzzAnalyBanGetMacefficiencyLow] @iDeptID='ALL',@iClassListID='NOW'");
            List<PieData> list = new List<PieData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new PieData { value = (int)row["iMachineID"], name = Convert.ToString((decimal)row["nfficiency"]) });
            }
            return list;
        }

        /// <summary>
        /// 获取织机机台号
        /// </summary>
        public DataTable GetLoomMachineNo()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT A.iMachineID,A.sMachineName FROM dbo.OpMachine A(NOLOCK) ORDER BY A.iMachineID");
            return data;
        }

        /// <summary>
        /// 获取织机机台号
        /// </summary>
        public DataTable GetLoomStateNowGauge(string iMachineID)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT nBancieff=CONVERT(DECIMAL(18, 2),CASE WHEN (X.iRunTime + X.iAllStopTime)=0 THEN 0 ELSE X.iRunTime * 100.0/ (X.iRunTime + X.iAllStopTime)END)
, nBanciSpeed = CASE WHEN X.iRunTime = 0 THEN 0 ELSE  CONVERT(DECIMAL(18, 0), (X.iPickCount * 60.0) / iRunTime) END
FROM(SELECT A.iMachineID
     , iRunTime = SUM(ISNULL(B.iRunTime, 0))
     , iAllStopTime = SUM(ISNULL(B.iAllStopTime, 0))
     , iPickCount = SUM(ISNULL(B.iPickCount, 0))
     FROM dbo.OpMachine A(NOLOCK)
     LEFT JOIN dbo.OpClassDataList B(NOLOCK) ON B.iMachineID = A.iMachineID AND B.iClassListID = dbo.fnzzGetClassid('NOW')
     WHERE A.iMachineID =:iMachineID
     GROUP BY A.iMachineID)X", iMachineID);
            return data;
        }
    }
}
