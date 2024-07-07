using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace ToDoList.Helpers.Converters
{
    public class ItemToIdConverter<T>: IValueConverter where T : class
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var itemType = typeof(T);
            var idProperty = itemType.GetProperty("Id");
            return idProperty?.GetValue(value);
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var id = value as int?;
            if (id == null || parameter == null) return null;

            var items = parameter as IEnumerable<T>;
            if (items == null) return null;

            var itemType = typeof(T);
            var idProperty = itemType.GetProperty("Id");
            return items.FirstOrDefault(item => (int?)idProperty?.GetValue(item) == id);
        }
    }
}
