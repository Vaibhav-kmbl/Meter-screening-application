using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter_screening_application
{
    internal class ValuesCalculationSecondFirmware
    {


   public bool check(byte[] arr)
        {
            if (arr.Length == 21 && arr[0] == 02 && arr[20] == 03) return true;
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
            double currentPhase = GetFloat(arr, 10);
            double currentNeutral = GetFloat(arr, 14);
            if(high == true)
            {
                meter_data[i, 0] = voltage;
                meter_data[i, 1] = currentPhase;
                meter_data[i, 2] = currentNeutral;
            }
            else
            {
                meter_data[i, 3] = voltage;
                meter_data[i, 4] = currentPhase;
                meter_data[i, 5] = currentNeutral;
            }
        }
    }
}
