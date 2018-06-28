using Microsoft.VisualStudio.TestTools.UnitTesting;
using HammingCode.HammingTypes;
using HammingCode.HammingTypes.Exceptions;
using System;

namespace TestLibrary
{
    /// <summary>
    /// Tests the Constructors and Clone method for the HammingObject.
    /// </summary>
    [TestClass]
    public class HammingObjectTests
    {
        private const int MAX_BYTE_VALUE = 256;
        private byte[] test;
        private Random rand;

        [TestInitialize]
        public void InitTests()
        {
            test = new byte[4] { 0x90, 0x01, 0x0a, 0x20 };
            rand = new Random();
        }

        [TestMethod]
        public void SimpleConstructorTest()
        {
            HammingObject hc = new HammingObject();
            Assert.ThrowsException<EmptyHammingObjectException>(hc.RetrieveValue); 
        }

        [TestMethod]
        public void ByteConstructorTest()
        {
            HammingObject hc = new HammingObject(test);
            Assert.IsNotNull(hc.RetrieveValue());
        }

        [TestMethod]
        public void EqualsTest()
        {
            HammingObject hc1 = new HammingObject(test);
            
            Assert.IsFalse(hc1.Equals(null)); 

            string testString = "test";
            Assert.IsFalse(hc1.Equals(testString));

            byte[] anotherByteArray = { 0x00, 0x01, 0x02 }; //differing byte array lengths
            HammingObject hc2 = new HammingObject(anotherByteArray);
            Assert.IsFalse(hc1.Equals(hc2));

            byte[] secondByteArray = { 0x00, 0x01, 0x02, 0x03 }; //same byte array lengths
            HammingObject hc3 = new HammingObject(secondByteArray);
            Assert.IsFalse(hc1.Equals(hc3));

            HammingObject hc4 = new HammingObject(test);
            Assert.IsTrue(hc1.Equals(hc4));
        }

        [TestMethod]
        public void CloneTest()
        {
            HammingObject hc = new HammingObject(test);
            HammingObject hammingClone = hc.Clone();

            Assert.AreEqual(hc, hammingClone);
        }

        [TestMethod]
        public void SimulateNoErrorTest()
        {
            for (int byteSize = 1; byteSize < 16; byteSize++)
            {
                byte[] byteArray = createByteArray(byteSize);

                HammingObject hc = HammingObject.ParseByteArray(byteArray); // encodes byte array

                // Simulate no error by doing nothing

                // builds and checks report
                HammingReport hr = hc.BuildReport(); 
                Assert.IsTrue(hr.Status == ErrorTypesEnum.NoError); 
                Assert.IsTrue(hr.Syndrome == 0);

                // checks return value
                byte[] retrievedValue = hc.RetrieveValue() as byte[];
                Assert.IsNotNull(retrievedValue);
                Assert.IsTrue(retrievedValue.Length == byteArray.Length);
                for (int i = 0; i < byteArray.Length; i++)
                {
                    Assert.IsTrue(byteArray[i] == retrievedValue[i]);
                }
            }
        }

        [TestMethod]
        public void SimulateMasterBitErrorTest()
        {
            for (int byteSize = 1; byteSize < 16; byteSize++)
            {
                byte[] byteArray = createByteArray(byteSize);

                HammingObject hc = HammingObject.ParseByteArray(byteArray); // encodes byte array

                // Simulate Master Bit Error
                hc.SimulateMasterParityBitError();

                // builds and checks report
                HammingReport hr = hc.BuildReport(); 
                Assert.IsTrue(hr.Status == ErrorTypesEnum.MasterParityBitError); 
                Assert.IsTrue(hr.Syndrome == 0);
                Assert.IsTrue(hr.Corrected); // Ensures HammingObject is corrected

                // checks return value
                byte[] retrievedValue = hc.RetrieveValue() as byte[];
                Assert.IsNotNull(retrievedValue);
                Assert.IsTrue(retrievedValue.Length == byteArray.Length);
                for (int i = 0; i < byteArray.Length; i++)
                {
                    Assert.IsTrue(byteArray[i] == retrievedValue[i]);
                }
            }
        }

