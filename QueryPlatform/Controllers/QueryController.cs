using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using huansi.assistantApi.Context;
using QueryPlatform.Core;
using QueryPlatform;
using System.Web.Http;

namespace MetronicTest.Controllers
{
    public class QueryController : Controller
    {
        public ActionResult Query()
        {
            ViewBag.Title = "Query Page";

            return View();
        }

        public ActionResult Tables()
        {
            ViewBag.Title = "Tables Page";

            return View();
        }

        /// <summary>
        /// 获取设备类型
        /// </summary>
        public JsonResult QueryMachineType()
        {
            JsonResult result = new JsonResult();
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT MachineType,MachineName FROM dbo.MachineType(NOLOCK) ORDER BY MachineName");
            int count = data.Rows.Count;
            object[] List = new object[count];
            int i = 0;
            foreach (DataRow row in data.Rows)
            {
                List[i] = new object[] { (string)row["MachineType"], (string)row["MachineName"] };
                i++;
            }
            result.Data = List;
            return result;
        }

        /// <summary>
        /// 获取机台号
        /// </summary>
        public JsonResult QueryMachineNo(string MachineType)
        {
            JsonResult result = new JsonResult();
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT MachineID,MachineNO FROM dbo.MachineNO(NOLOCK) WHERE MachineType=:sMachineType ORDER BY MachineNO", MachineType);
            int count = data.Rows.Count;
            object[] List = new object[count];
            int i = 0;
            foreach (DataRow row in data.Rows)
            {
                List[i] = new object[] { (int)row["MachineID"], (string)row["MachineNO"] };
                i++;
            }
            result.Data = List;
            return result;
        }

        /// <summary>
        /// 获取机台参数
        /// </summary>
        public JsonResult QueryMachineVar(string MachineType)
        {
            JsonResult result = new JsonResult();
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT VarAddress,VarNameCN FROM dbo.VarAddress(NOLOCK) WHERE MachineType=:sMachineType ORDER BY VarNameCN", MachineType);
            int count = data.Rows.Count;
            object[] List = new object[count];
            int i = 0;
            foreach (DataRow row in data.Rows)
            {
                List[i] = new object[] { (string)row["VarAddress"], (string)row["VarNameCN"] };
                i++;
            }
            result.Data = List;
            return result;
        }

        /// <summary>
        /// 获取折线图
        /// </summary>
        public JsonResult QueryData(string MachineNo, string MachineVar, string TimeStart, string TimeEnd)
        {
            JsonResult result = new JsonResult();
            if (String.IsNullOrEmpty(MachineNo) || String.IsNullOrEmpty(MachineVar))
            {
                result.Data = "";
                return result;
            }
            DateTime tStartTime = Convert.ToDateTime(TimeStart);
            DateTime tEndTime = Convert.ToDateTime(TimeEnd);
            string[] MachineVarList = MachineVar.Split(',');
            List<ChartData> list1 = new List<ChartData>();
            //遍历类型
            for (int j = 0; j < MachineVarList.Length; j++)
            {
                //获取类型名称和明细个数
                DataTable data1 = DBHelper.DbContext().m_ExecuteReader("SELECT A.VarNameCN,[count]=COUNT(1) FROM dbo.VarAddress A(NOLOCK) JOIN dbo.DataCollectRecord B(NOLOCK) ON B.VarAddress = A.VarAddress WHERE A.VarAddress=:sVarAddress AND B.MachineID=:iMachineID AND B.StartTime>=:tStartTime AND B.StartTime<=:tEndTime GROUP BY A.VarNameCN ORDER BY A.VarNameCN", MachineVarList[j], MachineNo, tStartTime, tEndTime);
                if (data1.Rows.Count == 0)
                {
                    continue;
                }
                int count = (int)data1.Rows[0]["count"];
                //根据数量定义数组个数
                object[] List2 = new object[count];
                //对象赋值
                ChartData chardata = new ChartData()
                {
                    name = (string)data1.Rows[0]["VarNameCN"],
                    type = "line",
                    showAllSymbol = true,
                    smooth = true,
                    symbolSize = 2
                };
                int i = 0;
                //获取明细数据
                DataTable data2 = DBHelper.DbContext().m_ExecuteReader("SELECT StartTime,Value FROM dbo.DataCollectRecord(NOLOCK) WHERE VarAddress=:sVarAddress AND MachineID=:iMachineID AND StartTime>=:tStartTime AND StartTime<=:tEndTime ORDER BY StartTime", MachineVarList[j], MachineNo, tStartTime, tEndTime);
                foreach (DataRow row2 in data2.Rows)
                {
                    DateTime time = (DateTime)row2["StartTime"];
                    List2[i] = new object[] { time.ToString(), (string)row2["Value"] };
                    i++;
                }
                //明细赋值
                chardata.data = List2;
                list1.Add(chardata);
            }
            //写入结果
            result.Data = list1;
            return result;
        }

        /// <summary>
        /// 获取tables
        /// </summary>
        public JsonResult QueryTables()
        {
            //获取当前页
            string strPage = Request.QueryString["pagination[page]"];
            //获取行数
            string strPerpage = Request.QueryString["pagination[perpage]"];          
            int page = strPage == "" ? 1 : Convert.ToInt32(strPage);
            int perpage = strPerpage == "" ? 10 : Convert.ToInt32(strPerpage);          
            //获取排序
            string field = Request.QueryString["sort[field]"];
            string sort = Request.QueryString["sort[sort]"];
            string order = "";
            if (!String.IsNullOrEmpty(field) && !String.IsNullOrEmpty(sort))
                order = " ORDER BY " + field + " " + sort;
            //json结果
            JsonResult result = new JsonResult();
            //json.data
            AjaxTablePageData<OpMachine> pageData = new AjaxTablePageData<OpMachine>();
            //根据当前页和行数，获取数据集
            DataTable data = DBHelper.DbContext().m_Query("SELECT iIden,iMachineID,sMachineName,sProductInfo,iPlanSpeed,iCurSpeed,nCurEfficiency FROM dbo.OpMachine(NOLOCK)" + order, perpage, page, false);
            //用于计算总数
            DataTable dataCount = DBHelper.DbContext().m_ExecuteReader("SELECT dataCount=COUNT(1) FROM dbo.OpMachine(NOLOCK)");
            List<OpMachine> list = new List<OpMachine>();
            //数据集转换为列表
            foreach (DataRow row in data.Rows)
            {
                OpMachine model = new OpMachine()
                {
                    iIden = (int)row["iIden"],
                    iMachineID = (int)row["iMachineID"],
                    sMachineName = (string)row["sMachineName"],
                    sProductInfo = (string)row["sProductInfo"],
                    iPlanSpeed = (int)row["iPlanSpeed"],
                    iCurSpeed = (int)row["iCurSpeed"],
                    nCurEfficiency = (decimal)row["nCurEfficiency"]
                };
                list.Add(model);
            }
            pageData.data = list ?? new List<OpMachine>();
            //设置分页参数
            pageData.meta = new AjaxTablePage
            {
                page = page,
                perpage = perpage,
                total = (int)dataCount.Rows[0]["dataCount"]
            };
            //允许get
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = pageData;
            return result;
        }
    }
}
