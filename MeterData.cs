using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter_screening_application
{
    internal class MeterData
    {
        public string MeterNumberFromOptical { get; set; }
        public string MeterNumberFromScanner { get; set; }
       public bool MeterNumberMatch { get; set; }
        public int meterAuthenticate { get; set; }
        public bool RTCDrift { get; set; }
        public bool CoverOpen { get; set; }
        public bool Magnet {  get; set; }
        public int RTCDriftValue { get; set; }
        
    }
}
