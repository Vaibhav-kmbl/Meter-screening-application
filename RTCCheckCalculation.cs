using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using log4net;

namespace Meter_screening_application
{
    internal class RTCCheckCalculation
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RTCCheckCalculation));
        public bool Check(byte[] arr)
        {
            if (arr.Length != 18 && arr[0] != 02 && arr[2] != 0F) return false;
            return true;

        }
        //public byte[] fetch(byte[] arr2)
        //{
        //    log.Info($"RTC Check for meter is {arr2[6]} ,{arr2[7]}, {arr2[14]}");
        //    byte[] arr = ExtractBytes(arr2 , 6,14);
            
        //   return arr;
        //}
        //public byte[] ExtractBytes(byte[] arr, int start, int end)
        //{
        //    if (arr == null || start < 0 || end >= arr.Length || start > end) {
        //        throw new ArgumentException("Invalid start or end index");
        //    }              
        //    int length = end - start + 1;
        //    byte[] result = new byte[length];
        //    Array.Copy(arr, start, result, 0, length);
        //    return result;
        //}
        public bool FinalResult(byte[] arr2 , string[] RTCvalues , int j)
        {
            ConfigCommands configCommands = new ConfigCommands();
            //   byte[] arr = fetch(arr2);
            int[] arr3 = new int[18];
            try
            { 
                for(int i = 0; i < 18; i++)
                {
                    arr3[i] = BcdToInt(arr2[i]);
                   // arr3[i] = int.Parse(arr2, System.Globalization.NumberStyles.HexNumber);
                }
               
            }
            catch(Exception ex) {
                log.Error(ex + "on RTC from meter {j}");
            }
            int second = arr3[6];
                int minute = arr3[7];
            int hour = arr3[8];
            int day = arr3[9];
            int month = arr3[11];
            int year = (arr3[12] * 100) + arr3[13];
            log.Info($"{second} , {minute} , {hour} , {day} , {month} , {year} for meter {j}");
            DateTime current = new DateTime(year, month, day, hour, minute, second);
            log.Info($"Date time for meter fetched is {current.ToString()} for meter {j}");
            RTCvalues[j] = Math.Abs(Math.Round((DateTime.Now - current).TotalSeconds,3)).ToString();
          if (Math.Abs((DateTime.Now - current).TotalSeconds) <= configCommands.RTCDRIFTTIME)  return true;
            log.Info($"RTC VALUE meter fetched i--->>>> {(DateTime.Now - current).TotalSeconds} for meter {j}");

            return false;

        }
        public int BcdToInt(byte bcd)
        {
            return ((bcd >> 4) * 10) + (bcd & 0x0F);
        }

    }
}
