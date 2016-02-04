using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.Presentation.Controls;
using FISCA.Data;
using Notice.DAO;

namespace Notice.UI
{
    public partial class AddNoticeForm : BaseForm
    {
        private int _newNoticeUID = 0;
        public AddNoticeForm()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (checkTextEmpty(txtTitle.Text))
                MsgBox.Show("標題必填。");

            if (checkTextEmpty(txtMessage.Text))
                MsgBox.Show("訊息必填。");

            if (checkTextEmpty(cboDisplaySender.Text))
                MsgBox.Show("發送者顯示名稱必填。");

            if (dtPostTime.IsEmpty)
                MsgBox.Show("公告顯示時間必填");
                
            btnAdd.Enabled = false;
            // 新增公告
            AddNotice();

            // 新增公告班級
            AddNoticeApprove();

            btnAdd.Enabled = true;
        }

        private bool checkTextEmpty(string text)
        {
            if (text.Replace(" ", "") == "")
                return true;
            else
                return false;
        }

        private void AddNotice()
        {
            _newNoticeUID = 0;
            udt_notice notice = new udt_notice();
            notice.Title = txtTitle.Text;
            notice.Message = txtMessage.Text;
            notice.DisplaySender = cboDisplaySender.Text;
            notice.PostTime = dtPostTime.Value;
            notice.GlobalNotice = true;            
            notice.Save();
            _newNoticeUID = int.Parse(notice.UID);            
        }

        /// <summary>
        /// 新增公告班級
        /// </summary>
        private void AddNoticeApprove()
        {
            if(_newNoticeUID==0)
            {
                MsgBox.Show("Notice 空值無法寫入");
            }
            else
            {
                // 取得有一般狀態班級ID                
                QueryHelper qh1 = new QueryHelper();
                string qry1 = "select class.id as cid from student inner join class on student.ref_class_id=class.id where student.status=1 group by class.id having count(student.id)>0 order by cid";
                DataTable dt1 = qh1.Select(qry1);
                List<int> ClassIDList = new List<int>();

                foreach(DataRow dr in dt1.Rows)
                {
                    int cid =int.Parse(dr["cid"].ToString());
                    if (!ClassIDList.Contains(cid))
                        ClassIDList.Add(cid);
                }

                List<udtNoticeApprove> udtNoticeApproveList = new List<udtNoticeApprove>();
                // 新增UDT資料
                foreach(int cid in ClassIDList)
                {
                    udtNoticeApprove data = new udtNoticeApprove();
                    data.Approve = false;
                    data.ClassID = cid;
                    data.NoticeID = _newNoticeUID;
                    udtNoticeApproveList.Add(data);
                }

                udtNoticeApproveList.SaveAll();
            }
        }

        private void AddNoticeForm_Load(object sender, EventArgs e)
        {
            LoadDefault();
        }

        private void LoadDefault()
        {
            dtPostTime.Value = DateTime.Now;
            cboDisplaySender.Items.Add("教務處");
            cboDisplaySender.Items.Add("學務處");
        }

    }
}
