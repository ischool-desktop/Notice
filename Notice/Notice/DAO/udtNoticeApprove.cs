using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace Notice.DAO
{
    /// <summary>
    /// NoticeApprove
    /// </summary>
    [TableName("ischool.1campus.notice_approve")]
    public class udtNoticeApprove:ActiveRecord
    {
        ///<summary>
        /// notice編號
        ///</summary>
        [Field(Field = "ref_notice_id", Indexed = true)]
        public int NoticeID { get; set; }

        ///<summary>
        /// 班級系統編號
        ///</summary>
        [Field(Field = "ref_class_id", Indexed = true)]
        public int ClassID { get; set; }

        ///<summary>
        /// 允許顯示
        ///</summary>
        [Field(Field = "approve", Indexed = false)]
        public bool? Approve { get; set; }
    }
}
