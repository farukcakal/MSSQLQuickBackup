using Actions.Business;

Console.WriteLine("Started");

BackupManager backupManager = new BackupManager();
ConfigManager configManager = new ConfigManager();

string serverName, databaseName, userName, password, backupPath;
configManager.LoadConfig(out serverName, out databaseName, out userName, out password, out backupPath);

backupManager.StartBackup(serverName, databaseName, userName, password, backupPath);