using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Helpers
{
    public static class EnumHelper
    {
        public static TEnum? GetEnumFromDescription<TEnum>(string description, IEnumDescriptionProvider<TEnum> provider) where TEnum : struct, Enum
        {
            if (description == "====Brak====")
            {
                return null;
            }

            foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
            {
                if (provider.GetDescription(value) == description)
                {
                    return value;
                }
            }

            throw new ArgumentException("Not found.", nameof(description));
        }
    }
}
