using System;
using System.Collections.Generic;
using System.Text;
using HammingCode.HammingTypes.Exceptions;

namespace HammingCode.HammingTypes
{
    public class HammingObject : IHammingObject
    {
        #region Properties
        public bool IsErroneous { get; set; }
        protected byte[] HammingBytes { get; set; }
        private int DataBitsSize { get; set; }
        
        #endregion

        #region Constructors
        /// <summary>
        /// Base Constructor for the HammingObject. 
        /// </summary>
        public HammingObject()
        {
            IsErroneous = false;
            HammingBytes = null;
            DataBitsSize = 0;
        }

        /// <summary>
        /// Constructs a HammingObject with a byte array converted to a hamming sequence.
        /// </summary>
        /// <param name="targetValues">Byte array that will be converted to Hamming Code</param>
        public HammingObject(byte[] targetValues) : this()
        {
            EncodeToHammingCode(targetValues);
        }
        
        /// <summary>
        /// Returns a deep copy of this HammingObject.
        /// </summary>
        /// <returns>Returns a new object with the same Data value and encoding as this Hamming Object. </returns>
        public HammingObject Clone()
        {
            HammingObject clone = new HammingObject(this.RetrieveValue() as byte[]);
            if (this.IsErroneous)
                clone.IsErroneous = true;
            return clone;
        }
        
        #endregion

        #region Interface Methods
        /// <summary>
        /// Given an array of bytes, creates a Hamming-Sequence of bits that protect the data.
        /// </summary>
        /// <param name="targetValue">Bytes representing a type of data.</param>
        public virtual void EncodeToHammingCode(byte[] targetValue)
        {
            //Input errors
            if (targetValue == null)
                throw new HammingObjectInvalidInputException("Input array is Null.");
            if (targetValue.Length < 1)
                throw new HammingObjectInvalidInputException("There are Fewer than 1 Bytes in the input array.");

            //update properties
            DataBitsSize = targetValue.Length * 8;
            HammingBytes = BuildHamming(targetValue, DataBitsSize + GetParityBitsSize());
            IsErroneous = false;
        }

        /// <summary>
        /// Retrieves the data given to the HammingObject. 
        /// </summary>
        /// <returns>Returns the same Object type given to the HammingCode</returns>
        public virtual Object RetrieveValue()
        {
            if (HammingBytes == null)   //error checking
                throw new EmptyHammingObjectException("Hamming Object is empty.");
            return HammingCodeToDataBytesArray();
        }

        /// <summary>
        /// Examines the current HammingObject and builds a report based on its bits. 
        /// The method will also auto-correct Single-Bit errors.
        /// </summary>
        /// <returns> Gives a HammingReport object back to the caller. </returns>
        public virtual HammingReport BuildReport()
        {
            int syndrome = GetSyndrome();
            bool evenParity = IsOverallParityEven();

            ErrorTypesEnum errorType;
            if (syndrome == 0 && evenParity)
                errorType = ErrorTypesEnum.NoError;
            else if (syndrome == 0 && !evenParity)
                errorType = ErrorTypesEnum.MasterParityBitError;
            else if (evenParity && syndrome != 0)
                errorType = ErrorTypesEnum.MultiBitError;
            else if (!evenParity && IsPowerOf2(syndrome))
                errorType = ErrorTypesEnum.ParityBitError;
            else errorType = ErrorTypesEnum.DataBitError;

            HammingReport report = new HammingReport(errorType, syndrome);
            IsErroneous = !CorrectError(report);
            return report;
        }

        /// <summary>
        /// <code>SimualteError</code> is a method for incorrectly flipping a bit.
        /// </summary>     
        public virtual void SimulateRandomError()
        {
            if (HammingBytes == null)   //error checking
                throw new EmptyHammingObjectException("Hamming Object is empty.");

            Random random = new Random();
            int numberOfErrors = random.Next(3);        //0, 1, or 2 errors

            int totalBits = DataBitsSize + GetParityBitsSize();
            if (numberOfErrors > 0)
            {
                FlipBitAt(random.Next(totalBits));  //first error
            }
            if (numberOfErrors > 1)                 
            {
                FlipBitAt(random.Next(totalBits));  //second error (And yes, there's a chance that it corrects the first flipped bit)
            }            
        }

        #endregion

        #region Override Methods
        /// <summary>
        /// Converts this object to its visual representation.
        /// </summary>
        /// <returns>Returns a string. </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(BitConverter.ToString(HammingBytes));
            sb.Insert(0, "0x");
            sb.Replace("-", ", 0x");
            return sb.ToString().ToLower();
        }

        /// <summary>
        /// Checks if another object is equivalent to this HammingObject.
        /// </summary>
        /// <param name="obj">Another object.</param>
        /// <returns>Returns true if the other is equivalent based on type and properties. </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            HammingObject other = obj as HammingObject;
            if (other == null)
                return false;

