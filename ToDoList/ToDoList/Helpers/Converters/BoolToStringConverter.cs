using System;
using System.Globalization;
using Xamarin.Forms;

namespace ToDoList.Helpers.Converters
{
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                string trueText = parameter.ToString().Split('|')[0];
                string falseText = parameter.ToString().Split('|')[1];

                return boolValue ? trueText : falseText;
            }

            throw new ArgumentException("Value is not a boolean.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
