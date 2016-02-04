using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FISCA;
using FISCA.Permission;
using FISCA.Presentation;
using System.ComponentModel;

namespace Notice
{
    public class Program
    {
        static BackgroundWorker _bgLLoadUDT = new BackgroundWorker();

        [MainMethod()]
        public static void Main()        
        {
            _bgLLoadUDT.DoWork += _bgLLoadUDT_DoWork;
            _bgLLoadUDT.RunWorkerCompleted += _bgLLoadUDT_RunWorkerCompleted;
            _bgLLoadUDT.RunWorkerAsync();            
        }

        static void _bgLLoadUDT_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string _addNotice_code = "Notice.UI.AddNoticeForm";

            // 全校公告
            Catalog catalog01 = RoleAclSource.Instance["教務作業"]["全校公告"];

            catalog01.Add(new RibbonFeature(_addNotice_code, "新增公告"));

            RibbonBarItem item01 = K12.Presentation.NLDPanels.Student.RibbonBarItems["全校公告"];
            item01["新增公告"].Enable = UserAcl.Current[_addNotice_code].Executable;
            item01["新增公告"].Click += delegate
            {
                UI.AddNoticeForm nf = new UI.AddNoticeForm();
                nf.ShowDialog();
            };

        }

        static void _bgLLoadUDT_DoWork(object sender, DoWorkEventArgs e)
        {
            // 建立使用到 UDT
            Utility.CreateUDTTable();
        }
    }
}
