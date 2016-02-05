using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FISCA.UDT;
using FISCA.Presentation.Controls;
using Notice.DAO;
using K12.Data;

namespace Notice.UI
{
    public partial class NoticeApproveForm : BaseForm
    {
        // 載入資料
        BackgroundWorker _bgLoadData;

        Dictionary<string, string> _TeacherNameDict;
        Dictionary<string, udtNotice> _NoticeDict;
        Dictionary<string, List<udtNoticeApprove>> _NoticeApproveDict;

        public NoticeApproveForm()
        {
            InitializeComponent();
            _bgLoadData = new BackgroundWorker();
            _bgLoadData.DoWork += _bgLoadData_DoWork;
            _bgLoadData.RunWorkerCompleted += _bgLoadData_RunWorkerCompleted;
            _TeacherNameDict = new Dictionary<string, string>();
            _NoticeDict = new Dictionary<string, udtNotice>();
            _NoticeApproveDict = new Dictionary<string, List<udtNoticeApprove>>();
        }

        void _bgLoadData_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            LoadDataToDataGrid();
            btnSave.Enabled = true;
        }

        void _bgLoadData_DoWork(object sender, DoWorkEventArgs e)
        {
            _TeacherNameDict.Clear();
            // 取得教師資料
            List<TeacherRecord> tRecList = Teacher.SelectAll();
            foreach(TeacherRecord rec in tRecList)
                if(rec.Status== TeacherRecord.TeacherStatus.一般)
                    if(!_TeacherNameDict.ContainsKey(rec.ID))
                    {
                        string TName = rec.Name;

                        if (!string.IsNullOrEmpty(rec.Nickname))
                            TName += "(" + rec.Nickname + ")";

                        _TeacherNameDict.Add(rec.ID, TName);
                    }


            // 取得公告資料
            AccessHelper accHelper = new AccessHelper();
            List<udtNotice> udtDataList = accHelper.Select<udtNotice>();
            _NoticeDict.Clear();
            udtDataList = udtDataList.OrderBy(x => x.PostTime).ToList();
            foreach(udtNotice data in udtDataList)
            {
                if (!_NoticeDict.ContainsKey(data.UID))
                    _NoticeDict.Add(data.UID, data);
            }

            // 取得需要允許的班級公告資料
            _NoticeApproveDict.Clear();

            if(_NoticeDict.Keys.Count>0)
            {
                string query="ref_notice_id in("+string.Join(",",_NoticeDict.Keys.ToArray())+")";
                List<udtNoticeApprove> apDataList = accHelper.Select<udtNoticeApprove>(query);
                foreach(udtNoticeApprove data in apDataList)
                {
                    string nid=data.NoticeID.ToString();
                    if (!_NoticeApproveDict.ContainsKey(nid))
                        _NoticeApproveDict.Add(nid, new List<udtNoticeApprove>());

                    _NoticeApproveDict[nid].Add(data);
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // 讀取畫面是否允許設定
            Dictionary<string, bool?> appDict = new Dictionary<string, bool?>();
            
            
        }

        private void LoadDataToDataGrid()
        {
            dgData.Rows.Clear();
            if(_NoticeDict.Count>0)
            {
               
                foreach(string key in _NoticeDict.Keys)
                {
                    int rowIdx = dgData.Rows.Add();
                    dgData.Rows[rowIdx].Tag = key;
                    dgData.Rows[rowIdx].Cells[colTitle.Index].Value = _NoticeDict[key].Title;
                    dgData.Rows[rowIdx].Cells[colMessage.Index].Value = _NoticeDict[key].Message;
                    dgData.Rows[rowIdx].Cells[colPostTime.Index].Value = _NoticeDict[key].PostTime.ToString();
                    dgData.Rows[rowIdx].Cells[colDisplaySender.Index].Value = _NoticeDict[key].DisplaySender;
                    if (_NoticeDict[key].GlobalNotice)
                        dgData.Rows[rowIdx].Cells[colGlobalNotice.Index].Value = "是";
                    else
                        dgData.Rows[rowIdx].Cells[colGlobalNotice.Index].Value = "否";

                    // 判斷是否允許
                    dgData.Rows[rowIdx].Cells[colApprove.Index].Value = "";
                    if(_NoticeApproveDict.ContainsKey(key))
                    {
                        if(_NoticeApproveDict[key].Count>0)
                        {
                            if(_NoticeApproveDict[key][0].Approve.HasValue)
                            {
                                if (_NoticeApproveDict[key][0].Approve.Value)
                                    dgData.Rows[rowIdx].Cells[colApprove.Index].Value = "是";
                                else
                                    dgData.Rows[rowIdx].Cells[colApprove.Index].Value = "否";
                            }
                        }
                    }

                    // 教師姓名
                    if (_NoticeDict[key].TeacherID.HasValue)
                    {
                        string tid = _NoticeDict[key].TeacherID.Value.ToString();
                        if (_TeacherNameDict.ContainsKey(tid))
                            dgData.Rows[rowIdx].Cells[colTeacher.Index].Value = _TeacherNameDict[tid];
                    }
                    else
                        dgData.Rows[rowIdx].Cells[colTeacher.Index].Value = "";
                    
                }
            }
        }

        private void NoticeApproveForm_Load(object sender, EventArgs e)
        {
            colApprove.Items.Add("");
            colApprove.Items.Add("是");
            colApprove.Items.Add("否");
            colApprove.DropDownStyle = ComboBoxStyle.DropDownList;

            btnSave.Enabled = false;
            _bgLoadData.RunWorkerAsync();
        }
    }
}
