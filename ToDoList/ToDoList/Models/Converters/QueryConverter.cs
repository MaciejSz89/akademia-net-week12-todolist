using System;
using System.Reflection;
using System.Text;

namespace ToDoList.Models.Converters
{
    public static class QueryConverter
    {
        public static string ToQueryString(this object obj)
        {
            var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var queryStringBuilder = new StringBuilder();


            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                if (value != null)
                {
                    if (queryStringBuilder.Length > 0)
                        queryStringBuilder.Append("&");
                    else 
                        queryStringBuilder.Append("?");

                    queryStringBuilder.Append(property.Name);
                    queryStringBuilder.Append("=");
                    queryStringBuilder.Append(Uri.EscapeDataString(value.ToString()));
                }
            }

            return queryStringBuilder.ToString();
        }
    }
}
