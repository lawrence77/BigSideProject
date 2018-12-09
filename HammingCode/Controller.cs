using System;
using HammingCode.HammingTypes;

namespace HammingCode
{
    class Controller
    {
        static void Main(string[] args)
        {
            byte[] bytesArray = { 0x90, 0x01, 0x0a, 0x20 };
            HammingBase code = new HammingBase(bytesArray);

            code.BuildReport();

            byte[] data = code.RetrieveValue() as byte[];
            Console.WriteLine("Success!");
        }
    }
}
