using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using huansi.assistantApi.Context;
using QueryPlatform.Core.Model;

namespace QueryPlatform.Core.DAL
{
    public class OpMachineDAL
    {
        public List<OpMachine> GetList(int page, int perpage, out int total, string field, string sort)
        {
            //拼接排序
            string order = "";
            if (!String.IsNullOrEmpty(field) && !String.IsNullOrEmpty(sort))
                order = " ORDER BY " + field + " " + sort;
            //根据当前页和行数，获取数据集
            DataTable data = DBHelper.DbContext().m_Query("SELECT iIden,iMachineID,sMachineName,sProductInfo,iPlanSpeed,iCurSpeed,nCurEfficiency FROM dbo.OpMachine(NOLOCK)" + order, perpage, page, false);
            //用于计算总数
            DataTable dataCount = DBHelper.DbContext().m_ExecuteReader("SELECT dataCount=COUNT(1) FROM dbo.OpMachine(NOLOCK)");
            total = (int)dataCount.Rows[0]["dataCount"];
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
            return list;
        }
    }
}
