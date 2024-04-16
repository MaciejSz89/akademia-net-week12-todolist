using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Exceptions
{
    public interface IEntityOperationException
    {
        EntityType EntityType { get; }
        OperationType OperationType { get; }
    }
}
