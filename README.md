# ProcessStartAsync
[![Build status](https://ci.appveyor.com/api/projects/status/m1svwtwno1g724bk?svg=true)](https://ci.appveyor.com/project/martinjarvis/processstartasync) 
[![Build status](https://sonarcloud.io/api/project_badges/measure?project=ProcessStartAsync&metric=alert_status)](https://sonarcloud.io/dashboard?id=ProcessStartAsync)
[![Codacy Badge](https://api.codacy.com/project/badge/Grade/bc23c9eac02d47c7a35fa9e19262cd79)](https://www.codacy.com/app/martinjarvis/ProcessStartAsync?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=martinjarvis/ProcessStartAsync&amp;utm_campaign=Badge_Grade)
[![codecov](https://codecov.io/gh/martinjarvis/ProcessStartAsync/branch/master/graph/badge.svg)](https://codecov.io/gh/martinjarvis/ProcessStartAsync)

A basic library to launch Processes as Cancellable Tasks

## Usage

### Invoke a process and get the exit code as the result

```csharp
using System.Diagnostics;

// ...

var process = new ProcessStartInfo("cmd.exe", "/c Hello World!");
var result = await process.StartAsync();
result.Should().Be(0);
```

### Invoke a process and cancel if it doesn't complete within a set time

```csharp
using System.Diagnostics;

// ...

try
{
    var process = new ProcessStartInfo("cmd.exe", "/c ping -t 127.0.0.1");
    var cts = new CancellationTokenSource();
    cts.CancelAfter(TimeSpan.FromMinutes(1));
    await process.StartAsync(cts.Token);
}
catch (TaskCanceledException)
{
    // One minute later
}
```