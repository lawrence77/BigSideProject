using Microsoft.VisualStudio.TestTools.UnitTesting;
using HammingCode.HammingTypes;
using HammingCode.HammingTypes.Exceptions;
using System;
using System.Text;


namespace TestLibrary.HammingCodeTests
{
    /// <summary>
    /// Tests the Constructors and Clone method for the HammingString.
    /// </summary>
    [TestClass]
    public class HamminStringTests
    {
        private Random rand;

        [TestInitialize]
        public void InitTests()
        {
            rand = new Random();
        }

        [TestMethod]
        public void ConstructorsTest()
        {
            String test = "Hello, World!";
            char[] test2 = { 'a', 'b', 'c' };

            HammingString hs = new HammingString();
            Assert.ThrowsException<EmptyHammingObjectException>(hs.RetrieveValue);

            hs = new HammingString(test);
            Assert.IsNotNull(hs.RetrieveValue());

            hs = new HammingString(test2);
            Assert.IsNotNull(hs.RetrieveValue());
        }

        [TestMethod]
        public void EqualsTest()
        {
            HammingString hs = new HammingString();
            HammingString other = new HammingString();

            Assert.IsFalse(hs.Equals(null)); // null test
            Assert.IsFalse(hs.Equals("stringObject")); // different object test
            Assert.IsTrue(hs.Equals(other)); // empty test

            String test = "Hello, World!";
            char[] test2 = { 'a', 'b', 'c' };

            // test for different strings
            hs.EncodeToHammingCode(test);
            other.EncodeToHammingCode(test2);
            Assert.IsFalse(hs.Equals(other));

            test = "abc";
            hs.EncodeToHammingCode(test);
            Assert.IsTrue(hs.Equals(other));
        }

        [TestMethod]
        public void CloneTest()
        {
            HammingString hs = new HammingString("test string");
            HammingString clone = hs.Clone();

            Assert.AreEqual(hs, clone); // test that they are the same

            clone.EncodeToHammingCode("other string");
            Assert.AreNotEqual(hs, clone); // test not a shallow copy
        }

        [TestMethod]
        public void EncodingTest()
        {
            String test = "MR. UTTERSON the lawyer was a man of a rugged countenance, that was never lighted by a smile; cold, scanty and embarrassed in discourse; backward in sentiment; lean, long, dusty, dreary, and yet somehow lovable.At friendly meetings, and when the wine was to his taste, something eminently human beaconed from his eye; something indeed which never found its way into his talk, but which spoke not only in these silent symbols of the after-dinner face, but more often and loudly in the acts of his life.He was austere with himself; drank gin when he was alone, to mortify a taste for vintages; and though he enjoyed the theatre, had not crossed the doors of one for twenty years. But he had an approved tolerance for others; sometimes wondering, almost with envy, at the high pressure of spirits involved in their misdeeds; and in any extremity inclined to help rather than to reprove. ‘I incline to, Cain’s heresy,’ he used to say. ‘I let my brother go to the devil in his quaintly: ‘own way.’ In this character, it was frequently his fortune to be the last reputable acquaintance and the last good influence in the lives of down-going men.And to such as these, so long as they came about his chambers, he never marked a shade of change in his demeanour.";
            HammingString hs = new HammingString(test);

            String retVal = hs.RetrieveValue() as String;
            Assert.AreEqual(test, retVal);
        }

