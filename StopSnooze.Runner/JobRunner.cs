using System.Diagnostics;
using System.IO;

using CommandLine;
using CommandLine.Text;

using StopSnooze.Core;
using StopSnooze.Core.Exceptions;
using StopSnooze.Runner;

namespace StopSnooze.Runner
{
    public interface IJobRunner
    {
        ExitCode Run();
    }

    public class JobRunner : IJobRunner
    {
        private readonly ParserResult<Options> _optionsParseResult;
        private readonly IPowerTask _powerTask;

        private JobRunner(ParserResult<Options> optionsResult, IPowerTask powerTask)
        {
            _optionsParseResult = optionsResult;
            _powerTask = powerTask;
        }

        public static IJobRunner Create(ParserResult<Options> optionsResult, IPowerTask powerTask)
        {
            return new JobRunner(optionsResult, powerTask);
        }

        public ExitCode Run()
        {
            ExitCode jobExitCode = ExitCode.Other;
            if (this._optionsParseResult.Tag == ParserResultType.Parsed)
            {
                IOptions jobOptions = ((Parsed<Options>)(_optionsParseResult)).Value;
                if (jobOptions.Validates())
                {
                    jobExitCode = RunStopSnoozeJob(jobOptions);
                }
                else
                {
                    // Options couldn't be validated - set error exit code
                    jobExitCode = ExitCode.ArgumentValidationError;
                }
            }
            else
            {
                // Command line arguments could not be parsed
                jobExitCode = ExitCode.ArgumentParsingError;
            }
            return jobExitCode;
        }

        private ExitCode RunStopSnoozeJob(IOptions jobOptions)
        {
            ExitCode jobExitCode = ExitCode.Success;
            try
            {
                _powerTask.Set();
                switch (jobOptions.OptionsSet)
                {
                    case CommandlineOptionsSet.None:
                        Console.WriteLine("StopSnooze active - press any key to exit...");
                        Console.ReadKey();
                        break;
                    case CommandlineOptionsSet.ProcessId:
                        _powerTask.WaitProcess(jobOptions.PID!.Value);
                        break;
                    case CommandlineOptionsSet.ProcessId | CommandlineOptionsSet.Wait:
                        _powerTask.WaitProcess(jobOptions.PID!.Value, jobOptions.MaxWaitTime!.Value);
                        break;
                    case CommandlineOptionsSet.Wait:
                        _powerTask.Wait(jobOptions.MaxWaitTime!.Value * 1000);
                        break;
                    case CommandlineOptionsSet.ShellExecute:
                    case CommandlineOptionsSet.ShellExecute | CommandlineOptionsSet.Wait:
                        using (Process shellProcess = new Process())
                        {
                            int firstSpaceIndex = jobOptions.ShellExecute.IndexOf(' ');
                            if (firstSpaceIndex == -1)
                            {
                                shellProcess.StartInfo.FileName = jobOptions.ShellExecute;
                            }
                            else
                            {
                                shellProcess.StartInfo.FileName = jobOptions.ShellExecute.Substring(0, firstSpaceIndex);
                                shellProcess.StartInfo.Arguments = jobOptions.ShellExecute.Substring(firstSpaceIndex + 1);
                            }
                            shellProcess.StartInfo.UseShellExecute = true;
                            try
                            {
                                shellProcess.Start();
                            }
                            catch (Exception ex)
                            {
                                throw new ShellExecuteException(ex.Message, ex);
                            }
                            if (jobOptions.MaxWaitTime.HasValue)
                                _powerTask.WaitProcess(shellProcess.Id, jobOptions.MaxWaitTime.Value * 1000);
                            else
                                _powerTask.WaitProcess(shellProcess.Id);
                        }
                        break;
                    default:
                        throw new InvalidOperationException();

                }
            }
            catch (WaitException ex)
            {
                jobExitCode = ExitCode.WaitTimeout;
                Console.WriteLine(ex.Message);
            }
            catch (StateChangeException ex)
            {
                jobExitCode = ExitCode.ExecutionStateChangeFailed;
                Console.WriteLine(ex.Message);
            }
            catch (ShellExecuteException ex)
            {
                jobExitCode = ExitCode.ShellExecuteError;
                Console.WriteLine(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                jobExitCode = ExitCode.InternalApplicationError;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                try
                {
                    _powerTask.Clear();
                }
                catch { }
            }
            return jobExitCode;
        }

    }
}
