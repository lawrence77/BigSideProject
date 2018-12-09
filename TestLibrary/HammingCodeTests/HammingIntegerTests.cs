﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using HammingCode.HammingTypes;
using HammingCode.HammingTypes.Exceptions;
using System;


namespace TestLibrary.HammingCodeTests
{
    /// <summary>
    /// Tests the Constructors and Clone method for the HammingInt16.
    /// </summary>
    [TestClass]
    public class HammingIntegerTests
    {
        private const int MAX_BYTE_VALUE = 256;
        private Int16 test;
        private Random rand;

        [TestInitialize]
        public void InitTests()
        {
            test = 10900;
            rand = new Random();
        }

        [TestMethod]
        public void ConstructorsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void EqualsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void EncodingTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SimulateNoErrorsTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SimulateMasterParityBitErrorTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SimulateSingleParityBitErrorTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SimulateSingleDataBitErrorTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void SimulateDoubleBitErrorTest()
        {
            throw new NotImplementedException();
        }
    }
}