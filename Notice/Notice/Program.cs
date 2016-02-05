using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA;
using FISCA.Permission;
using FISCA.Presentation;
using System.ComponentModel;
using FISCA.Presentation.Controls;

namespace Notice
{
    public class Program
    {
        static BackgroundWorker _bgLLoadUDT = new BackgroundWorker();

        static string _ErrorMessage = "";

        [MainMethod()]
        public static void Main()        
        {
            _bgLLoadUDT.DoWork += _bgLLoadUDT_DoWork;
            _bgLLoadUDT.RunWorkerCompleted += _bgLLoadUDT_RunWorkerCompleted;
            _bgLLoadUDT.RunWorkerAsync();            
        }

        static void _bgLLoadUDT_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_ErrorMessage))
                MsgBox.Show(_ErrorMessage);

            string _addNotice_code = "Notice.UI.AddNoticeForm";
            string _NoticeApproveForm_code = "Notice.UI.NoticeApproveForm";
            // 全校公告
            Catalog catalog01 = RoleAclSource.Instance["教務作業"]["全校公告"];

            catalog01.Add(new RibbonFeature(_addNotice_code, "新增公告"));
            catalog01.Add(new RibbonFeature(_NoticeApproveForm_code, "公告允許顯示設定"));

            RibbonBarItem item01 = K12.Presentation.NLDPanels.Student.RibbonBarItems["全校公告"];
            item01["新增公告"].Enable = UserAcl.Current[_addNotice_code].Executable;
            item01["新增公告"].Click += delegate
            {
                UI.AddNoticeForm nf = new UI.AddNoticeForm();
                nf.ShowDialog();
            };

            RibbonBarItem item02 = K12.Presentation.NLDPanels.Student.RibbonBarItems["全校公告"];
            item02["公告允許顯示設定"].Enable = UserAcl.Current[_NoticeApproveForm_code].Executable;
            item02["公告允許顯示設定"].Click += delegate
            {
                UI.NoticeApproveForm naf = new UI.NoticeApproveForm();
                naf.ShowDialog();
            };

        }

        static void _bgLLoadUDT_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // 建立使用到 UDT
                Utility.CreateUDTTable();
            }catch(Exception ex)
            {
                _ErrorMessage = "建立UDT發生錯誤," + ex.Message;
            }            
        }
    }
}
