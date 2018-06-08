using Microsoft.VisualStudio.TestTools.UnitTesting;
using HammingCode.HammingTypes;
using HammingCode.HammingTypes.Exceptions;

namespace TestLibrary
{
    [TestClass]
    public class ConstructorTests
    {
        private byte[] test;

        [TestInitialize]
        public void InitTests()
        {
            test = new byte[4] { 0x90, 0x01, 0x0a, 0x20 };
        }

        [TestMethod]
        public void HammingObjectSimpleConstructorTest()
        {
            HammingObject hc = new HammingObject();
            Assert.IsFalse(hc.IsErroneous);
            Assert.ThrowsException<EmptyHammingObjectException>(hc.RetrieveValue); 
        }

        [TestMethod]
        public void HammingObjectByteConstructorTest()
        {
            HammingObject hc = new HammingObject(test);
            Assert.IsFalse(hc.IsErroneous);
            Assert.IsNotNull(hc.RetrieveValue());
        }

        [TestMethod]
        public void HammingObjectCloneTest()
        {
            HammingObject hc = new HammingObject(test);
            HammingObject hammingClone = hc.Clone();

            Assert.AreEqual(hc.IsErroneous, hammingClone.IsErroneous);
            Assert.AreEqual(hc, hammingClone);
        }

        
    }
}
