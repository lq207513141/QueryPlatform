using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using huansi.assistantApi.Context;
using QueryPlatform.Core.Model;
using QueryPlatform.Core.Helper;

namespace QueryPlatform.Core.DAL
{
    public class YarnMachineDAL
    {
        /// <summary>
        /// 获取机型
        /// </summary>
        public DataTable GetMachineType()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT iMachineGroupID,sMachineGroupName FROM dbo.psYarnMachineGroup(NOLOCK)");
            return data;
        }

        /// <summary>
        /// 根据机型获取机台号
        /// </summary>
        public DataTable GetMachineByType(string machineType)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT iMachineID,sMachineName FROM dbo.psYarnMachine (NOLOCK) WHERE iMachineGroupID=:machineType ORDER BY iMachineID", machineType);
            return data;
        }

        /// <summary>
        /// 根据机型获取机台参数
        /// </summary>
        public DataTable GetMachineVarByType(string machineType)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT C.iaddress,C.saddress
FROM dbo.OpprotocolSet A(NOLOCK) 
JOIN Opprotocol B(NOLOCK) ON B.iiden=A.iOpprotocolid
JOIN dbo.OpprotocolMap C(NOLOCK) ON C.iOpprotocolid=B.iiden
WHERE A.bUsable=1 AND B.bUsable=1 AND C.bUsable=1 AND C.isNum=1
AND A.iMachineGroupID=:machineType
GROUP BY C.iaddress,C.saddress", machineType);
            return data;
        }

        /// <summary>
        /// 获取参数名称和明细个数
        /// </summary>
        public DataTable GetMachineVar(string MachineNo, string MachineVar, string MachineType, string TimeStart, string TimeEnd)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT A.saddressName
FROM dbo.OpprotocolValue A(NOLOCK)
WHERE A.iMachineGroupID=:MachineType
AND A.iMachineID=:MachineNo
AND A.iaddress=:MachineVar
AND A.tCreateTime>=:TimeStart AND A.tCreateTime<=:TimeEnd
GROUP BY A.saddressName", MachineType, MachineNo, MachineVar, TimeStart, TimeEnd);
            return data;
        }

        /// <summary>
        /// 获取参数记录
        /// </summary>
        public DataTable GetMachineVarList(string MachineNo, string MachineVar, string MachineType, string TimeStart, string TimeEnd)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT A.sValue,A.tCreateTime
FROM dbo.OpprotocolValue A(NOLOCK)
WHERE A.iMachineGroupID=:MachineType
AND A.iMachineID=:MachineNo
AND A.iaddress=:MachineVar
AND A.tCreateTime>=:TimeStart AND A.tCreateTime<=:TimeEnd
ORDER BY A.tCreateTime", MachineType, MachineNo, MachineVar, TimeStart, TimeEnd);
            return data;
        }

        /// <summary>
        /// 获取设备状态
        /// </summary>
        public DataTable GetState()
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader("SELECT iStatusID,sStatusType FROM dbo.OpStatusType_yarn(NOLOCK)");
            return data;
        }

        /// <summary>
        /// 获取设备实时数据
        /// </summary>
        /// <returns></returns>
        public List<Machine> GetStateNowListQuery(string query, string iMachineGroupID, string status, string field, string sort)
        {
            //拼接where条件
            string where = " WHERE A.iMachineGroupID=" + iMachineGroupID;
            if (!String.IsNullOrEmpty(query))
                where += " AND A.iMachineID=" + query;
            if (!String.IsNullOrEmpty(status))
                where += " AND A.iStatusID1=" + status;
            //拼接排序
            string order = "";
            if (!String.IsNullOrEmpty(field) && !String.IsNullOrEmpty(sort) && field == "iMachineID")
                order = " ORDER BY " + field + " " + sort;
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT A.iMachineID,A.iStatusID1,A.iStatusID2,A.sWorkName,A.sTaskNo1
,A.sTaskNo2,A.nSpeed1,A.nSpeed2,A.nEfficiency1,A.nEfficiency2 
FROM dbo.psYarnMachine A(NOLOCK)" + where + order);
            List<Machine> list = TableListChange.TableToList<Machine>(data);
            return list;
        }
    }
}
