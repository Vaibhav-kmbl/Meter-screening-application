using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter_screening_application
{
    public static class XMLConfigValue
    {
        public static string GetConfigValue(string key) => ConfigurationManager.AppSettings[key] ?? "N/A";

        public static string Voltage=> ConfigurationManager.AppSettings["VoltageThreshold"];
        public static string CurrentNeutral5A => ConfigurationManager.AppSettings["VoltageThreshold"];
        public static string CurrentPhase5A => ConfigurationManager.AppSettings["VoltageThreshold"];
        public static string PfNeutral5A => ConfigurationManager.AppSettings["VoltageThreshold"];
        public static string PfPhase5A => ConfigurationManager.AppSettings["VoltageThreshold"];
        public static string CurrentNeutral500mA => ConfigurationManager.AppSettings["VoltageThreshold"];
        public static string CurrentPhase500mA => ConfigurationManager.AppSettings["VoltageThreshold"];
        public static string PfPhase500mA => ConfigurationManager.AppSettings["VoltageThreshold"];
        public static string PfNeutral500mA => ConfigurationManager.AppSettings["VoltageThreshold"];




    }
}

