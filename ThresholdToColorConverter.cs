using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Meter_screening_application
{
    internal class ThresholdToColorConverter :IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                
               // int threshold = ConfigManager.ThresholdValue;

                // Check condition and return color
              //  return intValue > threshold ? Brushes.Red : Brushes.Green;
            }
            return Brushes.Black; // Default color for unexpected cases
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
