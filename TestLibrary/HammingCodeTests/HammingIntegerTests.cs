using Microsoft.VisualStudio.TestTools.UnitTesting;
using HammingCode.HammingTypes;
using HammingCode.HammingTypes.Exceptions;
using System;


namespace TestLibrary.HammingCodeTests
{
    /// <summary>
    /// Tests the Constructors and Clone method for the HammingInteger.
    /// </summary>
    [TestClass]
    public class HammingIntegerTests
    {
        private const int MAX_BYTE_VALUE = 256;
        private Random rand;

        [TestInitialize]
        public void InitTests()
        {
            rand = new Random();
        }

        [TestMethod]
        public void ConstructorsTest()
        {
            HammingInteger hi = new HammingInteger();
            Assert.AreEqual("None", hi.GetIntegerType());
            Assert.ThrowsException<EmptyHammingObjectException>(hi.RetrieveValue);

            byte testByte = 1;
            hi = new HammingInteger(testByte);
            Assert.AreEqual("Int8", hi.GetIntegerType());

            Int16 testInt16 = 1;
            hi = new HammingInteger(testInt16);
            Assert.AreEqual("Int16", hi.GetIntegerType());

            int testInt32 = 1;
            hi = new HammingInteger(testInt32);
            Assert.AreEqual("Int32", hi.GetIntegerType());
            
            Int64 testInt64 = 1;
            hi = new HammingInteger(testInt64);
            Assert.AreEqual("Int64", hi.GetIntegerType());

            byte[] testArray = { 1, 1 };
            hi = new HammingInteger(testArray);
            Assert.AreEqual("ArrayOfBytes", hi.GetIntegerType());
        }

        [TestMethod]
        public void EqualsTest()
        {
            HammingInteger curr = new HammingInteger();
            HammingInteger other = new HammingInteger();

            Assert.IsFalse(curr.Equals(null)); // null test
            Assert.IsFalse(curr.Equals("stringObject")); // different object test
            Assert.IsTrue(curr.Equals(other)); // empty test

            // test different integer tests
            byte test = 10;
            Int16 diffInt = 10;
            curr.EncodeToHammingCode(test);
            other.EncodeToHammingCode(diffInt);
            Assert.IsFalse(curr.Equals(other));

            // test same integer type different integer value
            test = 10;
            byte test2 = 9;
            curr.EncodeToHammingCode(test);
            other.EncodeToHammingCode(test2);
            Assert.IsFalse(curr.Equals(other));

            // test Int8
            test = 10; test2 = 10;
            curr.EncodeToHammingCode(test);
            other.EncodeToHammingCode(test2);
            Assert.IsTrue(curr.Equals(other));

            // test Int16
            Int16 testInt16 = 100; Int16 test2Int16 = 100;
            curr.EncodeToHammingCode(testInt16);
            other.EncodeToHammingCode(test2Int16);
            Assert.IsTrue(curr.Equals(other));

            // test Int32
            int testInt32 = 1000; int test2Int32 = 1000;
            curr.EncodeToHammingCode(testInt32);
            other.EncodeToHammingCode(test2Int32);
            Assert.IsTrue(curr.Equals(other));

            // test Int64
            Int64 testInt64 = 10000; Int64 test2Int64 = 10000;
            curr.EncodeToHammingCode(testInt64);
            other.EncodeToHammingCode(test2Int64);
            Assert.IsTrue(curr.Equals(other));

            // test bytes array
            byte[] testArray = { 1, 10 }; byte[] testArray2 = { 1, 10 };
            curr.EncodeToHammingCode(testArray);
            other.EncodeToHammingCode(testArray2);
            Assert.IsTrue(curr.Equals(other)); 
        }

        [TestMethod]
        public void CloneTest()
        {
            // None test
            HammingInteger hi = new HammingInteger();
            HammingInteger clone = hi.Clone();
            Assert.AreEqual(hi, clone);

            // Int8
            byte testByte = 100;
            hi = new HammingInteger(testByte);
            clone = hi.Clone();
            Assert.AreEqual(hi, clone);

            // Int16
            Int16 testInt16 = 100;
            hi = new HammingInteger(testInt16);
            clone = hi.Clone();
            Assert.AreEqual(hi, clone);

            // Int32
            Int32 testInt32 = 100;
            hi = new HammingInteger(testInt32);
            clone = hi.Clone();
            Assert.AreEqual(hi, clone);

            // Int64
            Int64 testInt64 = 100;
            hi = new HammingInteger(testInt64);
            clone = hi.Clone();
            Assert.AreEqual(hi, clone);

            // Byte Array
            byte[] byteArray = { 1, 10, 100 };
            hi = new HammingInteger(byteArray);
            clone = hi.Clone();
            Assert.AreEqual(hi, clone);

            // Test that clone isn't a swallow copy
            Int16 lastTest = 10;
            clone.EncodeToHammingCode(lastTest);
            Assert.AreNotEqual(hi, clone);
        }

