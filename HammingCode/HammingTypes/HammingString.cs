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
            // Conversion: 1 char = 2 bytes
            byte[] conversion = new byte[chars.Length * 2];

            int byteIndex = 0;
            foreach (char c in chars)
            {
                conversion[byteIndex++] = (byte)(c & FIRST_BYTE_MASK); // low byte
                conversion[byteIndex++] = (byte)((c >> 8) & FIRST_BYTE_MASK); // high byte
            }

            EncodeToHammingCode(conversion);
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
        public static HammingString EncodeString(String num) => new HammingString(num.ToCharArray());

        /// <summary>
        /// Encodes an array of characters into a hamming sequence.
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static HammingString EncodeCharArray(char[] chars) => new HammingString(chars);

        #endregion

        #region Override Methods
        public override string ToString() => RetrieveValue().ToString();
        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            HammingString other = obj as HammingString;
            if (other == null)
                return false;

            String thisValue = this.RetrieveValue() as String;
            String otherValue = other.RetrieveValue() as String;
            if (thisValue != null && otherValue != null && thisValue.Equals(otherValue))
                return true;
            else if (thisValue == null && otherValue == null)
                return true;
            return false;
        }


        public override Object RetrieveValue()
        {
            byte[] data = base.RetrieveValue() as byte[]; // data.Length guarenteed to be even

            char[] charArray = new char[data.Length / 2];
            int charIndex = charArray.Length - 1;
            for (int byteIndex = data.Length - 1; byteIndex >= 0; )
            {
                char nextChar = (char)data[byteIndex--]; // high byte
                nextChar <<= 8;
                nextChar &= (char)data[byteIndex--]; // low byte
                charArray[charIndex--] = nextChar;  // insert character
            }

            return charArray.ToString();
        }

        #endregion

    }
}
