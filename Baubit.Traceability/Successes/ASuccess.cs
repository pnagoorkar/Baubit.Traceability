using Baubit.Traceability.Reasons;
using FluentResults;
using System.Collections.Generic;

namespace Baubit.Traceability.Successes
{
    public abstract class ASuccess : AReason, ISuccess
    {
        protected ASuccess(string message, Dictionary<string, object> metadata) : base(message, metadata)
        {
        }
        protected ASuccess() : this(string.Empty, new Dictionary<string, object>())
        {
            
        }
    }
}
