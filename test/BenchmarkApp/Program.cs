namespace BenchmarkApp
{
    using BenchmarkDotNet.Running;

    internal static class Program
    {
        internal static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }
}