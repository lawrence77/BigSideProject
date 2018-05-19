using System;
using System.Collections.Generic;
using System.Text;

namespace HammingCode.Services
{
    public class Helper
    {

        public byte[] BuildHamming(byte[] dataBytes, int byteSize, int numberOfParityBits)
        {
            //sets Hamming elements to zero.
            byte[] hammingBytes = new byte[byteSize];
            int[] parityCounter = new int[numberOfParityBits];
            int dataCounter = 0;

            //Data-Bits Transfer
            for (int hammingCounter = 0; hammingCounter < byteSize*8; hammingCounter++)    //traverse all the bits in the hamming bits
            {
                if (dataCounter >= (dataBytes.Length * 8))
                    break;
                if (!IsPowerOf2(hammingCounter))
                {
                    bool bit = GetBit(dataBytes[dataCounter / 8], dataCounter % 8);
                    if (bit)
                    {
                        hammingBytes[hammingCounter / 8] = SetBit(hammingBytes[hammingCounter / 8], hammingCounter % 8, 1);

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
                    hammingBytes[parityLocation / 8 ] = SetBit(hammingBytes[parityLocation / 8], parityLocation % 8, 1);                    
                }
            }

            //Set Master-Bit 
            int masterCounter = 0;
            for (int index = 1; index < hammingBytes.Length*8; index++)
            {
                if (GetBit(hammingBytes[index / 8], index % 8))
                {
                    masterCounter++;
                }
            }
            if (masterCounter % 2 == 1)
                hammingBytes[0] = SetBit(0, 0, 1);

            return hammingBytes;
        }

        public byte[] HammingCodeToDataBytesArray(int dataBitSize, byte[] hammingBytes)
        {
            throw new NotImplementedException();
        }

        public bool GetBit(byte target, int offset)
        {
            target >>= offset;
            target &= 0x01;

            if (target == 1)
                return true;
            return false;
        }

        public byte SetBit(byte target, int offset, int value = 1)
        {
            byte mask = (byte)(target >> offset);
            mask &= 0x01;

            if (mask == 1 && value == 0)                  //set bit to 0.
            {
                mask <<= offset;
                target &= (byte)(~mask);
            }
            else if (mask == 0 && value == 1)            //set bit to 1.
            {
                mask = (byte) (1 << offset);
                target |= mask;
            }

            return target;                          
        }

        public bool IsPowerOf2(int target)
        {
            if (target == 0 || target == 1 || target == 2)
                return true;

            if (target % 2 == 1)    //any odd target is false
                return false;
            else return IsPowerOf2(target / 2);
        }
    }
}
