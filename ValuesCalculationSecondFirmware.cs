using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Meter_screening_application
{
    internal class ValuesCalculationSecondFirmware
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ValuesCalculationSecondFirmware));

        public bool check(byte[] arr)
        {
            if (arr.Length == 21 && arr[0] == 02 && arr[20] == 03)
            {
                log.Info("NOT PROVIDING EXACT OUTPUT FOR OLD FIRMWARE");
                return true;
            }
            return false;
        }
        private double GetFloat(byte[] buffer, int index, double defaultValue = float.MinValue)
        {
            try
            {
                double value = BitConverter.ToSingle(buffer, index);
                if (!double.IsNaN(value)) return value;
            }
            catch (Exception ex)
            {
            }
            return defaultValue;
        }
        public void ValueCalculation(byte[] arr , bool high , bool low  , double[,] meter_data , int i)
        {

            double voltage = GetFloat(arr, 6);
            double currentPhase = GetFloat(arr, 14);
            double currentNeutral = GetFloat(arr, 10);
            if(high == true)
            {
                meter_data[i, 0] = voltage;
                meter_data[i, 1] = currentPhase;
                meter_data[i, 2] = currentNeutral;
                log.Info($"value of voltage from meter {i} old firmware at higher side is {meter_data[i, 0]}");
                log.Info($"value of current phase from meter {i} old firmware at higher side is {meter_data[i, 1]}");
                log.Info($"value of current neutral from meter {i} old firmware at higher side is {meter_data[i, 2]}");
                log.Info("############################");
            }
            else
            {
                meter_data[i, 5] = voltage;
                meter_data[i, 6] = currentPhase;
                meter_data[i, 7] = currentNeutral;
                log.Info($"value of voltage from meter {i} old firmware at lower side is {meter_data[i, 5]}");
                log.Info($"value of current phase from meter {i} old firmware at lower side is {meter_data[i, 6]}");
                log.Info($"value of current neutral from meter {i} old firmware at lower side is {meter_data[i, 7]}");
                log.Info("############################");
            }
            
        }
    }
}
