using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bondski.QvdLib.Benchmark
{
    [SimpleJob]
    public class TransactionsBig
    {
        [Benchmark]
        public void ReadAllRows()
        {
            using (var reader = new QvdReader(@"C:\Temp\Transactions_big.qvd"))
            { 
                long sum = 0;
                while (reader.NextRow())
                {
                    sum += reader[0].Int;
                }
                Console.WriteLine(sum);
            }
        }
    }
}
