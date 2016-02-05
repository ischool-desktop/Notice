using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA.UDT;

namespace Notice.DAO
{

    /// <summary>
    /// Notice
    /// </summary>
    [TableName("ischool.1campus.notice")]
    public class udtNotice:ActiveRecord
    {

        ///<summary>
        /// 標題
        ///</summary>
        [Field(Field = "title", Indexed = false)]
        public string Title { get; set; }


        ///<summary>
        /// 訊息
        ///</summary>
        [Field(Field = "message", Indexed = false)]
        public string Message { get; set; }


        ///<summary>
        /// 公告顯示時間
        ///</summary>
        [Field(Field = "post_time", Indexed = false)]
        public DateTime PostTime { get; set; }


        ///<summary>
        /// 發送者顯示名稱
        ///</summary>
        [Field(Field = "display_sender", Indexed = false)]
        public string DisplaySender { get; set; }


        ///<summary>
        /// 全校性公告
        ///</summary>
        [Field(Field = "global_notice", Indexed = false)]
        public bool GlobalNotice { get; set; }

        ///<summary>
        /// 發送教師系統編號
        ///</summary>
        [Field(Field = "ref_teacher_id", Indexed = true)]
        public int? TeacherID { get; set; }
    }
}
