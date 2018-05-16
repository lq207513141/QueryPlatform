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
    public class LoomController : Controller
    {
        public ActionResult LoomStateNowPie()
        {
            return View();
        }

        public ActionResult LoomStateNowGauge()
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

        public ActionResult LoomAnalysisPie()
        {
            return View();
        }

        /// <summary>
        /// 织机实时状态查询
        /// </summary>
        public JsonResult LoomStateNowQuery()
        {
            string varCheck = Request.QueryString["query[varCheck]"]; 
            int machineVar = string.IsNullOrEmpty(varCheck) ? 0 : Convert.ToInt32(varCheck);
            //json结果
            JsonResult result = new JsonResult();
            //json.data
            AjaxTablePageData<LoomStateCoordinate> pageData = new AjaxTablePageData<LoomStateCoordinate>();
            //根据当前页和行数，获取数据集
            List<LoomStateCoordinate> list = new LoomDAL().GetLoomStateQuery(machineVar);
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
        /// 获取织机机台号
        /// </summary>
        public JsonResult GetLoomMachineNo()
        {
            JsonResult result = new JsonResult();
            DataTable data = new LoomDAL().GetLoomMachineNo();
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
        /// 织机实时车速图
        /// </summary>
        public JsonResult GetLoomStateNowGauge(string iMachineID)
        {
            JsonResult result = new JsonResult();
            DataTable data = new LoomDAL().GetLoomStateNowGauge(iMachineID);
            int count = data.Rows.Count;
            object[] List = new object[count];
            int i = 0;
            foreach (DataRow row in data.Rows)
            {
                List[i] = new object[] { Convert.ToInt32((decimal)row["nBanciSpeed"]), (decimal)row["nBancieff"] };
                i++;
            }
            result.Data = List;
            return result;
        }

        /// <summary>
        /// 获取停机情况
        /// </summary>
        public JsonResult GetLoomPie1()
        {
            JsonResult result = new JsonResult();

            //获取类型名称和明细个数
            List<PieData> list = new LoomDAL().GetLoomPie1();
            //写入结果
            result.Data = list;
            return result;
        }

        /// <summary>
        /// 获取停机明细
        /// </summary>
        public JsonResult GetLoomPie2()
        {
            JsonResult result = new JsonResult();

            //获取类型名称和明细个数
            List<PieData> list = new LoomDAL().GetLoomPie2();
            //写入结果
            result.Data = list;
            return result;
        }

        /// <summary>
        /// 获取效率统计
        /// </summary>
        public JsonResult GetLoomPie3()
        {
            JsonResult result = new JsonResult();

            //获取类型名称和明细个数
            List<PieData> list = new LoomDAL().GetLoomPie3();
            //写入结果
            result.Data = list;
            return result;
        }

        /// <summary>
        /// 获取车速情况
        /// </summary>
        public JsonResult GetLoomPie4()
        {
            JsonResult result = new JsonResult();

            //获取类型名称和明细个数
            List<PieData> list = new LoomDAL().GetLoomPie4();
            //写入结果
            result.Data = list;
            return result;
        }

        /// <summary>
        /// 获取停台次数分析
        /// </summary>
        public JsonResult LoomAnalysisPie1()
        {
            JsonResult result = new JsonResult();

            //获取类型名称和明细个数
            List<PieData> list = new LoomDAL().LoomAnalysisPie1();
            //写入结果
            result.Data = list;
            return result;
        }

        /// <summary>
        /// 获取停台时间分析
        /// </summary>
        public JsonResult LoomAnalysisPie2()
        {
            JsonResult result = new JsonResult();

            //获取类型名称和明细个数
            List<PieData> list = new LoomDAL().LoomAnalysisPie2();
            //写入结果
            result.Data = list;
            return result;
        }

        /// <summary>
        /// 获取低效率机台
        /// </summary>
        public JsonResult LoomAnalysisPie4()
        {
            JsonResult result = new JsonResult();

            //获取类型名称和明细个数
            List<PieData> list = new LoomDAL().LoomAnalysisPie4();
            //写入结果
            result.Data = list;
            return result;
        }
    }
}
