using CommandLine;
using CommandLine.Text;
#nullable disable

namespace NoSnooze.Runner
{
    public interface IOptions
    {
        public int? PID { get; set; }
        public int? MaxWaitTime { get; set; }
        public string ShellExecute { get; set; }
        public static IEnumerable<Example> Examples { get; }
        public CommandlineOptionsSet OptionsSet { get; }
        public bool Validates();
    }
    public class Options :IOptions
    {
        [Option('p', "pid", SetName = "ProcId", Required = false, Default = null, HelpText = "Wait for process id (pid) to exit. Value must be greater than or equal to 0. Cannot be combined with the [-x|--shx] option.")]
        public int? PID { get; set; }

        [Option('x', "shx", SetName = "ShellEx", Required = false, Default = null, HelpText = "Execute command. Cannot be combined with the [-p|--pid] option.")]
        public string ShellExecute { get; set; }

        [Option('w', "wait", Required = false, Default = null, HelpText = "Maximum number of seconds to wait before exiting. Value must be from 1 to 2147483.")]
        public int? MaxWaitTime { get; set; }

        [Usage(ApplicationAlias = "NoSnooze")]
        public static IEnumerable<Example> Examples
        {
            get
            {
                return new List<Example>()
                {
                    new Example("Prevent sleep for 60 seconds", new Options { MaxWaitTime = 60 }),
                    new Example("Prevent sleep while process id (pid) 2942 is running", new Options { PID = 2942}),
                    new Example("Prevent sleep for up to 60 seconds while process id (pid) 2942 is running", new Options { MaxWaitTime = 60, PID = 2942}),
                    new Example("Start notepad.exe, and prevent sleep while it is running", new Options { ShellExecute = "notepad.exe" }),
                    new Example("Start notepad.exe, and prevent sleep for up to 60 seconds while it is running", new Options { MaxWaitTime = 60, ShellExecute = "notepad.exe" })
                };
            }
        }

        public CommandlineOptionsSet OptionsSet
        {
            get
            {
                CommandlineOptionsSet opts = CommandlineOptionsSet.None;
                if (this.PID.HasValue) opts |= CommandlineOptionsSet.ProcessId;
                if (this.ShellExecute != null) opts |= CommandlineOptionsSet.ShellExecute;
                if (this.MaxWaitTime.HasValue) opts |= CommandlineOptionsSet.Wait;
                return opts;
            }
        }

        /// <summary>
        /// Check that options are within valid ranges
        /// </summary>
        public bool Validates()
        {
            bool validates = true;
            // PID must be a positive integer
            if (this.PID.HasValue && this.PID < 0)
            {
                validates = false;
                Console.WriteLine("Process id (-p or --pid) must be greater than 0.");
            }
            // MaxWaitTime must be from 1 to 2147483
            if (this.MaxWaitTime.HasValue && (this.MaxWaitTime.Value < 1 || this.MaxWaitTime.Value > 2147483))
            {
                validates = false;
                Console.Write("Wait time (-w or --wait) must be from 1 to 2147483, inclusive.");
            }
            return validates;
        }

    }
}
