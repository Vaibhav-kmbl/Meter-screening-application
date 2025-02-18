using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter_screening_application
{
    internal class VoltageCurrentCommandValueCheck
    {
        //public byte[] arr;
        //public VoltageCurrentCommandValueCheck(byte[] arr)
        //{
        //    this.arr = arr;
        //}
        public bool check(byte[] arr) {
            if (arr.Length != 70) return false;
            if (arr[0] != 02 && arr[69]!= 03) return false;
            return true;
        }
        double[] values_fetched = { 0, 0, 0 };
        public double[] result(byte[] arr)
        {
            if (!check(arr)) return values_fetched;
            values_fetched[0] = GetFloat(arr, 6);
            values_fetched[1] = GetFloat(arr, 10);
            values_fetched[2] = GetFloat(arr, 54);
            return values_fetched;
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

    }
   
}
