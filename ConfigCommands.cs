using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Meter_screening_application
{
    internal class ConfigCommands
    {
      public string MASTERMETER = ConfigurationManager.AppSettings["Master"];
        public int  delay = int.Parse(ConfigurationManager.AppSettings["delay on start"]);
        public int RETRYMETERAUTH = int.Parse(ConfigurationManager.AppSettings["meter auth retry"]);
        public int RETRY = int.Parse(ConfigurationManager.AppSettings["normal retry"]);
        public int TIMEOUT = int.Parse(ConfigurationManager.AppSettings["Timeout"]);
        public double tolerance = double.Parse(ConfigurationManager.AppSettings["tolerance value"]);
        public int TIMEOUTCURRENTVOLTAGECOMMAND = int.Parse(ConfigurationManager.AppSettings["timeout_for_current_voltage_command"]);
        public string filepath = ConfigurationManager.AppSettings["file path"];
        public string  METER1 = ConfigurationManager.AppSettings["Meter 1"];
           public string  METER2 = ConfigurationManager.AppSettings["Meter 2"];
         public string   JIG = ConfigurationManager.AppSettings["Jig"];
        public int DELAYAFTERCLOSINGJIG = int.Parse(ConfigurationManager.AppSettings["delay on closing jig"]);
        public int JIGONDELAY = int.Parse(ConfigurationManager.AppSettings["delay on jig on"]);
        public double CURRENTTOLERANCEPHASE5A = double.Parse(ConfigurationManager.AppSettings["phase Current tolerance 5A"]);
        public double CURRENTTOLERANCENEUTRAL5A = double.Parse(ConfigurationManager.AppSettings["neutral Current tolerance 5A"]);
        public double CURRENTTOLERANCENEUTRAL500mA = double.Parse(ConfigurationManager.AppSettings["neutral Current tolerance 500mA"]);
        public double CURRENTTOLERANCEPHASE500mA = double.Parse(ConfigurationManager.AppSettings["phase Current tolerance 500mA"]);
        public double PFTOLERANCEPHASE5A = double.Parse(ConfigurationManager.AppSettings["phase Pf tolerance 5A"]);
        public double PFTOLERANCENEUTRAL500mA = double.Parse(ConfigurationManager.AppSettings["neutral Pf tolerance 500mA"]);
        public double PFTOLERANCENEUTRAL5A = double.Parse(ConfigurationManager.AppSettings["neutral Pf tolerance 5A"]);
        public double PFTOLERANCEPHASE500mA = double.Parse(ConfigurationManager.AppSettings["phase Pf tolerance 500mA"]);
        public double VOLTAGETOLERANCE = double.Parse(ConfigurationManager.AppSettings["Voltage tolerance"]);
        public int TIMEOUTOLDFIRMWARE = int.Parse(ConfigurationManager.AppSettings["timeout for old firmware command"]);
        public int RETRIESOLDFIRMWARE = int.Parse(ConfigurationManager.AppSettings["retries for old firmware command"]);
        public int RTCDRIFTTIME = int.Parse(ConfigurationManager.AppSettings["RTC drift time value"]);
        public int RTCCHECKMAXRETRIES = int.Parse(ConfigurationManager.AppSettings["RTC check max retries"]);
        public int RTCCHECKTIMOUT= int.Parse(ConfigurationManager.AppSettings["RTC check timeout"]);






    }
}
