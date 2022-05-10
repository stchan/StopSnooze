namespace NoSnooze.Runner
{
    /// <summary>
    /// Exit codes returned to shell
    /// </summary>
    public enum ExitCode : int
    {
        Success = 0,
        ArgumentParsingError = 1,
        ArgumentValidationError = 2,
        ExecutionStateChangeFailed = 3,
        WaitTimeout = 4,
        ShellExecuteError = 5,
        InternalApplicationError = 998,
        Other = 999
    }


    [Flags]
    public enum CommandlineOptionsSet
    {
        None = 0,
        Wait = 1,
        ProcessId = 2,
        ShellExecute = 4
    }
}