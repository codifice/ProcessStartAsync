using System.Reflection;

[assembly: AssemblyProduct("ProcessStartAsync")]
[assembly: AssemblyTitle("Converts executing a command line process into an Cancellable Task")]
[assembly: AssemblyDescription("A handy ProcessStartInfo extension method for asynchronously calling a command line executable and monitoring their output")]
[assembly: AssemblyCompany("MCJ Development Ltd")]
[assembly: AssemblyCopyright("Copyright © 2018 MCJ Development Ltd")]
[assembly: AssemblyFileVersion("0.0.0.0")]
[assembly: AssemblyInformationalVersion("0.0.0.0-alpha")]
[assembly: AssemblyVersion("0.0.0.0")]

#if RELEASE

[assembly: AssemblyConfiguration("Release")]

#else

[assembly: AssemblyConfiguration("Debug")]

#endif