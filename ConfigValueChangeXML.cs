using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Meter_screening_application
{
    internal class ConfigValueChangeXML : INotifyPropertyChanged
    {
        private static ConfigValueChangeXML _instance;
        public static ConfigValueChangeXML Instance => _instance ??= new ConfigValueChangeXML();
        private static readonly ILog log = LogManager.GetLogger(typeof(ConfigValueChangeXML));


        private string _voltageThreshold , _pfphase5Atol, _pfneutral5Atol, _pfphase500mAtol, _pfneutral500mAtol
            ,_currentphase5Atol , _currentneutral5Atol , _currentphase500mAtol  , _currentneutral500mAtol;

        public string VoltageTol
        {
            get => _voltageThreshold;
            set
            {
                _voltageThreshold = value;
                OnPropertyChanged(nameof(VoltageTol));
            }
        }
        public string CurrentPhase5ATol
        {
            get => _currentphase5Atol;
            set
            {
                _currentphase5Atol = value;
                OnPropertyChanged(nameof(CurrentPhase5ATol));
            }
        }
        public string CurrentNeutral5ATol
        {
            get => _currentneutral5Atol;
            set
            {
                _currentneutral5Atol = value;
                OnPropertyChanged(nameof(CurrentNeutral5ATol));
            }
        }
        public string CurrentPhase500mATol
        {
            
            get => _currentphase500mAtol;
            set
            {
                _currentphase500mAtol = value;
                OnPropertyChanged(nameof(CurrentPhase500mATol));
            }
        }
        public string CurrentNeutral500mATol
        {
            get => _currentphase500mAtol;
            set
            {
                _currentphase500mAtol = value;
                OnPropertyChanged(nameof(CurrentNeutral500mATol));
            }
        }
        public string PFPhase5ATol
        {
            get => _pfphase5Atol;
            set
            {
                _pfphase5Atol = value;
                OnPropertyChanged(nameof(PFPhase5ATol));
            }
        }
        public string PFNeutral5ATol
        {
            get => _pfneutral5Atol;
            set
            {
                 _pfneutral5Atol = value;
                OnPropertyChanged(nameof(PFNeutral5ATol));
            }
        }
        public string PFPhase500MATol
        {
            get => _pfphase500mAtol;
            set
            {
                _pfphase500mAtol = value;
                OnPropertyChanged(nameof(PFPhase500MATol));
            }
        }
        public string PFNeutral500mATol
        {
            get => _pfneutral500mAtol;
            set
            {
                _pfneutral500mAtol = value;
                OnPropertyChanged(nameof(PFNeutral500mATol));
            }
        }
        
        public ConfigValueChangeXML()
        {

           
            VoltageTol = XMLConfigValue.GetConfigValue("Voltage tolerance");
            CurrentPhase5ATol = XMLConfigValue.GetConfigValue("phase Current tolerance 5A");
            CurrentNeutral5ATol = XMLConfigValue.GetConfigValue("neutral Current tolerance 5A");
            CurrentPhase500mATol = XMLConfigValue.GetConfigValue("phase Current tolerance 500mA");
            CurrentNeutral500mATol = XMLConfigValue.GetConfigValue("neutral Current tolerance 500mA");
            PFPhase5ATol = XMLConfigValue.GetConfigValue("phase Current tolerance 5A");
            PFNeutral5ATol = XMLConfigValue.GetConfigValue("neutral Current tolerance 5A");
            PFPhase500MATol = XMLConfigValue.GetConfigValue("phase Pf tolerance 500mA");
            PFNeutral500mATol = XMLConfigValue.GetConfigValue("neutral Pf tolerance 500mA");
            log.Info($"---------{VoltageTol}");


        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
