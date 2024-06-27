using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Helpers
{
    public interface IEnumDescriptionProvider<TEnum> where TEnum : struct, Enum
    {
        string GetDescription(TEnum? value);
    }
}
