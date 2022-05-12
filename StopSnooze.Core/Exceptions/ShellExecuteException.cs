using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StopSnooze.Core.Exceptions
{
    public class ShellExecuteException : StopSnoozeException
    {
        public ShellExecuteException(string message) : base(message)
        { }

        public ShellExecuteException(string message, Exception innerException) : base(message, innerException)
        { }

    }
}
