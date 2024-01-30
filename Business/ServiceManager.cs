using System;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MssqlQuickBackup.Business
{
    public class ServiceManager
    {
        private string serviceName = "QuickServiceBackup";
        private string servicePath = @"C:\QuickServiceBackup.exe";

        public void InstallAndStartService()
        {
            try
            {
                using (ServiceController serviceController = new ServiceController(serviceName))
                {
                    if (!ServiceExists(serviceName))
                    {
                        MessageBox.Show($"Service '{serviceName}' is not installed. Installing...");

                        ManagedInstallerClass.InstallHelper(new string[] { servicePath });
                        MessageBox.Show($"Service '{serviceName}' installed successfully.");
                    }

                    StartService(serviceController);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error installing and starting service: {ex.Message}");
            }
        }

        private void StartService(ServiceController serviceController)
        {
            try
            {
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running);
                MessageBox.Show($"Service '{serviceController.ServiceName}' started successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting service: {ex.Message}");
            }
        }

        private bool ServiceExists(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController service in services)
            {
                if (service.ServiceName == serviceName)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
