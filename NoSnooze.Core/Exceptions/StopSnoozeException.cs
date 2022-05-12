
namespace StopSnooze.Core.Exceptions
{
    /// <summary>
    /// Base class for any StopSnooze core specific exceptions
    /// </summary>
    abstract public class StopSnoozeException : Exception
    {
        public StopSnoozeException()
        { }

        public StopSnoozeException(string message) : base(message)
        { }

        public StopSnoozeException(string message, Exception innerException) : base(message, innerException)
        { }
    }
}
