using MssqlQuickBackup.Business;
using System.Windows.Forms;

namespace MssqlQuickBackup
{
    public partial class FrmMain : Form
    {
        ConfigHelper configHelper;
        BackupHelper backupHelper;
        public FrmMain()
        {
            InitializeComponent();
            configHelper = new ConfigHelper();
            backupHelper = new BackupHelper();
        }

        private void FillTextBoxes()
        {
            string serverName, databaseName, userName, password, backupPath;
            configHelper.LoadConfig(out serverName, out databaseName, out userName, out password, out backupPath);

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

            configHelper.SaveConfig(serverName, databaseName, userName, password, backupPath);
        }

        private void btnSaveSettings_Click(object sender, System.EventArgs e)
        {
            SaveConfig();
        }

        private void FrmMain_Load(object sender, System.EventArgs e)
        {
            FillTextBoxes();
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
            backupHelper.StartBackup(txtServerName.Text, txtDatabaseName.Text, txtUsername.Text, txtPassword.Text, txtPath.Text);
        }
    }
}
