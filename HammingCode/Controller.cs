using System;
using HammingCode.HammingTypes;

namespace HammingCode
{
    class Controller
    {
        static void Main(string[] args)
        {
            BaseHammingObject code = new BaseHammingObject();

            byte[] bytesArray = { 0x90, 0x01, 0x0a, 0x20 };
            code.ConvertByteArrayToHammingCode(bytesArray);

            byte[] data = code.RetrieveValue() as byte[];
            
            //code.SimulateRandomError();

            //code.BuildReport();
            Console.WriteLine("Success!");
            
        }
    }
}
