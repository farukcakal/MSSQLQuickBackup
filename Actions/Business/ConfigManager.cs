using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actions.Business
{
    public class ConfigManager
    {
        private const string configFilePath = "config.ini";
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
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