        [TestMethod]
        public void SimulateSingleParityBitErrorTest()
        {
            for (int byteSize = 1; byteSize < 16; byteSize++)
            {
                byte[] byteArray = createByteArray(byteSize);

                HammingObject hc = HammingObject.ParseByteArray(byteArray); // encodes byte array

                // Simulate Master Bit Error
                hc.SimulateSingleParityBitError();

                // builds and checks report
                HammingReport hr = hc.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.ParityBitError);
                Assert.IsTrue(hr.Syndrome > 0);
                Assert.IsTrue(hr.Corrected); // Ensures HammingObject is corrected

                // checks return value
                byte[] retrievedValue = hc.RetrieveValue() as byte[];
                Assert.IsNotNull(retrievedValue);
                Assert.IsTrue(retrievedValue.Length == byteArray.Length);
                for (int i = 0; i < byteArray.Length; i++)
                {
                    Assert.IsTrue(byteArray[i] == retrievedValue[i]);
                }
            }
        }

        [TestMethod]
        public void SimulateSingleDataBitErrorTest()
        {
            for (int byteSize = 1; byteSize < 16; byteSize++)
            {
                byte[] byteArray = createByteArray(byteSize);

                HammingObject hc = HammingObject.ParseByteArray(byteArray); // encodes byte array

                // Simulate Master Bit Error
                hc.SimulateSingleDataBitError();

                // builds and checks report
                HammingReport hr = hc.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.DataBitError);
                Assert.IsTrue(hr.Syndrome > 0);
                Assert.IsTrue(hr.Corrected); // Ensures HammingObject is corrected

                // checks return value
                byte[] retrievedValue = hc.RetrieveValue() as byte[];
                Assert.IsNotNull(retrievedValue);
                Assert.IsTrue(retrievedValue.Length == byteArray.Length);
                for (int i = 0; i < byteArray.Length; i++)
                {
                    Assert.IsTrue(byteArray[i] == retrievedValue[i]);
                }
            }
        }

        [TestMethod]
        public void SimulateDoubleBitErrorTest()
        {
            for (int byteSize = 1; byteSize < 16; byteSize++)
            {
                byte[] byteArray = createByteArray(byteSize);

                HammingObject hc = HammingObject.ParseByteArray(byteArray); // encodes byte array

                // Simulate Master Bit Error
                hc.SimulateDoubleBitError();

                // builds and checks report
                HammingReport hr = hc.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.MultiBitError);
                Assert.IsTrue(hr.Syndrome > 0);
                Assert.IsFalse(hr.Corrected); // Ensures HammingObject could not be corrected

                // checks return value
                byte[] retrievedValue = hc.RetrieveValue() as byte[];
                Assert.IsNotNull(retrievedValue);
                Assert.IsTrue(retrievedValue.Length == byteArray.Length); 
            }
        }

        [TestMethod]
        public void SimlulateRandomErrorTest()
        {
            for (int i = 0; i < 10000; i++)
            {
                int selector = rand.Next(5);
                switch (selector)
                {
                    case 0:
                        SimulateNoErrorTest();
                        break;
                    case 1:
                        SimulateMasterBitErrorTest();
                        break;
                    case 2:
                        SimulateSingleDataBitErrorTest();
                        break;
                    case 3:
                        SimulateSingleParityBitErrorTest();
                        break;
                    case 4:
                        SimulateDoubleBitErrorTest();
                        break;
                }

            }
        }

        private byte[] createByteArray(int numberOfBytes)
        {          
            //create new byte array with random values
            byte[] byteArray = new byte[numberOfBytes];
            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = (byte)rand.Next(MAX_BYTE_VALUE);
            }
            return byteArray;
        }
    }
}
