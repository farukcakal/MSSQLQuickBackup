using System;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Linq;

namespace MssqlQuickBackup.Business
{
    public class BackupManager
    {
        public void StartBackup(string serverName, string databaseName, string userName, string password, string backupPath)
        {
            try
            {
                
                Server server = null;
                Backup backup = new Backup();

                if (Directory.Exists(backupPath))
                {
                    DirectoryInfo dir = new DirectoryInfo(backupPath);

                    DirectorySecurity dirSec = dir.GetAccessControl();

                    SecurityIdentifier everyoneSecId = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
                    FileSystemAccessRule writePerm = new FileSystemAccessRule(everyoneSecId, FileSystemRights.Write, AccessControlType.Allow);

                    if (!dirSec.GetAccessRules(true, true, typeof(SecurityIdentifier))
                                  .OfType<FileSystemAccessRule>()
                                  .Any(rule => rule.IdentityReference == everyoneSecId && rule.FileSystemRights == FileSystemRights.Write))
                    {
                        dirSec.AddAccessRule(writePerm);

                        dir.SetAccessControl(dirSec);
                    }
                }

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    server = new Server(new ServerConnection(serverName, userName, password));
                }
                else
                {
                    server = new Server(new ServerConnection(serverName));
                }

                backup.Action = BackupActionType.Database;
                backup.Database = databaseName;
                backup.Devices.AddDevice(backupPath + "\\" + DateTime.Now.ToString("dd-MM-yyyy-H-mm-ss") + ".bak", DeviceType.File);
                backup.Initialize = false;
                backup.SqlBackup(server);

                MessageBox.Show("Backup saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
