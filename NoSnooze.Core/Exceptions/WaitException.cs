using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NoSnooze.Core;

namespace NoSnooze.Core.Exceptions
{
    /// <summary>
    /// A <see cref="WaitException"/> may be thrown if waiting for a process to exit fails
    /// </summary>
    public class WaitException : NoSnoozeException
    {
        public WaitException(WaitExitResult result, string message) : base(message)
        { }

        public WaitException(WaitExitResult result, string message, Exception innerException) : base(message, innerException)
        { }

        public WaitExitResult ExitResult { get; private set; }
    }
}