        [TestMethod]
        public void SimulateNoErrorsTest()
        {
            String test = "MR. UTTERSON the lawyer was a man of a rugged countenance, that was never lighted by a smile; cold, scanty and embarrassed in discourse; backward in sentiment; lean, long, dusty, dreary, and yet somehow lovable.At friendly meetings, and when the wine was to his taste, something eminently human beaconed from his eye; something indeed which never found its way into his talk, but which spoke not only in these silent symbols of the after-dinner face, but more often and loudly in the acts of his life.He was austere with himself; drank gin when he was alone, to mortify a taste for vintages; and though he enjoyed the theatre, had not crossed the doors of one for twenty years. But he had an approved tolerance for others; sometimes wondering, almost with envy, at the high pressure of spirits involved in their misdeeds; and in any extremity inclined to help rather than to reprove. ‘I incline to, Cain’s heresy,’ he used to say. ‘I let my brother go to the devil in his quaintly: ‘own way.’ In this character, it was frequently his fortune to be the last reputable acquaintance and the last good influence in the lives of down-going men.And to such as these, so long as they came about his chambers, he never marked a shade of change in his demeanour.";
            for (int i = 1; i < test.Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(test, 0, i);

                HammingString hs = new HammingString(sb.ToString());

                // Simulate no error by doing nothing

                HammingReport hr = hs.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.NoError);
                Assert.IsTrue(hr.Syndrome == 0);

                // check return value
                String retVal = hs.RetrieveValue();
                Assert.IsNotNull(retVal);
                Assert.AreEqual(sb.ToString(), retVal);
            }
        }

