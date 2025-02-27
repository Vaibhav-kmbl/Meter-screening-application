using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Meter_screening_application
{
    class MeterNumberFetch
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MeterNumberFetch));

        public bool check(byte[] data)
        {
            if(data.Length != 25) return false;
            return true;
        }

        //public string ConvertHexToAscii(byte[] hexBytes)
        //{
        //    byte[] extractedArray = new byte[16];

        //    Array.Copy(hexBytes,6, extractedArray, 0, 15);
        //    StringBuilder sb = new StringBuilder();

        //    foreach (byte b in extractedArray)
        //    {

        //        if (b >= 32 && b <= 126)
        //        {
        //            sb.Append((char)b);
        //            if (b == 3f) break;
        //        }
        //        else
        //        {
        //            continue;
        //         //   sb.Append('.');
        //        }
        //    }
        //    log.Info($"meter number is {sb.ToString()}");
        //    return sb.ToString();
        //}
        public string ConvertHexToAscii(byte[] hexBytes)
        {
            byte[] extractedArray = new byte[16];
            //Array.Copy(hexBytes, 6, extractedArray, 0, 16);
            //string asciiString = Encoding.ASCII.GetString(extractedArray);
            //log.Info($"meter number response string is {extractedArray.ToString()}");
            //log.Info($"Meter number is ----->>>     {asciiString}");
            //return asciiString;
          //  byte[] extractedArray = new byte[16];
            Array.Copy(hexBytes, 6, extractedArray, 0, 16);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in extractedArray)
            {
                if ((b >= 48 && b <= 57) || (b >= 65 && b <= 90) || (b >= 97 && b <= 122))
                {
                    sb.Append((char)b);
                    
                    
                }
                else if (sb.Length > 0)
                {
                    break;
                }

                //if (b >= 32 && b <= 126)
                //{
                //    sb.Append((char)b);
                //    if (b == 0x3F) break;
                //}
                //else
                //{
                //    continue;
                //}
            }

            return sb.ToString();
        }

    }
}
