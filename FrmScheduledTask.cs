using MssqlQuickBackup.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MssqlQuickBackup
{
    public partial class FrmScheduledTask : Form
    {
        ScheduledTaskManager scheduledTaskManager;
        private bool _mainTaskStatus;
        private Label _mainStatusLabel;
        private Button _mainBtnStartService;
        public FrmScheduledTask(bool mainTaskStatus, Label mainStatusLabel, Button mainBtnStartService)
        {
            scheduledTaskManager = new ScheduledTaskManager();
            _mainTaskStatus = mainTaskStatus;
            _mainStatusLabel = mainStatusLabel;
            _mainBtnStartService = mainBtnStartService;
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            scheduledTaskManager.CreateTask(datetimeStartDate.Value, (short)numericInterval.Value);
            _mainTaskStatus = true;
            _mainStatusLabel.Text = "Task Active";
            _mainBtnStartService.Text = "Remove Scheduled Task";
        }
    }
}
