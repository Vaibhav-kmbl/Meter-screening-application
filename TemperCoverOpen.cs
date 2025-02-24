using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json.Linq;

namespace Meter_screening_application
{

    internal class TemperCoverOpen
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TemperCoverOpen));

        public string ByteConversion(byte by ,int i)
        {
            string binaryString = Convert.ToString(by, 2).PadLeft(8, '0');
            log.Info($"byte coming {by} and string --->> {binaryString} for meter {i}");
            return binaryString;

        }
        public bool CoverOpen(byte by ,int i)
        {
            string str = ByteConversion(by , i);
            if (str[7] == '0') return true;
            return false;
        }         
        public bool Magnet(byte by , int i)
        {
            string str = ByteConversion(by, i);
            if (str[2] == '0') return true;
            return false;
        }
    }
}
