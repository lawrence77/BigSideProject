using System;
using System.Collections.Generic;
using System.Text;
using HammingCode.HammingTypes;
using HammingCode.HammingTypes.Exceptions;

namespace HammingCode.Components
{
    class IntegerHammingObject : IHammingObject
    {
        #region Members
        private ValidTypesEnum intType { get; set; }

        #endregion

        #region Interface Methods

        public void EncodeToHammingCode(byte[] targetValue)
        {
            throw new NotImplementedException();
        }

        public void SimulateRandomError()
        {
            throw new NotImplementedException();
        }

        public object RetrieveValue()
        {
            throw new NotImplementedException();
        }

        public HammingReport BuildReport()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Expanded Functionality

        public void ConvertInt16(Int16 targetValue)
        {
            throw new NotImplementedException();
        }

        public void ConvertInt32(Int32 targetValue)
        {
            throw new NotImplementedException();
        }

        public void ConvertInt64(Int64 targetValue)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Functions
        private void SimulateError(ErrorTypesEnum error)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Enums

        private enum ValidTypesEnum
        {
            Int16,
            Int32,
            Int64,
            ByteArray
        }
        #endregion
    }

}
