﻿using System;
using System.Collections.Generic;
using System.Text;
using HammingCode.HammingTypes;
using HammingCode.HammingTypes.Exceptions;

namespace HammingCode.Components
{
    class StringHammingObject : IHammingObject
    {
        #region Properties

        #endregion

        #region Interface Functions

        public void EncodeToHammingCode(byte[] targetValue)
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


        public void SimulateRandomError()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Expanded Functionality

        #endregion

        #region Private Methods        

        #endregion

    }
}
