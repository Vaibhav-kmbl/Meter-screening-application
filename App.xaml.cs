using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using log4net;
using log4net.Config;
using Newtonsoft.Json;

namespace Meter_screening_application
{
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //string logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            //if (!Directory.Exists(logsDirectory))
            //{
            //    Directory.CreateDirectory(logsDirectory);
            //}

            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            log.Info("Application started");
            log.Info($"Config info is: {JsonConvert.SerializeObject(ConfigurationManager.AppSettings)}");
        }
    }

}
