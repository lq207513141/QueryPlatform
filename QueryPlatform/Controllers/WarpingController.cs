﻿using System;
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
    public class WarpingController : Controller
    {
        public ActionResult StateNowList()
        {
            return View();
        }

        /// <summary>
        /// 设备实时状态列表查询
        /// </summary>
        public JsonResult StateNowListQuery()
        {
            //json结果
            JsonResult result = new JsonResult();            
            //json.data
            AjaxTablePageData<Warping> pageData = new AjaxTablePageData<Warping>();
            //根据当前页和行数，获取数据集
            List<Warping> list = new WarpingDAL().GetStateNowListQuery();
            pageData.data = list;
            //允许get
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            result.Data = pageData;
            return result;
        }
    }
}
