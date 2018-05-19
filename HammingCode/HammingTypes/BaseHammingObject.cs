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
        public virtual void ConvertByteArrayToHammingCode(byte[] targetValue)
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
            int numberOfDataBits = targetValue.Length * 8, numberOfParityBits = 0, totalBits = 0, byteSize = 0;
            services.CalculateBitSizes(ref byteSize, ref totalBits, ref numberOfParityBits, numberOfDataBits);

            //update properties
            DataBitsSize = numberOfDataBits;
            HammingBytes = services.BuildHamming(targetValue, byteSize, numberOfParityBits);
        }

        public virtual Object RetrieveValue()
        {
            return services.HammingCodeToDataBytesArray(DataBitsSize, HammingBytes);
        }

        public virtual HammingReport BuildReport()
        {
            throw new NotImplementedException();
        }

        public virtual void SimulateRandomError()
        {
            Random random = new Random();
            int numberOfErrors = random.Next(3);        //1 or 2 errors

            int byteSize = 0, totalBits = 0, numberOfParityBits = 0;
            services.CalculateBitSizes(ref byteSize, ref totalBits, ref numberOfParityBits, DataBitsSize);

            int flipBitAt = random.Next(totalBits);     //first error
            FlipBitAt(flipBitAt);            

            if (numberOfErrors > 1)                     //one-third chance of second error. (And yes, there's a chance that it corrects the first flipped bit)
            {
                flipBitAt = random.Next(totalBits);
                FlipBitAt(flipBitAt);
            }
        }

        #endregion

        #region Private Methods
        private void FlipBitAt(int index)
        {
            if (services.GetBit(HammingBytes[index / 8], index % 8))    //Bit is 1.
            {
                HammingBytes[index / 8] = services.SetBit(HammingBytes[index / 8], index % 8, 0); //Set to 0.
            }
            else                                                        //Bit is 0.
            {
                HammingBytes[index /8] = services.SetBit(HammingBytes[index / 8], index % 8, 1); //Set to 1.
            }
        }

        #endregion
    }
}
