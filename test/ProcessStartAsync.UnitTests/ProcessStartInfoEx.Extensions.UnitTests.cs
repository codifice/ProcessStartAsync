namespace System.Diagnostics
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using FluentAssertions;

    using Moq;

    using Xunit;

    // ReSharper disable once TestFileNameWarning
    public partial class ProcessStartInfoExUnitTests
    {
        [Fact]
        public async Task CanInvokeWithJustCancellationToken()
        {
            var psi = new ProcessStartInfo("cmd.exe", "/c @echo Hello World");
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            var result = await psi.StartAsync(
                             cts.Token).ConfigureAwait(false);
            result.Should().Be(0);
        }

        [Fact]
        public async Task CanInvokeAndMonitorOutputAndCancellationToken()
        {
            var psi = new ProcessStartInfo("cmd.exe", "/c @echo Hello World");
            var helper = new Mock<ITestHelper>();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            var result = await psi.StartAsync(
                             helper.Object.Output,
                             cts.Token).ConfigureAwait(false);
            result.Should().Be(0);
            helper.Verify(h => h.Output("Hello World"), Times.Once);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Never);
            helper.Verify(h => h.Error(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CanInvokeAndMonitorOutputAndErrorAndCancellationToken()
        {
            var psi = new ProcessStartInfo("cmd.exe", "/c @echo Hello World && @echo Goodbye Cruel World 1>&2");
            var helper = new Mock<ITestHelper>();
            var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(2));
            var result = await psi.StartAsync(
                             helper.Object.Output,
                             helper.Object.Error,
                             cts.Token).ConfigureAwait(false);
            result.Should().Be(0);
            helper.Verify(h => h.Output("Hello World "), Times.Once);
            helper.Verify(h => h.Error("Goodbye Cruel World "), Times.Once);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Never);
        }

        [Fact]
        public async Task CanInvokeWithNoArguments()
        {
            var psi = new ProcessStartInfo("cmd.exe", "/c @echo Hello World");
            var result = await psi.StartAsync().ConfigureAwait(false);
            result.Should().Be(0);
        }

        [Fact]
        public async Task CanInvokeAndMonitorOutput()
        {
            var psi = new ProcessStartInfo("cmd.exe", "/c @echo Hello World");
            var helper = new Mock<ITestHelper>();
            var result = await psi.StartAsync(helper.Object.Output).ConfigureAwait(false);
            result.Should().Be(0);
            helper.Verify(h => h.Output("Hello World"), Times.Once);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Never);
            helper.Verify(h => h.Error(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task CanInvokeAndMonitorOutputAndError()
        {
            var psi = new ProcessStartInfo("cmd.exe", "/c @echo Hello World && @echo Goodbye Cruel World 1>&2");
            var helper = new Mock<ITestHelper>();
            var result = await psi.StartAsync(
                             helper.Object.Output,
                             helper.Object.Error).ConfigureAwait(false);
            result.Should().Be(0);
            helper.Verify(h => h.Output("Hello World "), Times.Once);
            helper.Verify(h => h.Error("Goodbye Cruel World "), Times.Once);
            helper.Verify(h => h.ProcessStarted(It.IsAny<Process>()), Times.Never);
        }
    }
}