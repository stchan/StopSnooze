using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using StopSnooze.Core;

namespace StopSnooze.Core.Exceptions
{
    /// <summary>
    /// A <see cref="StateChangeException"/> may be thrown
    /// if a thread can't change its execution state
    /// (Win32 API returns 0)
    /// </summary>
    public class StateChangeException : StopSnoozeException
    {
        public StateChangeException(string message, EXECUTION_STATE failure) : base(message)
        {
            this.FailureType = failure;
        }

        public StateChangeException(string message, EXECUTION_STATE failure, Exception innerException) : base(message, innerException)
        {
            this.FailureType = failure;
        }
        /// <summary>
        /// EXECUTION_STATE the thread failed to set
        /// </summary>
        public EXECUTION_STATE FailureType { get; private set; }

    }
}
