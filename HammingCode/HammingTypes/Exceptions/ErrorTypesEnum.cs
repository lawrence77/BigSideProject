using System;
using System.Collections.Generic;
using System.Text;

namespace HammingCode.HammingTypes.Exceptions
{
    public enum ErrorTypesEnum
    {
        NoError,
        MasterParityBitError,
        DataBitError,
        ParityBitError,
        MultiBitError        
    }
}
