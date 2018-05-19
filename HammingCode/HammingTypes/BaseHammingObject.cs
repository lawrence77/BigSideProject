using System;
using System.Collections.Generic;
using System.Text;
using HammingCode.Services;
using HammingCode.HammingExceptions;

namespace HammingCode.HammingTypes
{
    public class BaseHammingObject : IHammingObject
    {
        #region Properties
        protected byte[] HammingBytes { get; set; }
        private Helper services { get; set; }
        private int DataBitsSize { get; set; }
        #endregion

        #region Constructor
        public BaseHammingObject()
        {
            HammingBytes = null;
            services = new Helper();
        }
        #endregion

        #region Interface Methods
        public void ConvertByteArrayToHammingCode(byte[] targetValue)
        {
            //Input errors
            if (targetValue == null)
            {
                throw new HammingObjectInvalidInputException("Input array is Null.");
            }
            if (targetValue.Length < 1)
            {
                throw new HammingObjectInvalidInputException("There are Fewer than 1 Bytes in the input array.");
            }

            //size calculations
            int numberOfDataBits = targetValue.Length * 8;
            int numberOfParityBits = (int) Math.Floor(Math.Log(numberOfDataBits, 2)) + 2;     //Plus two for Master Parity Bit and Parity Bit 1.
            int totalBits = numberOfDataBits + numberOfParityBits;
            int byteSize = totalBits % 8 == 0 ? totalBits / 8 : (totalBits / 8) + 1;

            //update properties
            DataBitsSize = numberOfDataBits;
            HammingBytes = services.BuildHamming(targetValue, byteSize, numberOfParityBits);
        }

        public Object RetrieveValue()
        {
            return services.HammingCodeToDataBytesArray(DataBitsSize, HammingBytes);
        }

        public HammingReport BuildReport()
        {
            throw new NotImplementedException();
        }

        public void SimulateRandomError()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods
        

        #endregion
    }
}
