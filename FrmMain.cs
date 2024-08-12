using MssqlQuickBackup.Business;
using System.Windows.Forms;

namespace MssqlQuickBackup
{
    public partial class FrmMain : Form
    {
        ConfigManager configManager;
        BackupManager backupManager;
        ScheduledTaskManager scheduledTaskManager;
        public bool scheduledTaskStatus = false; 
        public FrmMain()
        {
            InitializeComponent();
            configManager = new ConfigManager();
            backupManager = new BackupManager();
            scheduledTaskManager = new ScheduledTaskManager();
            scheduledTaskStatus = scheduledTaskManager.CheckTaskStatus();
        }

        private void FillTextBoxes()
        {
            string serverName, databaseName, userName, password, backupPath;
            configManager.LoadConfig(out serverName, out databaseName, out userName, out password, out backupPath);

            txtServerName.Text = serverName;
            txtDatabaseName.Text = databaseName;
            txtUsername.Text = userName;
            txtPassword.Text = password;
            txtPath.Text = backupPath;
        }
        private void SaveConfig()
        {
            string serverName = txtServerName.Text;
            string databaseName = txtDatabaseName.Text;
            string userName = txtUsername.Text;
            string password = txtPassword.Text;
            string backupPath = txtPath.Text;

            configManager.SaveConfig(serverName, databaseName, userName, password, backupPath);
        }

        private void btnSaveSettings_Click(object sender, System.EventArgs e)
        {
            SaveConfig();
        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            FillTextBoxes();
            lblTaskStatus.Text = scheduledTaskStatus ? "Task Active" : "Task Passive";
            btnStartService.Text = scheduledTaskStatus ? "Remove Scheduled Task" : "Create Scheduled Task";
        }

        private void btnSelectPath_Click(object sender, System.EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Yedeklemenin yapılacağı klasörü seçin.";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string backupPath = folderBrowserDialog.SelectedPath;
                    txtPath.Text = backupPath;
                }
            }
        }

        private void btnBackup_Click(object sender, System.EventArgs e)
        {
            backupManager.StartBackup(txtServerName.Text, txtDatabaseName.Text, txtUsername.Text, txtPassword.Text, txtPath.Text);
        }

        private void btnStartService_Click(object sender, System.EventArgs e)
        {
            if(!scheduledTaskStatus)
            {
                FrmScheduledTask frm = new FrmScheduledTask(scheduledTaskStatus, lblTaskStatus, btnStartService);
                frm.ShowDialog();
            }
            else
            {
                scheduledTaskManager.DeleteTask();
                scheduledTaskStatus = false;
                lblTaskStatus.Text = "Task Passive";
            }
        }
    }
}
