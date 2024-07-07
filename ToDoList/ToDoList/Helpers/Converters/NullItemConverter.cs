using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using static Android.Content.ClipData;
using Xamarin.Forms;

namespace ToDoList.Helpers.Converters
{
    public class NullableItemConverter<T> : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            var values = (object[])parameter;
            if (values.Length < 2)
                throw new ArgumentException("Two values must be provided: propertyName and nullValue.");

            var propertyName = values[0] as string;
            var nullValue = values[1];

            if (value == null)
            {
                return nullValue;
            }
            else
            {
                var itemType = value.GetType();
                var property = itemType.GetProperty(propertyName);
                if (property != null)
                {
                    return property.GetValue(value);
                }
            }

            return value; // Default fallback
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
