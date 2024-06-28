using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Helpers
{
    public interface IEnumDescriptionProvider<TEnum> where TEnum : struct, Enum
    {
        IEnumerable<string> GetDescriptions();
        string GetDescription(TEnum? value);
    }
}
