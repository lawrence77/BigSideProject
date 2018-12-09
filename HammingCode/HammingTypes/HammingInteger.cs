using System;
using System.Collections.Generic;
using System.Text;
using HammingCode.HammingTypes.Exceptions;

namespace HammingCode.HammingTypes
{
    public class HammingInteger : HammingBase
    {
        #region Constants

        // mask for grabbing the first byte from a set or sequence of bytes
        private const int FIRST_BYTE_MASK = 0x0FF;

        #endregion

        #region Properties

        private TypeOfInteger status { get; set; } 

        #endregion

        #region Constructors
        /// <summary>
        /// Creates an empty HammingInteger
        /// </summary>
        public HammingInteger() : base()
        {
            // Nothing inserted
            status = TypeOfInteger.None;
        }

        /// <summary>
        /// Creates a HammingInteger for a byte.
        /// </summary>
        /// <param name="num"></param>
        public HammingInteger(byte num) : base()
        {
            EncodeToHammingCode(num);
        }       

        /// <summary>
        /// Creates a HammingInteger for a short.
        /// </summary>
        /// <param name="num"></param>
        public HammingInteger(Int16 num) : base()
        {
            EncodeToHammingCode(num);            
        }

        /// <summary>
        /// Creates a HammingInteger for an int.
        /// </summary>
        /// <param name="num"></param>
        public HammingInteger(Int32 num) : base()
        {
            EncodeToHammingCode(num);
        }

        /// <summary>
        /// Creates a HammingInteger for a long
        /// </summary>
        /// <param name="num"></param>
        public HammingInteger(Int64 num) : base()
        {
            EncodeToHammingCode(num);
        }

        /// <summary>
        /// Creates a HammingInteger for an array of bytes
        /// </summary>
        /// <param name="bytes"></param>
        public HammingInteger(byte[] bytes) : base()
        {
            EncodeToHammingCode(bytes);
        }

        public new HammingInteger Clone()
        {
            switch (status)
            {
                case TypeOfInteger.Int8:
                    byte? byteCopy = RetrieveValue() as byte?;
                    if (byteCopy.HasValue)
                        return new HammingInteger(byteCopy.Value);
                    goto case TypeOfInteger.None;

                case TypeOfInteger.Int16:
                    Int16? shortCopy = RetrieveValue() as Int16?;
                    if (shortCopy.HasValue)
                        return new HammingInteger(shortCopy.Value);
                    goto case TypeOfInteger.None;

                case TypeOfInteger.Int32:
                    Int32? int32Copy = RetrieveValue() as Int32?;
                    if (int32Copy.HasValue)
                        return new HammingInteger(int32Copy.Value);
                    goto case TypeOfInteger.None;

                case TypeOfInteger.Int64:
                    Int64? int64Copy = RetrieveValue() as Int64?;
                    if (int64Copy.HasValue)
                        return new HammingInteger(int64Copy.Value);
                    goto case TypeOfInteger.None;
                case TypeOfInteger.ArrayOfBytes:
                    byte[] byteArray = RetrieveValue() as byte[];
                    if (byteArray != null)
                        return new HammingInteger(byteArray);
                    goto case TypeOfInteger.None;

                case TypeOfInteger.None:
                default:
                    return new HammingInteger();
            }            
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Encodes a byte into a hamming sequence.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static HammingInteger EncodeByte(byte num) => new HammingInteger(num);

        /// <summary>
        /// Encodes a short into a hamming sequence.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static HammingInteger EncodeShort(Int16 num) => new HammingInteger(num);

        /// <summary>
        /// Encodes an int into a hamming sequence.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static HammingInteger EncodeInt(Int32 num) => new HammingInteger(num);

        /// <summary>
        /// Encodes a long into a hamming sequence.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static HammingInteger EncodeLong(Int64 num) => new HammingInteger(num);

        #endregion

        #region Public Methods
        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => RetrieveValue().ToString();

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            HammingInteger other = obj as HammingInteger;
            if (other == null)
                return false;

            if (this.status == other.status)
            {
                switch(this.status)
                {
                    case TypeOfInteger.None:
                        return true;

                    case TypeOfInteger.ArrayOfBytes:
                        byte[] arrayData = this.RetrieveValue() as byte[];
                        byte[] otherArrayData = other.RetrieveValue() as byte[];
                        if (arrayData == null || otherArrayData == null)
                            return false;
                        if (arrayData.Length == otherArrayData.Length)
                        {
                            for (int i = 0; i < arrayData.Length; i++)
                                if (arrayData[i] != otherArrayData[i])
                                    return false;
                            return true;
                        }
                        return false;

                    case TypeOfInteger.Int8:
                        byte? thisByte= this.RetrieveValue() as byte?;
                        byte? otherByte= other.RetrieveValue() as byte?;
                        if (thisByte == null || otherByte == null)
                            return false;
                        if (thisByte == otherByte)
                            return true;
                        return false;    
                        
                    case TypeOfInteger.Int16:
                        Int16? thisInt16 = this.RetrieveValue() as Int16?;
                        Int16? otherInt16 = other.RetrieveValue() as Int16?;
                        if (thisInt16 == null || otherInt16 == null)
                            return false;
                        if (thisInt16 == otherInt16)
                            return true;
                        return false;     
                        
                    case TypeOfInteger.Int32:
                        Int32? thisInt32 = this.RetrieveValue() as Int32?;
                        Int32? otherInt32 = other.RetrieveValue() as Int32?;
                        if (thisInt32 == null || otherInt32 == null)
                            return false;
                        if (thisInt32 == otherInt32)
                            return true;
                        return false;

                    case TypeOfInteger.Int64:
                        Int64? thisInt64 = this.RetrieveValue() as Int64?;
                        Int64? otherInt64 = other.RetrieveValue() as Int64?;
                        if (thisInt64 == null || otherInt64 == null)
                            return false;
                        if (thisInt64 == otherInt64)
                            return true;
                        return false;
                }
            }            
            return false;
        }

