using FluentResults;
using System;
using System.Collections.Generic;

namespace Baubit.Traceability.Errors
{
    public abstract class AError : IError
    {
        public DateTime CreationTime { get; private set; } = DateTime.Now;
        public List<IError> Reasons { get; private set; } = new List<IError>();

        public virtual string Message { get; private set; }

        public Dictionary<string, object> Metadata { get; private set; }

        protected AError(List<IError> reasons, 
                         string message, 
                         Dictionary<string, object> metadata)
        {
            Reasons = reasons ?? new List<IError>();
            Message = message;
            Metadata = metadata;
        }
        protected AError() : this(new List<IError>(), string.Empty, new Dictionary<string, object>())
        {

        }
    }
}
