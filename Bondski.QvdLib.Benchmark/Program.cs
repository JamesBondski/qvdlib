
using BenchmarkDotNet.Running;
using Bondski.QvdLib.Benchmark;

var summary = BenchmarkRunner.Run<TransactionsBig>();
return;