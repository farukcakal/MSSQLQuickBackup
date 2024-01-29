using System;
using System.IO;
using System.Windows.Forms;

namespace MssqlQuickBackup.Business
{
    public class ConfigHelper
    {
        private const string configFilePath = "config.ini";

        public void SaveConfig(string serverName, string databaseName, string userName, string password, string backupPath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(configFilePath))
                {
                    writer.WriteLine($"ServerName={serverName}");
                    writer.WriteLine($"DatabaseName={databaseName}");
                    writer.WriteLine($"UserName={userName}");
                    writer.WriteLine($"Password={password}");
                    writer.WriteLine($"BackupPath={backupPath}");
                }

                MessageBox.Show("Config saved successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void LoadConfig(out string serverName, out string databaseName, out string userName, out string password, out string backupPath)
        {
            serverName = "";
            databaseName = "";
            userName = "";
            password = "";
            backupPath = "";

            try
            {
                if (File.Exists(configFilePath))
                {
                    using (StreamReader reader = new StreamReader(configFilePath))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            string[] parts = line.Split('=');

                            if (parts.Length == 2)
                            {
                                switch (parts[0])
                                {
                                    case "ServerName":
                                        serverName = parts[1];
                                        break;
                                    case "DatabaseName":
                                        databaseName = parts[1];
                                        break;
                                    case "UserName":
                                        userName = parts[1];
                                        break;
                                    case "Password":
                                        password = parts[1];
                                        break;
                                    case "BackupPath":
                                        backupPath = parts[1];
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
