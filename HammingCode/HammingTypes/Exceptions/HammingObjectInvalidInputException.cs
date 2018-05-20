using System;
using System.Collections.Generic;
using System.Text;

namespace HammingCode.HammingTypes.Exceptions
{
    public class HammingObjectInvalidInputException : Exception
    {
        public HammingObjectInvalidInputException() { }

        public HammingObjectInvalidInputException(string message) : base(message) { }

        public HammingObjectInvalidInputException(string message, Exception inner) : base(message, inner) { }
    }
}
