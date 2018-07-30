namespace BenchmarkApp
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    using BenchmarkDotNet.Attributes;

    [CoreJob]
    public class Benchmarks
    {
        [Benchmark]
        public async Task<int> HappyRunAsync()
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(10));
            return await new ProcessStartInfo("cmd.exe", "/c @echo \"Hello World!\"").StartAsync(cts.Token);
        }
    }
}