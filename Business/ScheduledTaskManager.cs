using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace MssqlQuickBackup.Business
{
    public class ScheduledTaskManager
    {
        public void CreateTask(DateTime startDate, short daysInterval)
        {
            using (TaskService ts = new TaskService())
            {
                // Yeni bir görev tanımı oluştur.
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "MQB";

                // Görevi SYSTEM hesabı altında çalışacak şekilde ayarla.
                td.Principal.UserId = "SYSTEM"; // Alternatif olarak "NT AUTHORITY\\SYSTEM"
                td.Principal.LogonType = TaskLogonType.ServiceAccount;

                // Belirtilen başlangıç tarihi ve aralığa göre günlük tetikleyici ekle.
                DailyTrigger dailyTrigger = new DailyTrigger
                {
                    StartBoundary = startDate,
                    DaysInterval = daysInterval
                };
                td.Triggers.Add(dailyTrigger);

                // Actions.exe dosyasının proje dizinindeki "actions" klasöründe olup olmadığını kontrol et.
                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string executablePath = Path.Combine(projectDirectory, "actions", "Actions.exe");

                // Çalıştırılabilir dosyanın var olup olmadığını doğrula.
                if (!File.Exists(executablePath))
                {
                    MessageBox.Show($"The executable at path '{executablePath}' was not found.");
                    return;
                }

                // Belirtilen çalıştırılabilir dosyayı çalıştıracak bir eylem ekle.
                td.Actions.Add(new ExecAction(executablePath, null, null));

                // Görevi kök klasöründe kaydet.
                ts.RootFolder.RegisterTaskDefinition("MQB", td);

                MessageBox.Show("Scheduled Task created successfully!");
            }
        }

        public void DeleteTask() 
        {
            using (TaskService ts = new TaskService())
            {
                ts.RootFolder.DeleteTask("MQB", false);
                MessageBox.Show("Scheduled Task deleted successfully!");
            }
        }

        public string GetNextRunTime()
        {
            using (TaskService ts = new TaskService())
            {
                Task task = ts.FindTask("MQB");
                if (task != null)
                {
                    return task.NextRunTime.ToString("dd.MM.yyyy H:m:s");
                }
                else
                {
                    return "";
                }
            }
        }

        public bool CheckTaskStatus()
        {
            using (TaskService ts = new TaskService())
            {
                Task task = ts.FindTask("MQB");
                if (task != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
