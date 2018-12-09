using System;
using System.Collections.Generic;
using System.Text;
using HammingCode.HammingTypes.Exceptions;

namespace HammingCode.HammingTypes
{
    public class HammingString : HammingBase
    {
        #region Constants
        // mask for grabbing the first byte from a set or sequence of bytes
        private const int FIRST_BYTE_MASK = 0x0FF;
        #endregion

        #region Constructors
        public HammingString() : base() { }

        public HammingString(char[] chars) : base()
        {
            this.EncodeToHammingCode(chars);
        }

        public HammingString(String text) : this(text.ToCharArray())  { }

        public new HammingString Clone()
        {
            String copy = RetrieveValue() as String;

            if (copy != null)
                return new HammingString(copy);
            else return new HammingString();
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Encodes a String into a hamming sequence.
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static HammingString EncodeString(String text) => new HammingString(text.ToCharArray());

        /// <summary>
        /// Encodes an array of characters into a hamming sequence.
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static HammingString EncodeCharArray(char[] text) => new HammingString(text);

        #endregion

        #region Public Methods
        public override string ToString() => RetrieveValue().ToString();
        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            HammingString other = obj as HammingString;
            if (other == null)
                return false;

            if (this.GetHammingBytes() == null && other.GetHammingBytes() == null) // both: empty check
                return true;
            if (this.GetHammingBytes() == null || other.GetHammingBytes() == null) // One is empty
                return false;

            // Neither are empty
            String thisValue = this.RetrieveValue();
            String otherValue = other.RetrieveValue();
            if (thisValue != null && otherValue != null && thisValue.Equals(otherValue))
                return true;
            else if (thisValue == null && otherValue == null)
                return true;
            return false;
        }


        public new String RetrieveValue()
        {
            byte[] data = base.RetrieveValue() as byte[]; // data.Length guarenteed to be even

            char[] charArray = new char[data.Length / 2];
            int charIndex = charArray.Length - 1;
            for (int byteIndex = data.Length - 1; byteIndex >= 0; )
            {
                char nextChar = (char)data[byteIndex--]; // high byte
                nextChar <<= 8;
                nextChar |= (char)data[byteIndex--]; // low byte
                charArray[charIndex--] = nextChar;  // insert character
            }

            return new StringBuilder().Append(charArray).ToString();            
        }

        public virtual void EncodeToHammingCode(String text)
        {
            this.EncodeToHammingCode(text.ToCharArray());
        }

        public virtual void EncodeToHammingCode(char[] value)
        {
            // Conversion: 1 char = 2 bytes
            byte[] conversion = new byte[value.Length * 2];

            int byteIndex = 0;
            foreach (char c in value)
            {
                conversion[byteIndex++] = (byte)(c & FIRST_BYTE_MASK); // low byte
                conversion[byteIndex++] = (byte)((c >> 8) & FIRST_BYTE_MASK); // high byte
            }

            EncodeToHammingCode(conversion);
        }

        #endregion


    }
}
