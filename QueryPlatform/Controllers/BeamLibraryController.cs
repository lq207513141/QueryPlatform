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
    public class BeamLibraryController : Controller
    {
        public ActionResult Query()
        {
            return View();
        }

        public ActionResult CountQuery()
        {
            return View();
        }

        /// <summary>
        /// 获取品名
        /// </summary>
        public JsonResult GetMaterialNo()
        {
            JsonResult result = new JsonResult();
            DataTable data = new BeamLibraryDAL().GetMaterialNo();
            int count = data.Rows.Count;
            object[] List = new object[count];
            int i = 0;
            foreach (DataRow row in data.Rows)
            {
                List[i] = new object[] { (string)row["sMaterialNo"], (string)row["sMaterialNo"] };
                i++;
            }
            result.Data = List;
            return result;
        }

        /// <summary>
        /// 轴库明细查询
        /// </summary>
        public JsonResult BeamLibraryQuery()
        {
            //json结果
            JsonResult result = new JsonResult();
            //获取查询
            string query = Request.QueryString["query[generalSearch]"];
            //获取状态
            string sMaterialNo = Request.QueryString["query[sMaterialNo]"];
            //获取排序
            string field = Request.QueryString["sort[field]"];
            string sort = Request.QueryString["sort[sort]"];
            //json.data
            AjaxTablePageData<BeamLibrary> pageData = new AjaxTablePageData<BeamLibrary>();
            //根据当前页和行数，获取数据集
            List<BeamLibrary> list = new BeamLibraryDAL().GetBeamLibraryQuery(query, sMaterialNo, field, sort);
            pageData.data = list;
            //允许get
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = pageData;
            return result;
        }

        /// <summary>
        /// 轴库明细统计
        /// </summary>
        public JsonResult BeamLibraryCountQuery()
        {
            //json结果
            JsonResult result = new JsonResult();
            //获取排序
            string field = Request.QueryString["sort[field]"];
            string sort = Request.QueryString["sort[sort]"];
            //json.data
            AjaxTablePageData<BeamLibraryCount> pageData = new AjaxTablePageData<BeamLibraryCount>();
            //根据当前页和行数，获取数据集
            List<BeamLibraryCount> list = new BeamLibraryDAL().GetBeamLibraryCountQuery(field, sort);
            pageData.data = list;
            //允许get
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = pageData;
            return result;
        }
    }
}
