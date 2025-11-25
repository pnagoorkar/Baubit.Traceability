using FluentResults;
using System;

namespace Baubit.Traceability.Exceptions
{
    public class FailedOperationException: Exception 
    {
        public IResultBase Result { get; private set; }

        public FailedOperationException(IResultBase result) : base(result.ToString())
        {
            this.Result = result;
        }
    }
}
