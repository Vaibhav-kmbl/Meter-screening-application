using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Meter_screening_application
{
    internal class BarcodeScannerService
    {
        public string meter_number(string str)
        {
            //string str2 = "";
            //foreach(char st in str)
            //{
            //    str2 = str2 + st;
            //    if (st == '(' || st == ' ') break;
            //}
            //return str2;
            string pattern = @"[A-Z]{1,4}\d{6,9}";

            MatchCollection matches = Regex.Matches(str, pattern);
            foreach (Match match in matches) { return match.Value; }
                return "";
          
        }
    }
}
