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
WHERE A.bUsable=1 AND B.bUsable=1 AND C.bUsable=1
AND A.iMachineGroupID=:machineType
GROUP BY C.iaddress,C.saddress", machineType);
            return data;
        }

        /// <summary>
        /// 获取参数名称和明细个数
        /// </summary>
        public DataTable GetMachineVarAndCount(string MachineNo, string MachineVar, string MachineType, string TimeStart, string TimeEnd)
        {
            //获取数据集
            DataTable data = DBHelper.DbContext().m_ExecuteReader(@"SELECT A.saddressName,count=COUNT(1)
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
AND A.tCreateTime>=:TimeStart AND A.tCreateTime<=:TimeEnd", MachineType, MachineNo, MachineVar, TimeStart, TimeEnd);
            return data;
        }
    }
}
