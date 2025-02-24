using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter_screening_application
{
  public  class data
    {
        public data(string Name, string Voltage, string VoltageTolerance, string CurrentNeutral, string CurrentPhase ,string CurrentNeutralTolerance,string CurrentPhaseTolerance, string PFNeutral, string PFPhase , string PFNeutralTolerance, string PFPhaseTolerance,string RTCCheck, string RTCValue , string CoverOpen , string Magnet , string Result)
        {
            this.Name = Name;
            this.Voltage = Voltage;
            this.VoltageTolerance = VoltageTolerance;
            this.CurrentNeutral = CurrentNeutral;
            this.CurrentPhase = CurrentPhase;
            this.CurrentNeutralTolerance = CurrentNeutralTolerance;
            this.CurrentPhaseTolerance = CurrentPhaseTolerance;

            this.PFNeutral = PFNeutral;
            this.PFPhase = PFPhase;
            this.PFNeutralTolerance = PFNeutralTolerance;
            this.PFPhaseTolerance = PFPhaseTolerance;
            this.RTCCheck = RTCCheck;
            this.RTCValue = RTCValue;
            this.CoverOpen = CoverOpen;
            this.Magnet = Magnet;   
            this.Result = Result;
            
        }
        public string Name { get; private set; }
        public string Voltage { get; set; }

        public string VoltageTolerance { get; set; }

        public string CurrentNeutral { get; set; }
        public string CurrentPhase { get; set; }
        public string CurrentNeutralTolerance { get; set; }
        public string CurrentPhaseTolerance { get; set; }
        public string PFNeutral { get; set; }
        public string PFPhase { get; set; }
        public string PFNeutralTolerance { get; set; }
        public string  PFPhaseTolerance { get; set; }
        public string RTCCheck { get; set; }
        public string RTCValue { get; set; }    
        public string Result { get; set; }
        public string CoverOpen { get; set; }

        public string Magnet{ get; set; }

    
}
}
