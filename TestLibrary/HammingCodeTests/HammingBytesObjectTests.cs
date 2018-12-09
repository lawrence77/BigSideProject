using Microsoft.VisualStudio.TestTools.UnitTesting;
using HammingCode.HammingTypes;
using HammingCode.HammingTypes.Exceptions;
using System;

namespace TestLibrary.HammingCodeTests
{
    /// <summary>
    /// Tests the Constructors and Clone method for the HammingObject.
    /// </summary>
    [TestClass]
    public class HammingBytesObjectTests
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
            HammingBase hc = new HammingBase();
            Assert.ThrowsException<EmptyHammingObjectException>(hc.RetrieveValue); 
        }

        [TestMethod]
        public void ByteConstructorTest()
        {
            HammingBase hc = new HammingBase(test);
            Assert.IsNotNull(hc.RetrieveValue());
        }

        [TestMethod]
        public void EqualsTest()
        {
            HammingBase hc1 = new HammingBase(test);
            
            Assert.IsFalse(hc1.Equals(null)); 

            string testString = "test";
            Assert.IsFalse(hc1.Equals(testString));

            byte[] anotherByteArray = { 0x00, 0x01, 0x02 }; //differing byte array lengths
            HammingBase hc2 = new HammingBase(anotherByteArray);
            Assert.IsFalse(hc1.Equals(hc2));

            byte[] secondByteArray = { 0x00, 0x01, 0x02, 0x03 }; //same byte array lengths
            HammingBase hc3 = new HammingBase(secondByteArray);
            Assert.IsFalse(hc1.Equals(hc3));

            HammingBase hc4 = new HammingBase(test);
            Assert.IsTrue(hc1.Equals(hc4));

            HammingInteger hcInt32 = new HammingInteger(537526672);
            Assert.IsFalse(hc1.Equals(hcInt32));
        }

        [TestMethod]
        public void CloneTest()
        {
            HammingBase hc = new HammingBase(test);
            HammingBase hammingClone = hc.Clone();

            Assert.AreEqual(hc, hammingClone);
        }

        [TestMethod]
        public void TestGetHammingBytes()
        {
            HammingBase hc = HammingBase.EncodeByteArray(test);

            byte[] testArray = hc.GetHammingBytes();
            Assert.IsNotNull(testArray);

            byte[] cloneArray = new byte[testArray.Length];
            for (int i = 0; i < testArray.Length; i++)
            {
                cloneArray[i] = testArray[i];
            }

            hc = HammingBase.EncodeByteArray(new byte[2]{ 0x1, 0x0});
            byte[] testArray2 = hc.GetHammingBytes();

            // Ensures that the testArray is just a copy of the Hamming Bytes. Not a reference
            Assert.IsTrue(cloneArray.Length == testArray.Length);
            for (int i = 0; i < cloneArray.Length; i++)
            {
                Assert.IsTrue(cloneArray[i] == testArray[i]);
            }

            Assert.IsFalse(testArray.Length == testArray2.Length);
        }

        [TestMethod]
        public void TestBytesArrayEncoding()
        {
            HammingBase hc = HammingBase.EncodeByteArray(test);

            byte[] testArray = hc.RetrieveValue() as byte[];

            Assert.IsNotNull(testArray);
            Assert.AreEqual(testArray.Length, test.Length);
            for (int i = 0; i < testArray.Length; i++)
            {
                Assert.IsTrue(test[i] == testArray[i]);
            }
        }

        [TestMethod]
        public void SimulateNoErrorTest()
        {
            for (int byteSize = 1; byteSize < 16; byteSize++)
            {
                byte[] byteArray = createByteArray(byteSize);

                HammingBase hc = HammingBase.EncodeByteArray(byteArray); // encodes byte array

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

                HammingBase hc = HammingBase.EncodeByteArray(byteArray); // encodes byte array

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

                HammingBase hc = HammingBase.EncodeByteArray(byteArray); // encodes byte array

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

                HammingBase hc = HammingBase.EncodeByteArray (byteArray); // encodes byte array

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

                HammingBase hc = HammingBase.EncodeByteArray(byteArray); // encodes byte array

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
