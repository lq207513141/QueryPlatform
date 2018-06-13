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
                    where += " AND C.iStatusID=" + status;
                }
            }
            //拼接排序
            string order = "";
            if (!String.IsNullOrEmpty(field) && !String.IsNullOrEmpty(sort) && field == "iMachineID")
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
        /// 获取停台情况
        /// </summary>
        /// <returns></returns>
        public List<IntData> GetLoomPie1()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT * FROM
(SELECT value=COUNT(1),name='运转'
FROM dbo.OpMachine A(NOLOCK)
JOIN dbo.vwMachineMap B(NOLOCK) ON B.iMachineID = A.iMachineID
WHERE B.iStatusID = 0
UNION
SELECT count = COUNT(1), name = '停台'
FROM dbo.OpMachine A(NOLOCK)
JOIN dbo.vwMachineMap B(NOLOCK) ON B.iMachineID = A.iMachineID
WHERE B.iStatusID <> 0) X
ORDER BY X.name DESC");
            List<IntData> list = TableListChange.TableToList<IntData>(data);
            return list;
        }

        /// <summary>
        /// 获取停台明细
        /// </summary>
        /// <returns></returns>
        public List<IntData> GetLoomPie2()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC dbo.spzzAnalyBanGetMacStatus @iDeptID = 'ALL'");
            List<IntData> list = new List<IntData>();
            foreach (DataRow row in data.Rows)
            {
                if ((int)row["bGroup"] == 0)
                {
                    list.Add(new IntData { value = (int)row["iMacNum"], name = (string)row["sStatusType"] });
                }
            }
            return list;
        }

        /// <summary>
        /// 获取效率情况
        /// </summary>
        /// <returns></returns>
        public List<IntData> GetLoomPie3()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC dbo.spzzAnalyBanGetefficiencyFendang @iDeptID = 'ALL',@iClassListID = 'NOW'");
            List<IntData> list = new List<IntData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new IntData { value = (int)row["iMacNum"], name = (string)row["sStatusType"] });
            }
            return list;
        }

        /// <summary>
        /// 获取车速情况
        /// </summary>
        /// <returns></returns>
        public List<IntData> GetLoomPie4()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC spzzAnalyBanGetnSpeedFendang @iDeptID='ALL'");
            List<IntData> list = new List<IntData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new IntData { value = (int)row["iMacNum"], name = (string)row["sStatusType"] });
            }
            return list;
        }

        /// <summary>
        /// 获取停台次数分析
        /// </summary>
        /// <returns></returns>
        public List<IntData> LoomAnalysisPie1()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC [dbo].[spzzAnalyMacGetMacStopStatusCountTime] @sMacNo='ALL'");
            List<IntData> list = new List<IntData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new IntData { value = (int)row["iStatusCount"], name = (string)row["sStatusType"] });
            }
            return list;
        }

        /// <summary>
        /// 获取停台次数分析
        /// </summary>
        /// <returns></returns>
        public List<IntData> LoomAnalysisPie1_1(string time, string sClassName)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC [dbo].[spwebAnalyMacGetMacStopStatusCountTime] @sMacNo='ALL',@time=:time,@sClassName=:sClassName",time,sClassName);
            List<IntData> list = new List<IntData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new IntData { value = (int)row["iStatusCount"], name = (string)row["sStatusType"] });
            }
            return list;
        }

        /// <summary>
        /// 获取停台时间分析
        /// </summary>
        /// <returns></returns>
        public List<IntData> LoomAnalysisPie2()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC [dbo].[spzzAnalyMacGetMacStopStatusCountTime] @sMacNo='ALL'");
            List<IntData> list = new List<IntData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new IntData { value = Convert.ToInt32((decimal)row["iStatusTime"]), name = (string)row["sStatusType"] });
            }
            return list;
        }

        /// <summary>
        /// 获取停台时间分析
        /// </summary>
        /// <returns></returns>
        public List<IntData> LoomAnalysisPie2_1(string time, string sClassName)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC [dbo].[spwebAnalyMacGetMacStopStatusCountTime] @sMacNo='ALL',@time=:time,@sClassName=:sClassName", time, sClassName);
            List<IntData> list = new List<IntData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new IntData { value = Convert.ToInt32((decimal)row["iStatusTime"]), name = (string)row["sStatusType"] });
            }
            return list;
        }

        /// <summary>
        /// 获取停台频繁机台，按停机次数排序
        /// </summary>
        /// <returns></returns>
        public DataTable LoomAnalysisPie3(string type,int iClassId)
        {
            string sql = "";
            if(iClassId==0)
            {
                sql = "SELECT * FROM dbo.vwOpClassDataList WHERE iClassListID=dbo.fnzzGetClassid('NOW')";
            }
            else
            {
                sql = "SELECT * FROM dbo.vwOpClassDataList WHERE iClassListID="+ Convert.ToString(iClassId);
            }
            if (type== "iAllStopCount")
            {
                sql += " ORDER BY iAllStopCount DESC";
            }
            else if(type== "iAllStopTime")
            {
                sql += " ORDER BY iAllStopTime DESC";
            }

            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(sql);
            DataTable result = new DataTable();
            //克隆结构
            result = data.Clone();
            //取前十条
            if(data.Rows.Count>0)
            {
                for (int i = 0; i < 10; i++)
                {
                    result.Rows.Add(data.Rows[i].ItemArray);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取低效率机台
        /// </summary>
        /// <returns></returns>
        public List<DecimalData> LoomAnalysisPie4()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC [dbo].[spzzAnalyBanGetMacefficiencyLow] @iDeptID='ALL',@iClassListID='NOW'");
            List<DecimalData> list = new List<DecimalData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new DecimalData { value = (decimal)row["nefficiency"], name = (string)row["iMachineID"] });
            }
            return list;
        }

        /// <summary>
        /// 获取产量月报
        /// </summary>
        /// <returns></returns>
        public List<DecimalData> DayAnalysis1(string time)
        {
            string start = time + "-01";
            string end = Convert.ToString(Convert.ToDateTime(start).AddMonths(1).AddDays(-1));
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC dbo.spzzReportfmProductDaySumReport @iDeptID=1,@tClassDate1=:start,@tClassDate2=:end",start,end);
            List<DecimalData> list = new List<DecimalData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new DecimalData { value = (decimal)row["nLength"], name = (string)row["tClassDate"] });
            }
            return list;
        }

        /// <summary>
        /// 获取产量月报
        /// </summary>
        /// <returns></returns>
        public List<DecimalData> DayAnalysis2(string time)
        {
            string start = time + "-01";
            string end = Convert.ToString(Convert.ToDateTime(start).AddMonths(1).AddDays(-1));
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC dbo.spzzReportfmProductDaySumReport @iDeptID=1,@tClassDate1=:start,@tClassDate2=:end", start, end);
            List<DecimalData> list = new List<DecimalData>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(new DecimalData { value = (decimal)row["nbancieff"], name = (string)row["tClassDate"] });
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

        /// <summary>
        /// 获取当前班次名称对应的id
        /// </summary>
        public int GetClassNameNow()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT iiden FROM dbo.OpWorkTime(NOLOCK) WHERE sType=(SELECT sClassName FROM dbo.OpClassTimeList WHERE iClassListID=dbo.fnzzGetClassid('NOW'))");
            int iClassId = (int)data.Rows[0]["iiden"];
            return iClassId;
        }

        /// <summary>
        /// 获取当前班次id
        /// </summary>
        public int GetClassIdNow()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT iClassListID=CONVERT(INT,dbo.fnzzGetClassid('NOW'))");
            int iClassId = (int)data.Rows[0]["iClassListID"];
            return iClassId;
        }

        /// <summary>
        /// 获取班次
        /// </summary>
        public DataTable GetWorkTime()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT text=sType,value=iiden FROM dbo.OpWorkTime(NOLOCK)");
            return data;
        }

        /// <summary>
        /// 获取班次id
        /// </summary>
        public int GetClassId(string time,string sClassName)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT TOP 1 iClassListID FROM dbo.OpClassTimeList (NOLOCK) WHERE DATEDIFF(dd,tClassDate,:time)=0 AND sClassName=:sClassName", time, sClassName);
            int iClassId = (int)data.Rows[0]["iClassListID"];
            return iClassId;
        }

        /// <summary>
        /// 获取品名每日情况
        /// </summary>
        /// <returns></returns>
        public List<LoomProductNoDay> GetProductNoDay(int iClassId)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC [dbo].[spzzReportfmProductDaySumReportByType] @iDeptID = 1,@iClassListID=:iClassId", iClassId);
            List<LoomProductNoDay> list = TableListChange.TableToList<LoomProductNoDay>(data);
            return list;
        }

        /// <summary>
        /// 获取挡车工每日情况
        /// </summary>
        /// <returns></returns>
        public List<LoomWorkerDay> GetWorkerDay(int iClassId)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("EXEC [dbo].[spzzReportfmWorkProductDaySumReport] @iDeptID = 1,@iClassListID=:iClassId", iClassId);
            List<LoomWorkerDay> list = TableListChange.TableToList<LoomWorkerDay>(data);
            return list;
        }
    }
}