        public virtual string GetIntegerType() => status.ToString();

        public override Object RetrieveValue()
        {
            byte[] data = base.RetrieveValue() as byte[];

            switch (status)
            {
                case TypeOfInteger.Int8:
                    return data[0];

                case TypeOfInteger.Int16:
                    Int16 shortNum = data[1]; // high byte
                    shortNum <<= 8;
                    shortNum |= data[0]; // low byte
                    return shortNum;

                case TypeOfInteger.Int32:
                    Int32 intNum = data[3]; // high byte
                    for (int i = 2; i >= 0; i--)
                    {
                        intNum <<= 8;
                        intNum |= data[i]; // lower bytes
                    }
                    return intNum;

                case TypeOfInteger.Int64:
                    Int64 longNum = data[7];
                    for (int i = 6; i >= 0; i--)
                    {
                        longNum <<= 8;
                        longNum |= data[i]; // lower bytes
                    }
                    return longNum;

                case TypeOfInteger.ArrayOfBytes:
                    return data;

                case TypeOfInteger.None:
                default:
                    throw new EmptyHammingObjectException("HammingInteger is Empty");
            }
        }

        /// <summary>
        /// Given an array of bytes, creates a Hamming-Sequence of bits that protect the data.
        /// </summary>
        /// <param name="targetValue">Bytes representing a type of data.</param>
        public override void EncodeToHammingCode(byte[] targetValue)
        {
            base.EncodeToHammingCode(targetValue);
            status = TypeOfInteger.ArrayOfBytes;
        }

        public virtual void EncodeToHammingCode(byte num)
        {
            // Conversion: 1:1
            EncodeToHammingCode(new byte[1] { num });
            status = TypeOfInteger.Int8;
        }

        public virtual void EncodeToHammingCode(Int16 num)
        {
            // Conversion: 1 int16 = 2 bytes
            byte[] conversion = new byte[2];

            conversion[0] = (byte)(num & FIRST_BYTE_MASK);
            conversion[1] = (byte)((num >> 8) & FIRST_BYTE_MASK);

            EncodeToHammingCode(conversion);
            status = TypeOfInteger.Int16;
        }

        public virtual void EncodeToHammingCode(Int32 num)
        {
            // Conversion: 1 int32 = 4 bytes
            byte[] conversion = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                int bitShift = i * 8;
                conversion[i] = (byte)((num >> bitShift) & FIRST_BYTE_MASK);
            }

            EncodeToHammingCode(conversion);
            status = TypeOfInteger.Int32;
        }

        public virtual void EncodeToHammingCode(Int64 num)
        {
            // Conversion: 1 Int64 = 8 bytes
            byte[] conversion = new byte[8];

            for (int i = 0; i < 8; i++)
            {
                int bitShift = i * 8;
                conversion[i] = (byte)((num >> bitShift) & FIRST_BYTE_MASK);
            }

            EncodeToHammingCode(conversion);
            status = TypeOfInteger.Int64;
        }

        #endregion

        #region Private Enum

        private enum TypeOfInteger
        {
            None,
            Int8,
            Int16,
            Int32,
            Int64,
            ArrayOfBytes
        }

        #endregion
    }
}
