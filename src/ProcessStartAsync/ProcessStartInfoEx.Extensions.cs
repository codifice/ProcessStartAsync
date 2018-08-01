namespace System.Diagnostics
{
    using System.Threading;
    using System.Threading.Tasks;

    using JetBrains.Annotations;

    public static partial class ProcessStartInfoEx
    {
        /// <summary>
        ///     Start the process in an async manner
        /// </summary>
        /// <param name="startInfo">
        ///     The <see cref="ProcessStartInfo" /> describing the process to launch
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
        public static Task<int> StartAsync(
            [NotNull] this ProcessStartInfo startInfo,
            CancellationToken cancellationToken)
        {
            return StartAsync(startInfo, null, null, null, cancellationToken);
        }

        /// <summary>
        ///     Start the process in an async manner
        /// </summary>
        /// <param name="startInfo">
        ///     The <see cref="ProcessStartInfo" /> describing the process to launch
        /// </param>
        /// <param name="outputMessage">
        ///     Callback for each line of text emitted on the launched Process's Standard Output Stream
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
        public static Task<int> StartAsync(
            [NotNull] this ProcessStartInfo startInfo,
            Action<string> outputMessage,
            CancellationToken cancellationToken)
        {
            return StartAsync(startInfo, outputMessage, null, null, cancellationToken);
        }

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
        /// <param name="cancellationToken">
        ///     The cancellation token.
        /// </param>
        /// <returns>
        ///     The <see cref="Task" /> representing the executing process.  The completed result will be the exit code value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the process cannot be started
        /// </exception>
        public static Task<int> StartAsync(
            [NotNull] this ProcessStartInfo startInfo,
            Action<string> outputMessage,
            Action<string> errorMessage,
            CancellationToken cancellationToken)
        {
            return StartAsync(startInfo, outputMessage, errorMessage, null, cancellationToken);
        }

        /// <summary>
        ///     Start the process in an async manner
        /// </summary>
        /// <param name="startInfo">
        ///     The <see cref="ProcessStartInfo" /> describing the process to launch
        /// </param>
        /// <returns>
        ///     The <see cref="Task" /> representing the executing process.  The completed result will be the exit code value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the process cannot be started
        /// </exception>
        public static Task<int> StartAsync([NotNull] this ProcessStartInfo startInfo)
        {
            return StartAsync(startInfo, null, null, null, CancellationToken.None);
        }

        /// <summary>
        ///     Start the process in an async manner
        /// </summary>
        /// <param name="startInfo">
        ///     The <see cref="ProcessStartInfo" /> describing the process to launch
        /// </param>
        /// <param name="outputMessage">
        ///     Callback for each line of text emitted on the launched Process's Standard Output Stream
        /// </param>
        /// <returns>
        ///     The <see cref="Task" /> representing the executing process.  The completed result will be the exit code value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the process cannot be started
        /// </exception>
        public static Task<int> StartAsync([NotNull] this ProcessStartInfo startInfo, Action<string> outputMessage)
        {
            return StartAsync(startInfo, outputMessage, null, null, CancellationToken.None);
        }

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
        /// <returns>
        ///     The <see cref="Task" /> representing the executing process.  The completed result will be the exit code value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the process cannot be started
        /// </exception>
        public static Task<int> StartAsync(
            [NotNull] this ProcessStartInfo startInfo,
            Action<string> outputMessage,
            Action<string> errorMessage)
        {
            return StartAsync(startInfo, outputMessage, errorMessage, null, CancellationToken.None);
        }

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
        /// <returns>
        ///     The <see cref="Task" /> representing the executing process.  The completed result will be the exit code value.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the process cannot be started
        /// </exception>
        public static Task<int> StartAsync(
            [NotNull] this ProcessStartInfo startInfo,
            Action<string> outputMessage,
            Action<string> errorMessage,
            Action<Process> startedCallback)
        {
            return StartAsync(startInfo, outputMessage, errorMessage, startedCallback, CancellationToken.None);
        }
    }
}