        [TestMethod]
        public void EncodingTest()
        {
            // Int8 encoding test
            byte testByte = 10;
            HammingInteger hi = new HammingInteger(testByte);
            byte? returnByte = hi.RetrieveValue() as byte?;
            Assert.IsNotNull(returnByte);
            Assert.AreEqual(testByte, returnByte);

            // Int16 encoding test
            Int16 testInt16 = 100;
            hi = new HammingInteger(testInt16);
            Int16? returnInt16 = hi.RetrieveValue() as Int16?;
            Assert.IsNotNull(returnInt16);
            Assert.AreEqual(testInt16, returnInt16);

            // Int32 encoding test
            Int32 testInt32 = 1000;
            hi = new HammingInteger(testInt32);
            Int32? returnInt32 = hi.RetrieveValue() as Int32?;
            Assert.IsNotNull(returnInt32);
            Assert.AreEqual(testInt32, returnInt32);

            // Int64 encoding test
            Int64 testInt64 = 10000;
            hi = new HammingInteger(testInt64);
            Int64? returnInt64 = hi.RetrieveValue() as Int64?;
            Assert.IsNotNull(returnInt64);
            Assert.AreEqual(testInt64, returnInt64);

            // Bytes array encoding test
            byte[] testByteArray = { 1, 10, 100 };
            hi = new HammingInteger(testByteArray);
            byte[] returnByteArray = hi.RetrieveValue() as byte[];
            Assert.IsNotNull(returnByteArray);
            for (int i = 0; i < returnByteArray.Length; i++)
            {
                Assert.AreEqual(testByteArray[i], returnByteArray[i]);
            }
        }

        [TestMethod]
        public void SimulateNoErrorsTest()
        {
            for (int i = 0; i < 100; i++)
            {
                int test = rand.Next(Int32.MaxValue);
                HammingInteger hi = HammingInteger.EncodeInt(test);

                // Simulate no error by doing nothing

                // builds and checks report
                HammingReport hr = hi.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.NoError);
                Assert.IsTrue(hr.Syndrome == 0);

                // check return value
                int? retVal = hi.RetrieveValue() as int?;
                Assert.IsNotNull(retVal);
                Assert.AreEqual(test, retVal);
            }
        }

        [TestMethod]
        public void SimulateMasterParityBitErrorTest()
        {
            for (int i = 0; i < 100; i++)
            {
                int test = rand.Next(Int32.MaxValue);
                HammingInteger hi = HammingInteger.EncodeInt(test);

                // Simulate master bit error
                hi.SimulateMasterParityBitError();

                // builds and checks report
                HammingReport hr = hi.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.MasterParityBitError);
                Assert.IsTrue(hr.Syndrome == 0);
                Assert.IsTrue(hr.Corrected);

                // check return value
                int? retVal = hi.RetrieveValue() as int?;
                Assert.IsNotNull(retVal);
                Assert.AreEqual(test, retVal);
            }
        }

        [TestMethod]
        public void SimulateSingleParityBitErrorTest()
        {
            for (int i = 0; i < 100; i++)
            {
                int test = rand.Next(Int32.MaxValue);
                HammingInteger hi = HammingInteger.EncodeInt(test);

                // Simulate single parity bit error
                hi.SimulateSingleParityBitError();

                // builds and checks report
                HammingReport hr = hi.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.ParityBitError);
                Assert.IsTrue(hr.Syndrome > 0);
                Assert.IsTrue(hr.Corrected);

                // check return value
                int? retVal = hi.RetrieveValue() as int?;
                Assert.IsNotNull(retVal);
                Assert.AreEqual(test, retVal);
            }
        }

        [TestMethod]
        public void SimulateSingleDataBitErrorTest()
        {
            for (int i = 0; i < 100; i++)
            {
                int test = rand.Next(Int32.MaxValue);
                HammingInteger hi = HammingInteger.EncodeInt(test);

                // Simulate single data bit error
                hi.SimulateSingleDataBitError();

                // builds and checks report
                HammingReport hr = hi.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.DataBitError);
                Assert.IsTrue(hr.Syndrome > 0);
                Assert.IsTrue(hr.Corrected);

                // check return value
                int? retVal = hi.RetrieveValue() as int?;
                Assert.IsNotNull(retVal);
                Assert.AreEqual(test, retVal);
            }
        }

        [TestMethod]
        public void SimulateDoubleBitErrorTest()
        {
            for (int i = 0; i < 100; i++)
            {
                int test = rand.Next(Int32.MaxValue);
                HammingInteger hi = HammingInteger.EncodeInt(test);

                // Simulate double bit error
                hi.SimulateDoubleBitError();

                // builds and checks report
                HammingReport hr = hi.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.MultiBitError);
                Assert.IsTrue(hr.Syndrome > 0);
                Assert.IsFalse(hr.Corrected);
                
                // Cannot check return value since its uncorrectable
            }
        }
    }
}