        [TestMethod]
        public void SimulateMasterParityBitErrorTest()
        {
            String test = "MR. UTTERSON the lawyer was a man of a rugged countenance, that was never lighted by a smile; cold, scanty and embarrassed in discourse; backward in sentiment; lean, long, dusty, dreary, and yet somehow lovable.At friendly meetings, and when the wine was to his taste, something eminently human beaconed from his eye; something indeed which never found its way into his talk, but which spoke not only in these silent symbols of the after-dinner face, but more often and loudly in the acts of his life.He was austere with himself; drank gin when he was alone, to mortify a taste for vintages; and though he enjoyed the theatre, had not crossed the doors of one for twenty years. But he had an approved tolerance for others; sometimes wondering, almost with envy, at the high pressure of spirits involved in their misdeeds; and in any extremity inclined to help rather than to reprove. ‘I incline to, Cain’s heresy,’ he used to say. ‘I let my brother go to the devil in his quaintly: ‘own way.’ In this character, it was frequently his fortune to be the last reputable acquaintance and the last good influence in the lives of down-going men.And to such as these, so long as they came about his chambers, he never marked a shade of change in his demeanour.";
            for (int i = 1; i < test.Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(test, 0, i);

                HammingString hs = new HammingString(sb.ToString());

                // Simulate master bit error
                hs.SimulateMasterParityBitError();

                HammingReport hr = hs.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.MasterParityBitError);
                Assert.IsTrue(hr.Syndrome == 0);
                Assert.IsTrue(hr.Corrected);

                // check return value
                String retVal = hs.RetrieveValue();
                Assert.IsNotNull(retVal);
                Assert.AreEqual(sb.ToString(), retVal);
            }
        }

        [TestMethod]
        public void SimulateSingleParityBitErrorTest()
        {
            String test = "MR. UTTERSON the lawyer was a man of a rugged countenance, that was never lighted by a smile; cold, scanty and embarrassed in discourse; backward in sentiment; lean, long, dusty, dreary, and yet somehow lovable.At friendly meetings, and when the wine was to his taste, something eminently human beaconed from his eye; something indeed which never found its way into his talk, but which spoke not only in these silent symbols of the after-dinner face, but more often and loudly in the acts of his life.He was austere with himself; drank gin when he was alone, to mortify a taste for vintages; and though he enjoyed the theatre, had not crossed the doors of one for twenty years. But he had an approved tolerance for others; sometimes wondering, almost with envy, at the high pressure of spirits involved in their misdeeds; and in any extremity inclined to help rather than to reprove. ‘I incline to, Cain’s heresy,’ he used to say. ‘I let my brother go to the devil in his quaintly: ‘own way.’ In this character, it was frequently his fortune to be the last reputable acquaintance and the last good influence in the lives of down-going men.And to such as these, so long as they came about his chambers, he never marked a shade of change in his demeanour.";
            for (int i = 1; i < test.Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(test, 0, i);

                HammingString hs = new HammingString(sb.ToString());

                // Simulate single parity bit error
                hs.SimulateSingleParityBitError();

                // builds and checks report
                HammingReport hr = hs.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.ParityBitError);
                Assert.IsTrue(hr.Syndrome > 0);
                Assert.IsTrue(hr.Corrected);

                // check return value
                String retVal = hs.RetrieveValue();
                Assert.IsNotNull(retVal);
                Assert.AreEqual(sb.ToString(), retVal);
            }
        }

        [TestMethod]
        public void SimulateSingleDataBitErrorTest()
        {
            String test = "MR. UTTERSON the lawyer was a man of a rugged countenance, that was never lighted by a smile; cold, scanty and embarrassed in discourse; backward in sentiment; lean, long, dusty, dreary, and yet somehow lovable.At friendly meetings, and when the wine was to his taste, something eminently human beaconed from his eye; something indeed which never found its way into his talk, but which spoke not only in these silent symbols of the after-dinner face, but more often and loudly in the acts of his life.He was austere with himself; drank gin when he was alone, to mortify a taste for vintages; and though he enjoyed the theatre, had not crossed the doors of one for twenty years. But he had an approved tolerance for others; sometimes wondering, almost with envy, at the high pressure of spirits involved in their misdeeds; and in any extremity inclined to help rather than to reprove. ‘I incline to, Cain’s heresy,’ he used to say. ‘I let my brother go to the devil in his quaintly: ‘own way.’ In this character, it was frequently his fortune to be the last reputable acquaintance and the last good influence in the lives of down-going men.And to such as these, so long as they came about his chambers, he never marked a shade of change in his demeanour.";
            for (int i = 1; i < test.Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(test, 0, i);

                HammingString hs = new HammingString(sb.ToString());

                // Simulate single data bit error
                hs.SimulateSingleDataBitError();

                // builds and checks report
                HammingReport hr = hs.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.DataBitError);
                Assert.IsTrue(hr.Syndrome > 0);
                Assert.IsTrue(hr.Corrected);

                // check return value
                String retVal = hs.RetrieveValue();
                Assert.IsNotNull(retVal);
                Assert.AreEqual(sb.ToString(), retVal);
            }
        }

        [TestMethod]
        public void SimulateDoubleBitErrorTest()
        {
            String test = "MR. UTTERSON the lawyer was a man of a rugged countenance, that was never lighted by a smile; cold, scanty and embarrassed in discourse; backward in sentiment; lean, long, dusty, dreary, and yet somehow lovable.At friendly meetings, and when the wine was to his taste, something eminently human beaconed from his eye; something indeed which never found its way into his talk, but which spoke not only in these silent symbols of the after-dinner face, but more often and loudly in the acts of his life.He was austere with himself; drank gin when he was alone, to mortify a taste for vintages; and though he enjoyed the theatre, had not crossed the doors of one for twenty years. But he had an approved tolerance for others; sometimes wondering, almost with envy, at the high pressure of spirits involved in their misdeeds; and in any extremity inclined to help rather than to reprove. ‘I incline to, Cain’s heresy,’ he used to say. ‘I let my brother go to the devil in his quaintly: ‘own way.’ In this character, it was frequently his fortune to be the last reputable acquaintance and the last good influence in the lives of down-going men.And to such as these, so long as they came about his chambers, he never marked a shade of change in his demeanour.";
            for (int i = 1; i < test.Length; i++)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(test, 0, i);

                HammingString hs = new HammingString(sb.ToString());

                // Simulate double bit error
                hs.SimulateDoubleBitError();

                // builds and checks report
                HammingReport hr = hs.BuildReport();
                Assert.IsTrue(hr.Status == ErrorTypesEnum.MultiBitError);
                Assert.IsTrue(hr.Syndrome > 0);
                Assert.IsFalse(hr.Corrected);

                // Cannot check return value since its uncorrectable
            }
        }
    }
}
