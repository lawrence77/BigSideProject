using System;
using System.Collections.Generic;
using System.Text;
using HammingCode.HammingTypes.Exceptions;

namespace HammingCode.HammingTypes
{
    public struct HammingReport
    {
        public ErrorTypesEnum Status;
        public int Syndrome;
        public bool Corrected;

        public HammingReport(int errorLocation, ErrorTypesEnum typeOfError, bool errorFixed)
        {
            Status = typeOfError;
            Syndrome = errorLocation;
            Corrected = errorFixed;
        }
    }
}