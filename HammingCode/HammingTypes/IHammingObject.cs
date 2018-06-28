using System;
using System.Collections.Generic;
using System.Text;

namespace HammingCode.HammingTypes
{
    /// <summary>
    /// Hamming-Code object that will be able to encode the byte(s) and provide Single-Bit Correct and Double-bit detection errors.
    /// </summary>
    public interface IHammingObject
    {
        /// <summary>
        /// Given an array of bytes, creates a Hamming-Sequence of bits that protect the data.
        /// </summary>
        /// <param name="targetValue">Bytes representing a type of data.</param>
        void EncodeToHammingCode(byte[] targetValue);

        /// <summary>
        /// Retrieves the data given to the HammingObject. 
        /// </summary>
        /// <returns>Returns the same Object type given to the HammingCode</returns>
        Object RetrieveValue();

        /// <summary>
        /// Examines the current HammingObject and builds a report based on its bits. 
        /// The method will also auto-correct Single-Bit errors.
        /// </summary>
        /// <returns> Gives a HammingReport object back to the caller. </returns>
        HammingReport BuildReport();

    }
}
