using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meter_screening_application
{
    internal class AverageCalculation
    {
        double[] finalresult = new double[3];
        public double[] AverageValue(double[,] arr)
        {
            double current = 0, voltage = 0, pf = 0;
            double n = ValueOfDenomenator(arr);
            for(int i = 0; i <= 2; i++)
            {
                current += arr[i, 0];
                voltage += arr[i, 1];
                pf += arr[i, 2];
            }
            finalresult[0] = current/n;
            finalresult[1] = voltage/n;
            finalresult[2] = pf/n;

            return finalresult;
        }
        public double ValueOfDenomenator(double[,] arr)
        {
            double count = 0;
          for(int i = 0; i <= 2; i++)
            {
                if (arr[0,i] > 0) count++;
            }
            return count;
        }
    }
}
