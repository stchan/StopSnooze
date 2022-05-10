
namespace NoSnooze.Core.Exceptions
{
    /// <summary>
    /// Base class for any NoSnooze core specific exceptions
    /// </summary>
    abstract public class NoSnoozeException : Exception
    {
        public NoSnoozeException()
        { }

        public NoSnoozeException(string message) : base(message)
        { }

        public NoSnoozeException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
