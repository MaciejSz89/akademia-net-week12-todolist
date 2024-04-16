using System;
using System.Collections.Generic;
using System.Text;

namespace ToDoList.Exceptions
{
    public class EntityOperationException : Exception, IEntityOperationException
    {
        public EntityOperationException(EntityType entityType, OperationType operationType) : base()
        {
            
        }

        public EntityOperationException(EntityType entityType, OperationType operationType, string message) : base(message)
        {
        }

        public EntityOperationException(EntityType entityType, OperationType operationType, string message, Exception innerException) : base(message, innerException)
        {
        }

        public EntityType EntityType { get; }
        public OperationType OperationType { get; }
    }
}
