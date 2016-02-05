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
    [TableName("ischool.1campus.notice_log")]
    public class udtNoticeLog:ActiveRecord
    {

        ///<summary>
        /// Notice ID
        ///</summary>
        [Field(Field = "ref_notice_id", Indexed = true)]
        public int NoticeID { get; set; }

        ///<summary>
        /// 學生系統編號
        ///</summary>
        [Field(Field = "ref_student_id", Indexed = true)]
        public int StudentID { get; set; }



        ///<summary>
        /// Parent ID
        ///</summary>
        [Field(Field = "ref_parent_id", Indexed = false)]
        public int? ParentID { get; set; }


        ///<summary>
        /// 時間
        ///</summary>
        [Field(Field = "time", Indexed = false)]
        public DateTime Time { get; set; }



    }
}