            byte[] thisArray = this.RetrieveValue() as byte[];
            byte[] otherArray = other.RetrieveValue() as byte[];
            if (thisArray.Length != otherArray.Length)
            {
                return false;
            }
            for (int i = 0; i < thisArray.Length; i++)
            {
                if (thisArray[i] != otherArray[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the hash code of this HammingObject.
        /// </summary>
        /// <returns> Returns the hash code for this object. </returns>
        public override int GetHashCode() 
        {
            return base.GetHashCode();
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// Checks if the parameter if a power of two.
        /// </summary>
        /// <param name="target">An integer value</param>
        /// <returns>Returns true if the parameters was a power of two, else it returns false. </returns>
        protected virtual bool IsPowerOf2(int target)
        {
            if (target == 0 || target == 1 || target == 2)
                return true;

            if (target % 2 == 1)    //any odd target is false
                return false;
            else return IsPowerOf2(target / 2);
        }
        
        /// <summary>
        /// Calculates the number of Parity Bits needed for the Data Bits.
        /// </summary>
        /// <returns>Returns a positive integer. </returns>
        protected virtual int GetParityBitsSize()
        {
            if (DataBitsSize == 0)   //error checking
                throw new EmptyHammingObjectException("Data Bit size not set");
            return (int)Math.Floor(Math.Log(DataBitsSize, 2)) + 2;     //Plus two for Master Parity Bit and Parity Bit 1.
        }
        
        /// <summary>
        /// Determines if the value of a bit within the target.
        /// </summary>
        /// <param name="target">A byte containing a set of 8 bits.</param>
        /// <param name="offset">The location of the bit within the byte. </param>
        /// <returns>Returns true if the bit is one or false if the bit is zero.</returns>
        protected virtual bool GetBit(byte target, int offset)
        {
            target >>= offset;
            target &= 0x01;

            if (target == 1)
                return true;
            return false;
        }

        /// <summary>
        /// Sets a specific bit to a given value. Note, that no change will occur if the bit already has the stated value.
        /// </summary>
        /// <param name="target">A byte that contains the bit.</param>
        /// <param name="offset">Location of the bit within the byte. Must be a zero or positive integer less than eight.</param>
        /// <param name="value">Value the the bit will be set to. Note, any values other than 0 or 1 will not cause a change to occur.</param>
        protected virtual void SetBit(ref byte target, int offset, int value = 1)
        {
            if (offset >= 8 || offset < 0)        //Error checking
                return;

            byte mask = (byte)(target >> offset);
            mask &= 0x01;
            if (mask == 1 && value == 0)                  //set bit to 0.
            {
                mask <<= offset;
                target &= (byte)(~mask);
            }
            else if (mask == 0 && value == 1)            //set bit to 1.
            {
                mask = (byte)(1 << offset);
                target |= mask;
            }
        }

        /// <summary>
        /// Flips a bit given a location within the HammingBytes. 
        /// </summary>
        /// <param name="index">Index location of the bit</param>
        protected virtual void FlipBitAt(int index)
        {
            if (HammingBytes == null)                                   //error checking
                return;

            if (GetBit(HammingBytes[index / 8], index % 8))             //Bit is 1.
            {
                SetBit(ref HammingBytes[index / 8], index % 8, 0);      //Set to 0.
            }
            else                                                        //Bit is 0.
            {
                SetBit(ref HammingBytes[index / 8], index % 8, 1);      //Set to 1.
            }
        }
        
        /// <summary>
        /// Constructs the array of Hamming Bytes by placing the Data Bits in the correct locations and by setting the Parity Bits.
        /// </summary>
        /// <param name="dataBytes">Represents the array of data values.</param>
        /// <param name="totalBits">Represents the total number of bits needed in the Hamming Code.</param>
        /// <returns>Returns the fully constructed array of Hamming Bytes.</returns>
        protected virtual byte[] BuildHamming(byte[] dataBytes, int totalBits)
        {
            byte[] hammingBytes = new byte[totalBits % 8 == 0 ? totalBits / 8 : (totalBits / 8) + 1];
            int[] parityCounter = new int[GetParityBitsSize()];
            int dataCounter = 0;

            //Data-Bits Transfer
            for (int hammingCounter = 0; hammingCounter < totalBits; hammingCounter++)    //traverse all the bits in the hamming bits
            {
                if (dataCounter >= (dataBytes.Length * 8))
                    break;
                if (!IsPowerOf2(hammingCounter))
                {
                    bool bit = GetBit(dataBytes[dataCounter / 8], dataCounter % 8);
                    if (bit)
                    {
                        SetBit(ref hammingBytes[hammingCounter / 8], hammingCounter % 8, 1);

                        int temp = hammingCounter; int parityCount = 1; //update parity-bit counters
                        while (temp != 0)       //log(n)
                        {
                            if (temp % 2 == 1)
                                parityCounter[parityCount]++;
                            temp >>= 1;
                            parityCount++;
                        }
                    }
                    dataCounter++;
                }
            }

            //Parity-Bit Calculation
            int parityLocation = 1;
            for (int i = 1; i < parityCounter.Length; i++, parityLocation *= 2)
            {
                if (parityCounter[i] % 2 == 1)
                {
                    SetBit(ref hammingBytes[parityLocation / 8], parityLocation % 8, 1);
                }
            }

            //Set Master-Bit 
            int masterCounter = 0;
            for (int index = 1; index < hammingBytes.Length * 8; index++)
            {
                if (GetBit(hammingBytes[index / 8], index % 8))
                {
                    masterCounter++;
                }
            }
            if (masterCounter % 2 == 1)
                SetBit(ref hammingBytes[0], 0, 1);

            return hammingBytes;
        }

        /// <summary>
        /// Converts the Hamming Code back to an array of byte values without any parity bits.
        /// </summary>
        /// <returns>Returns the fully constructed array of bytes with only data bits. </returns>
        protected virtual byte[] HammingCodeToDataBytesArray()
        {
            if (HammingBytes == null)
                throw new EmptyHammingObjectException("Hamming Object is empty");

            byte[] data = new byte[DataBitsSize / 8];

            int dataIndex = 0;
            for (int index = 0; index < HammingBytes.Length * 8 && dataIndex < DataBitsSize; index++)
            {
                if (!IsPowerOf2(index))
                {
                    if (GetBit(HammingBytes[index / 8], index % 8))
                        SetBit(ref data[dataIndex / 8], dataIndex % 8, 1);
                    dataIndex++;
                }
            }

            return data;
        }

        /// <summary>
        /// Calculates the syndrome of the Hamming Code. 
        /// </summary>
        /// <returns> Returns a zero or positive integer to represent the Syndrome value.
        /// </returns>
        protected virtual int GetSyndrome()
        {
            if (HammingBytes == null)
                throw new EmptyHammingObjectException("Hamming Object is empty");

            int[] parityCounter = new int[GetParityBitsSize()];

            //calculate parity counters
            for (int i = 0; i < HammingBytes.Length * 8; i++)
            {
                if (!IsPowerOf2(i) && GetBit(HammingBytes[i / 8], i % 8))
                {
                    int temp = i; int parityCount = 1; //update parity-bit counters
                    while (temp != 0)       //log(n)
                    {
                        if (temp % 2 == 1)
                            parityCounter[parityCount]++;
                        temp >>= 1;
                        parityCount++;
                    }
                }
            }

            //check for disagreeable parity bits excluding the master parity bit
            int syndrome = 0;
            for (int i = 1, j = 1; i < HammingBytes.Length * 8; i *= 2, j++)
            {
                if (GetBit(HammingBytes[i / 8], i % 8) && parityCounter[j] % 2 == 0)        //disagreeable
                {
                    syndrome += i;
                }
                else if (!GetBit(HammingBytes[i / 8], i % 8) && parityCounter[j] % 2 == 1)   //disagreeable
                {
                    syndrome += i;
                }
            }

            return syndrome;
        }

        /// <summary>
        /// Determines if the entire sequence of bytes has an Overall Parity that is even. 
        /// This includes all bits such as the Master Parity bit, Parity Bits, and Data Bits.
        /// </summary>
        /// <returns> True is the Overall Parity is even, and false if the Overall Parity is odd. </returns>
        protected virtual bool IsOverallParityEven()
        {
            if (HammingBytes == null)  
                throw new EmptyHammingObjectException("Hamming Object is empty");

            //count overall parity
            int overallCounter = 0;
            for (int i = 0; i < HammingBytes.Length * 8; i++)
            {
                if (GetBit(HammingBytes[i / 8], i % 8))
                    overallCounter++;
            }
            return (overallCounter % 2 == 0);
        }

        /// <summary>
        /// Attempts to correct any error in the HammingObject.
        /// </summary>
        /// <param name="report">Report will indicate the status and syndrome of this HammingObject</param>
        /// <returns>Returns true if the HammingObject has no errors and that is successfully corrected any errors. Returns false
        /// if errors were detected, but unable to fix. </returns>
        protected virtual bool CorrectError(HammingReport report)
        {
            if (HammingBytes == null)
                throw new EmptyHammingObjectException("Hamming Object is empty");

            bool success = false;
            switch (report.Status)
            {
                case ErrorTypesEnum.NoError:
                    success = true;
                    break;
                case ErrorTypesEnum.MasterParityBitError:
                case ErrorTypesEnum.ParityBitError:
                case ErrorTypesEnum.DataBitError:
                    FlipBitAt(report.Syndrome);
                    success = true;
                    break;
                case ErrorTypesEnum.MultiBitError:
                    success = false;
                    break;
            }
            return success;
        }

        #endregion
    }
}
