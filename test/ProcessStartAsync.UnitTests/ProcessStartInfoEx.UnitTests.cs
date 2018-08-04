namespace System.Diagnostics
{
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Moq;

    using Xunit;

    // ReSharper disable once TestFileNameWarning
    public static class ProcessStartInfoExUnitTests
    {
        private const string StandardErrorMessage = "Goodbye Cruel World";

        private const string StandardOutputMessage = "Hello World";

        [Fact]
        public static async Task CanCancelALongRunningProcess()
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
            sw.Elapsed.Should().BeLessThan(TimeSpan.FromSeconds(2));
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Once);
            helper.Verify(h => h.Output(It.IsAny<string>()), Times.AtLeastOnce);
            helper.Verify(h => h.Error(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public static async Task CanInvokeAndMonitorOutput()
        {
            var psi = new ProcessStartInfo("cmd.exe", $"/c @echo {StandardOutputMessage}");
            var helper = new Mock<ITestHelper>();
            var result = await psi.StartAsync(helper.Object.Output).ConfigureAwait(false);
            result.Should().Be(0);
            helper.Verify(h => h.Output(StandardOutputMessage), Times.Once);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Never);
            helper.Verify(h => h.Error(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public static async Task CanInvokeAndMonitorOutputAndCancellationToken()
        {
            var psi = new ProcessStartInfo("cmd.exe", $"/c @echo {StandardOutputMessage}");
            var helper = new Mock<ITestHelper>();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            var result = await psi.StartAsync(helper.Object.Output, cts.Token).ConfigureAwait(false);
            result.Should().Be(0);
            helper.Verify(h => h.Output(StandardOutputMessage), Times.Once);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Never);
            helper.Verify(h => h.Error(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public static async Task CanInvokeAndMonitorOutputAndError()
        {
            var psi = new ProcessStartInfo(
                "cmd.exe",
                $"/c @echo {StandardOutputMessage} && @echo {StandardErrorMessage} 1>&2");
            var helper = new Mock<ITestHelper>();
            var result = await psi.StartAsync(helper.Object.Output).ConfigureAwait(false);
            result.Should().Be(0);
            helper.Verify(h => h.Output(StandardOutputMessage), Times.Once);
            helper.Verify(h => h.Output(StandardErrorMessage), Times.Once);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Never);
        }

        [Fact]
        public static async Task CanInvokeAndMonitorOutputAndErrorAndCancellationToken()
        {
            var psi = new ProcessStartInfo(
                "cmd.exe",
                $"/c @echo {StandardOutputMessage} && @echo {StandardErrorMessage} 1>&2");
            var helper = new Mock<ITestHelper>();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            var result = await psi.StartAsync(helper.Object.Output, helper.Object.Error, cts.Token)
                             .ConfigureAwait(false);
            result.Should().Be(0);
            helper.Verify(h => h.Output(StandardOutputMessage), Times.Once);
            helper.Verify(h => h.Error(StandardErrorMessage), Times.Once);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Never);
        }

        [Fact]
        public static async Task CanInvokeAndMonitorOutputAndErrorAndProcessStarted()
        {
            var psi = new ProcessStartInfo(
                "cmd.exe",
                $"/c @echo {StandardOutputMessage} && @echo {StandardErrorMessage} 1>&2");
            var helper = new Mock<ITestHelper>();
            var result = await psi.StartAsync(helper.Object.Output, helper.Object.Error, helper.Object.ProcessStarted)
                             .ConfigureAwait(false);
            result.Should().Be(0);
            helper.Verify(h => h.Output(StandardOutputMessage), Times.Once);
            helper.Verify(h => h.Error(StandardErrorMessage), Times.Once);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Once);
        }

        [Fact]
        public static async Task CanInvokeAndMonitorOutputAndErrorExplicitly()
        {
            var psi = new ProcessStartInfo(
                "cmd.exe",
                $"/c @echo {StandardOutputMessage} && @echo {StandardErrorMessage} 1>&2");
            var helper = new Mock<ITestHelper>();
            var result = await psi.StartAsync(helper.Object.Output, helper.Object.Error).ConfigureAwait(false);
            result.Should().Be(0);
            helper.Verify(h => h.Output(StandardOutputMessage), Times.Once);
            helper.Verify(h => h.Error(StandardErrorMessage), Times.Once);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Never);
        }

        [Fact]
        public static async Task CanInvokeAProcessAndCaptureStandardError()
        {
            var psi = new ProcessStartInfo("cmd.exe", $"/c @echo {StandardErrorMessage} 1>&2");
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
            helper.Verify(h => h.Error(StandardErrorMessage), Times.Once);
            helper.Verify(h => h.Output(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public static async Task CanInvokeAProcessAndCaptureStandardOutput()
        {
            var psi = new ProcessStartInfo("cmd.exe", $"/c @echo {StandardOutputMessage}");
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
            helper.Verify(h => h.Output(StandardOutputMessage), Times.Once);
            helper.Verify(h => h.Error(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public static async Task CanInvokeAProcessThatErrorsAndCaptureStatus()
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

        [Fact]
        public static async Task CanInvokeWithJustCancellationToken()
        {
            var psi = new ProcessStartInfo("cmd.exe", $"/c @echo {StandardOutputMessage}");
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            var result = await psi.StartAsync(cts.Token).ConfigureAwait(false);
            result.Should().Be(0);
        }

        [Fact]
        public static async Task CanInvokeWithNoArguments()
        {
            var psi = new ProcessStartInfo("cmd.exe", $"/c @echo {StandardOutputMessage}");
            var result = await psi.StartAsync().ConfigureAwait(false);
            result.Should().Be(0);
        }
    }
}