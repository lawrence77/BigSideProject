using Microsoft.VisualStudio.TestTools.UnitTesting;
using HammingCode.HammingTypes;
using HammingCode.HammingTypes.Exceptions;

namespace TestLibrary
{
    /// <summary>
    /// Tests the Constructors and Clone method for the HammingObject.
    /// </summary>
    [TestClass]
    public class HammingObjectTests
    {
        private byte[] test;

        [TestInitialize]
        public void InitTests()
        {
            test = new byte[4] { 0x90, 0x01, 0x0a, 0x20 };
        }

        [TestMethod]
        public void SimpleConstructorTest()
        {
            HammingObject hc = new HammingObject();
            Assert.IsFalse(hc.IsErroneous);
            Assert.ThrowsException<EmptyHammingObjectException>(hc.RetrieveValue); 
        }

        [TestMethod]
        public void ByteConstructorTest()
        {
            HammingObject hc = new HammingObject(test);
            Assert.IsFalse(hc.IsErroneous);
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

            Assert.AreEqual(hc.IsErroneous, hammingClone.IsErroneous);
            Assert.AreEqual(hc, hammingClone);
        }

        [TestMethod]
        public void EncodingToHammingTest()
        {
            throw new System.NotImplementedException();
        }


        [TestMethod]
        public void SimulateNoErrorTest()
        {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SimulateMasterBitErrorTest()
        {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SimulateSingleParityBitErrorTest()
        {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SimulateSingleDataBitErrorTest()
        {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SimulateDoubleBitErrorTest()
        {
            throw new System.NotImplementedException();
        }

        [TestMethod]
        public void SimlulateRandomErrorTest()
        {
            throw new System.NotImplementedException();
        }
    }
}
