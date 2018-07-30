namespace System.Diagnostics
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Moq;

    using Xunit;

    // ReSharper disable once TestFileNameWarning
    public class ProcessStartInfoExUnitTests
    {
        public interface ITestHelper
        {
            void Error(string message);

            void Output(string message);

            void ProcessStarted(Process p);
        }

        [Fact]
        public async Task CanCancelALongRunningProcess()
        {
            var psi = new ProcessStartInfo("cmd.exe", "/c ping 127.0.0.1 -n 5");
            var helper = new Mock<ITestHelper>();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(0.5));
            var sw = new Stopwatch();
            sw.Start();
            await Assert.ThrowsAsync<TaskCanceledException>(
                () => psi.StartAsync(
                    helper.Object.Output,
                    helper.Object.Error,
                    helper.Object.ProcessStarted,
                    cts.Token)).ConfigureAwait(false);
            sw.Stop();
            sw.Elapsed.Should().BeLessThan(TimeSpan.FromSeconds(0.6));
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Once);
            helper.Verify(h => h.Output(It.IsAny<string>()), Times.AtLeastOnce);
            helper.Verify(h => h.Error(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CanInvokeAProcessAndCaptureStandardOutput()
        {
            var psi = new ProcessStartInfo("cmd.exe", "/c @echo Hello World");
            var helper = new Mock<ITestHelper>();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            var result = await psi.StartAsync(
                             helper.Object.Output,
                             helper.Object.Error,
                             helper.Object.ProcessStarted,
                             cts.Token).ConfigureAwait(false);
            result.Should().Be(0);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Once);
            helper.Verify(h => h.Output("Hello World"), Times.Once);
            helper.Verify(h => h.Error(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CanInvokeAProcessThatErrorsAndCaptureStatus()
        {
            var psi = new ProcessStartInfo("cmd.exe", "/c ping 0.0.0.0 -n 1");
            var helper = new Mock<ITestHelper>();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            var result = await psi.StartAsync(
                             helper.Object.Output,
                             helper.Object.Error,
                             helper.Object.ProcessStarted,
                             cts.Token).ConfigureAwait(false);
            result.Should().Be(1);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Once);
            helper.Verify(h => h.Output(It.IsAny<string>()), Times.AtLeastOnce);
            helper.Verify(h => h.Error(It.IsAny<string>()), Times.Never);
        }
    }
}