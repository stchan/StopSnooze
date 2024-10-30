using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StopSnooze.Core;

namespace StopSnooze.Core.Exceptions
{
    /// <summary>
    /// A <see cref="WaitException"/> may be thrown if waiting for a process to exit fails
    /// </summary>
    public class WaitException : StopSnoozeException
    {
        public WaitException(WaitExitResult result, string message) : base(message)
        {
            ExitResult = result;
        }

        public WaitException(WaitExitResult result, string message, Exception innerException) : base(message, innerException)
        {
            ExitResult = result;
        }

        public WaitExitResult ExitResult { get; private set; }
    }
}
