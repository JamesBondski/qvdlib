using Bondski.QvdLib;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(DateTime.Now);
            using (QvdReader reader = new QvdReader(@"C:\Temp\big.qvd"))
            {
                while (reader.NextRow()) ;
            }
            Console.WriteLine(DateTime.Now);
        }
    }
}
