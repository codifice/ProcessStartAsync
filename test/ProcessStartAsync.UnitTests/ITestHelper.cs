namespace System.Diagnostics
{
    public interface ITestHelper
    {
        void Error(string message);

        void Output(string message);

        void ProcessStarted(Process p);
    }
}