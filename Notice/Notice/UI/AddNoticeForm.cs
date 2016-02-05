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

        private StringBuilder _ErrorMessage = new StringBuilder();

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
                returnErr("標題必填。");

            if (checkTextEmpty(txtMessage.Text))
                returnErr("訊息必填。");

            if (checkTextEmpty(cboDisplaySender.Text))
                returnErr("發送者顯示名稱必填。");

            if (dtPostTime.IsEmpty)
                returnErr("公告顯示時間必填");
                
            btnAdd.Enabled = false;
            // 新增公告
            if(AddNotice())
            {
                // 新增公告班級
                if(AddNoticeApprove())
                {
                    MsgBox.Show("新增完成");
                    this.Close();
                }
                else
                {
                    btnAdd.Enabled = true;
                }
                
            }

            // 當有錯誤訊息
            if (_ErrorMessage.Length>0)
                MsgBox.Show(_ErrorMessage.ToString());
            
        }

        private void returnErr(string msg)
        {
            MsgBox.Show(msg);
            return;
        }

        private bool checkTextEmpty(string text)
        {
            if (text.Replace(" ", "") == "")
                return true;
            else
                return false;
        }

        private bool AddNotice()
        {
            bool pass = true;
            try
            {
                _newNoticeUID = 0;
                udtNotice notice = new udtNotice();
                notice.Title = txtTitle.Text;
                notice.Message = txtMessage.Text;
                notice.DisplaySender = cboDisplaySender.Text;
                notice.PostTime = dtPostTime.Value;
                notice.TeacherID = null;
                notice.GlobalNotice = true;
                notice.Save();
                _newNoticeUID = int.Parse(notice.UID);            

            }catch(Exception ex)
            {
                pass = false;
                _ErrorMessage.AppendLine("新增公告發生錯誤：");
                _ErrorMessage.AppendLine(ex.Message);                
            }
            return pass;            
        }

        /// <summary>
        /// 新增公告班級
        /// </summary>
        private bool AddNoticeApprove()
        {
            bool pass = true;
            try
            {
                if (_newNoticeUID == 0)
                {
                    _ErrorMessage.AppendLine("新增公告班級發生錯誤：Notice 空值無法寫入");                    
                }
                else
                {
                    // 取得有一般狀態班級ID                
                    QueryHelper qh1 = new QueryHelper();
                    string qry1 = "select class.id as cid from student inner join class on student.ref_class_id=class.id where student.status=1 group by class.id having count(student.id)>0 order by cid";
                    DataTable dt1 = qh1.Select(qry1);
                    List<int> ClassIDList = new List<int>();

                    foreach (DataRow dr in dt1.Rows)
                    {
                        int cid = int.Parse(dr["cid"].ToString());
                        if (!ClassIDList.Contains(cid))
                            ClassIDList.Add(cid);
                    }

                    List<udtNoticeApprove> udtNoticeApproveList = new List<udtNoticeApprove>();
                    // 新增UDT資料
                    foreach (int cid in ClassIDList)
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
            catch (Exception ex) { 
                pass = false;
                _ErrorMessage.AppendLine("新增公告班級發生錯誤：");
                _ErrorMessage.AppendLine(ex.Message);
            }
            return pass;
        }

        private void AddNoticeForm_Load(object sender, EventArgs e)
        {
            LoadDefault();
        }

        private void LoadDefault()
        {
            // 設定 PostTime 格式
            dtPostTime.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            dtPostTime.WatermarkText = "2016/1/1 23:59:59";
            dtPostTime.Value = DateTime.Now;
            cboDisplaySender.Items.Add("教務處");
            cboDisplaySender.Items.Add("學務處");
        }

    }
}
