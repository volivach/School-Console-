using System;
using System.Runtime.Serialization;

namespace StudentsGroup
{
    [Serializable]
    internal class ClassFullException : Exception
    {
        public ClassFullException()
        {
        }

        public ClassFullException(string message) : base(message)
        {
        }

        public ClassFullException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClassFullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}