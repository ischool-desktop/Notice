using FISCA.UDT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Notice.DAO;

namespace Notice
{
    public class Utility
    {
        /// <summary>
        /// 建立使用到的 UDT Table
        /// </summary>
        public static void CreateUDTTable()
        {            
            FISCA.UDT.SchemaManager Manager = new SchemaManager(new FISCA.DSAUtil.DSConnection(FISCA.Authentication.DSAServices.DefaultDataSource));
            Manager.SyncSchema(new udtNotice());
            Manager.SyncSchema(new udtNoticeApprove());
            Manager.SyncSchema(new udtNoticeLog());
            Manager.SyncSchema(new udtNoticeTarget());
        }
    }
}
