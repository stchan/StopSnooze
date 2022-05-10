using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSnooze.Core.Exceptions
{
    public class ShellExecuteException : NoSnoozeException
    {
        public ShellExecuteException(string message) : base(message)
        { }

        public ShellExecuteException(string message, Exception innerException) : base(message, innerException)
        { }

    }
}
