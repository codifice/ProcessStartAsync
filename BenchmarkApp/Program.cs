namespace BenchmarkApp
{
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Running;

    internal class Program
    {
        internal static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchmarks>();
        }
    }
}