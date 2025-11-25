using FluentResults;
using System;
using System.Collections.Generic;

namespace Baubit.Traceability.Reasons
{
    public abstract class AReason : IReason
    {
        public virtual string Message { get; private set; }
        public DateTime CreationTime { get; private set; } = DateTime.Now;

        public Dictionary<string, object> Metadata { get; private set; }
        protected AReason(string message, Dictionary<string, object> metadata)
        {
            Message = message;
            Metadata = metadata;
        }

        protected AReason() : this(string.Empty, new Dictionary<string, object>())
        {

        }

        public override string ToString()
        {
            return Message;
        }
    }
}
