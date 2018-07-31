namespace System.Diagnostics
{
    using System.Threading;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    /// <summary>
    ///     <see cref="ProcessStartInfo" /> extensions.
    /// </summary>
    public static class ProcessStartInfoEx
    {
        /// <summary>
        ///     Start the process in an async manner
        /// </summary>
        /// <param name="startInfo">
        ///     The <see cref="ProcessStartInfo" /> describing the process to launch
        /// </param>
        /// <param name="outputMessage">
        ///     Callback for each line of text emitted on the launched Process's Standard Output Stream
        /// </param>
        /// <param name="errorMessage">
        ///     Callback for each line of text emitted on the launched Process's Standard Error Stream
        /// </param>
        /// <param name="startedCallback">
        ///     Callback invoked with the started <see cref="Process" />
        /// </param>
        /// <param name="cancellationToken">
        ///     The cancellation token.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" /> representing the executing process.  The completed result will be the exit code value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the process cannot be started
        /// </exception>
        public static async Task<int> StartAsync(
            [NotNull] this ProcessStartInfo startInfo,
            Action<string> outputMessage = null,
            Action<string> errorMessage = null,
            Action<Process> startedCallback = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (cancellationToken == default(CancellationToken))
            {
                cancellationToken = CancellationToken.None;
            }

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            var tcs = new TaskCompletionSource<int>();
            var ps = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
            ps.Exited += (sender, eventArgs) =>
                {
                    var code = ps.ExitCode;
                    ps.CancelErrorRead();
                    ps.CancelOutputRead();
                    ps.Dispose();
                    tcs.TrySetResult(code);
                };
            using (cancellationToken.Register(
                () =>
                    {
                        tcs.TrySetCanceled();
                        try
                        {
                            if (ps.HasExited)
                            {
                                return;
                            }

                            ps.Kill();
                            ps.Dispose();
                        }
                        catch (InvalidOperationException)
                        {
                            // Ignore
                        }
                    }))
            {
                cancellationToken.ThrowIfCancellationRequested();
                ps.OutputDataReceived += (s, e) =>
                    {
                        if (e.Data != null)
                        {
                            outputMessage?.Invoke(e.Data);
                        }
                    };
                ps.ErrorDataReceived += (s, e) =>
                    {
                        if (e.Data != null)
                        {
                            errorMessage?.Invoke(e.Data);
                        }
                    };
                if (!ps.Start())
                {
                    throw new InvalidOperationException(
                        $"Failed to start \"{startInfo.FileName} {startInfo.Arguments}\"");
                }

                ps.BeginErrorReadLine();
                ps.BeginOutputReadLine();

                startedCallback?.Invoke(ps);

                return await tcs.Task.ConfigureAwait(false);
            }
        }
    }
}