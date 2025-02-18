using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Meter_screening_application
{
    internal class RowIndexToConfigValueConverter : IValueConverter
    {
        private static RowIndexToConfigValueConverter _instance;
        public static RowIndexToConfigValueConverter Instance => _instance ??= new RowIndexToConfigValueConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           
            if (value is int rowIndex && rowIndex >= 1)
            {
                string configKey = parameter as string;
                if (!string.IsNullOrEmpty(configKey))
                {
                    return ConfigurationManager.AppSettings[configKey] ?? "N/A"; 
                }
            }
            return ""; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
