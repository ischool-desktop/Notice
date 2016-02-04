using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace Notice.DAO
{
    /// <summary>
    /// NoticeTarget
    /// </summary>
    [TableName("1campus.notice_target")]
    public class udtNoticeTarget:ActiveRecord
    {
        ///<summary>
        /// notice編號
        ///</summary>
        [Field(Field = "ref_notice_id", Indexed = true)]
        public int NoticeID { get; set; }

        ///<summary>
        /// group 編號
        ///</summary>
        [Field(Field = "ref_group_id", Indexed = true)]
        public int GroupID { get; set; }
    }
}
