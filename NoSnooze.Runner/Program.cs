using CommandLine;
using CommandLine.Text;

using StopSnooze.Core;
using StopSnooze.Core.Exceptions;
using StopSnooze.Runner;

var parseResult = CommandLine.Parser.Default.ParseArguments<Options>(args);
IPowerTask powerTask = PowerTask.Create();
IJobRunner jobRunner = JobRunner.Create(parseResult, powerTask);
ExitCode exitCode = jobRunner.Run();

Environment.Exit((int)exitCode);

