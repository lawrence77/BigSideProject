using System;
using System.Collections.Generic;
using System.Text;

namespace HammingCode.HammingTypes.Exceptions
{
    public class EmptyHammingObjectException : Exception
    {
        public EmptyHammingObjectException() { }

        public EmptyHammingObjectException(string message) : base(message) { }

        public EmptyHammingObjectException(string message, Exception inner) : base(message, inner) { }
    }
}
