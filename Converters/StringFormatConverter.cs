using System;
using Windows.UI.Xaml.Data;

namespace MondoUniversalWindowsSample.Converters
{
    public sealed class StringFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return null;
            }

            if (parameter == null)
            {
                return value;
            }

            return ((IFormattable)value).ToString((string)parameter, null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
