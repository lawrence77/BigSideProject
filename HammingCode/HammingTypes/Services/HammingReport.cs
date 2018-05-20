using System;
using System.Collections.Generic;
using System.Text;
using HammingCode.HammingTypes.Exceptions;

namespace HammingCode.HammingTypes.Services
{
    public struct HammingReport
    {
        public ErrorTypesEnum Status;
        public int Syndrome;

        public HammingReport(ErrorTypesEnum typeOfError, int errorLocation)
        {
            Status = typeOfError;
            Syndrome = errorLocation;
        }
    }
}