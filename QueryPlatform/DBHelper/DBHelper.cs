using HuanSi.Lib.DB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace huansi.assistantApi.Context
{
    public class DBHelper
    {
        public static IHSDBManager DbContext(int iIimeout = 60)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DbContext"].ConnectionString;
            IHSDBProvider provider = new HSDBProvider_Sql(connectionString, 60);
            return new HSDBManager()
            {
                m_Provider = provider,
            };
        }
    }
}