using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QueryPlatform.Core.Model;
using QueryPlatform.Core.DAL;
using QueryPlatform.AjaxModel;
using System.Web.Http;
using System.Data;
using System.Reflection;

namespace MetronicTest.Controllers
{
    public class QueryController : Controller
    {
        public ActionResult Query(string iMachineGroupID)
        {
            ViewBag.iMachineGroupID = iMachineGroupID;
            return View();
        }
        public ActionResult LoomStateNowPie()
        {
            return View();
        }
        
        public ActionResult Tables()
        {
            return View();
        }

        public ActionResult LoomStateNow()
        {
            return View();
        }

        public ActionResult LoomStateNowList()
        {
            return View();
        }

        /// <summary>
        /// 获取tables
        /// </summary>
        public JsonResult QueryTables()
        {
            //方法1，后台分页，适合数据量大的情况
            //获取当前页  
            int page = Request.QueryString["pagination[page]"] == "" ? 1 : Convert.ToInt32(Request.QueryString["pagination[page]"]);
            //获取行数
            int perpage = Request.QueryString["pagination[perpage]"] == "" ? 10 : Convert.ToInt32(Request.QueryString["pagination[perpage]"]);
            //获取排序
            string field = Request.QueryString["sort[field]"];
            string sort = Request.QueryString["sort[sort]"];
            //json结果
            JsonResult result = new JsonResult();
            //json.data
            AjaxTablePageData<OpMachine> pageData = new AjaxTablePageData<OpMachine>();
            //方法1 适合数据量大的情况
            //根据当前页和行数，获取数据集
            int total = 0;
            List<OpMachine> list = new OpMachineDAL().GetList(page, perpage, out total, field, sort);
            pageData.data = list ?? new List<OpMachine>();
            //设置分页参数
            pageData.meta = new AjaxTablePage
            {
                page = page,
                perpage = perpage,
                total = total
            };
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = pageData;
            return result;

            //方法2，前台分页，适合数据量小的情况
            ////获取排序
            //string field = Request.QueryString["sort[field]"];
            //string sort = Request.QueryString["sort[sort]"];
            ////json结果
            //JsonResult result = new JsonResult();
            ////json.data
            //AjaxTablePageData<OpMachine> pageData = new AjaxTablePageData<OpMachine>();
            //List<OpMachine> list = new OpMachineDAL().GetListSimple(field, sort);
            //pageData.data = list ?? new List<OpMachine>();
            ////允许get
            //result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            //result.Data = pageData;
            //return result;
        }

        /// <summary>
        /// 织机实时状态查询
        /// </summary>
        public JsonResult LoomStateNowQuery()
        {
            //json结果
            JsonResult result = new JsonResult();
            //json.data
            AjaxTablePageData<LoomStateCoordinate> pageData = new AjaxTablePageData<LoomStateCoordinate>();
            //根据当前页和行数，获取数据集
            List<LoomStateCoordinate> list = new LoomDAL().GetLoomStateQuery();
            pageData.data = list;
            //允许get
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = pageData;
            return result;
        }

        /// <summary>
        /// 织机实时状态列表查询
        /// </summary>
        public JsonResult LoomStateNowListQuery()
        {
            //获取查询
            string query = Request.QueryString["query[generalSearch]"];
            //获取状态
            string status = Request.QueryString["query[Status]"];
            //获取排序
            string field = Request.QueryString["sort[field]"];
            string sort = Request.QueryString["sort[sort]"];
            //json结果
            JsonResult result = new JsonResult();
            //json.data
            AjaxTablePageData<Loom> pageData = new AjaxTablePageData<Loom>();
            //根据当前页和行数，获取数据集
            List<Loom> list = new LoomDAL().GetLoomStateNowListQuery(query, status, field, sort);
            pageData.data = list;
            //允许get
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = pageData;
            return result;
        }

        /// <summary>
        /// 获取状态类型
        /// </summary>
        public JsonResult GetLoomState()
        {
            JsonResult result = new JsonResult();
            DataTable data = new LoomDAL().GetLoomState();
            int count = data.Rows.Count;
            object[] List = new object[count];
            int i = 0;
            foreach (DataRow row in data.Rows)
            {
                List[i] = new object[] { (int)row["iStatusID"], (string)row["sStatusType"] };
                i++;
            }
            result.Data = List;
            return result;
        }

        /// <summary>
        /// 获取机型
        /// </summary>
        public JsonResult QueryMachineType()
        {
            JsonResult result = new JsonResult();
            DataTable data = new YarnMachineDAL().GetMachineType();
            int count = data.Rows.Count;
            object[] List = new object[count];
            int i = 0;
            foreach (DataRow row in data.Rows)
            {
                List[i] = new object[] { (int)row["iMachineGroupID"], (string)row["sMachineGroupName"] };
                i++;
            }
            result.Data = List;
            return result;
        }

        /// <summary>
        /// 获取机台号
        /// </summary>
        public JsonResult QueryMachineNo(string machineType)
        {
            JsonResult result = new JsonResult();
            DataTable data = new YarnMachineDAL().GetMachineByType(machineType);
            int count = data.Rows.Count;
            object[] List = new object[count];
            int i = 0;
            foreach (DataRow row in data.Rows)
            {
                List[i] = new object[] { (int)row["iMachineID"], (string)row["sMachineName"] };
                i++;
            }
            result.Data = List;
            return result;
        }

        /// <summary>
        /// 获取机型参数
        /// </summary>
        public JsonResult QueryMachineVar(string machineType)
        {
            JsonResult result = new JsonResult();
            DataTable data = new YarnMachineDAL().GetMachineVarByType(machineType);
            int count = data.Rows.Count;
            object[] List = new object[count];
            int i = 0;
            foreach (DataRow row in data.Rows)
            {
                List[i] = new object[] { (int)row["iaddress"], (string)row["saddress"] };
                i++;
            }
            result.Data = List;
            return result;
        }

        /// <summary>
        /// 获取折线图
        /// </summary>
        public JsonResult QueryData(string MachineNo, string MachineVar, string MachineType, string TimeStart, string TimeEnd)
        {
            string url = Request.Url.ToString();
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
                DataTable data1 = new YarnMachineDAL().GetMachineVarAndCount(MachineNo, MachineVarList[j], MachineType,TimeStart,TimeEnd);
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
                    name = (string)data1.Rows[0]["saddressName"],
                    type = "line",
                    showAllSymbol = true,
                    smooth = true,
                    symbolSize = 2
                };
                int i = 0;
                //获取明细数据
                DataTable data2 = new YarnMachineDAL().GetMachineVarList(MachineNo, MachineVarList[j], MachineType, TimeStart, TimeEnd);
                foreach (DataRow row2 in data2.Rows)
                {
                    DateTime time = (DateTime)row2["tCreateTime"];
                    List2[i] = new object[] { time.ToString(), (string)row2["sValue"] };
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
    }
}
