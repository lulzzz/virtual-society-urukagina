﻿using System;
using System.Runtime.Serialization;

namespace Vs.Rules.Core.Exceptions
{
    [Serializable]
    public class UnresolvedException : Exception
    {
        public UnresolvedException()
        {
            throw new NotImplementedException();
        }

        public UnresolvedException(string message) : base(message)
        {
        }

        public UnresolvedException(string message, Exception innerException) : base(message, innerException)
        {

        }

        protected UnresolvedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            throw new NotImplementedException();
        }
    }
}