using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StopSnooze.Core
{

    public enum StateChangeFailure
    {
        [EnumMessage("Failed to set StopSnooze state - could not set execution state flags (EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS)")]
        SetStopSnoozeFailed,
        [EnumMessage("Failed to clear StopSnooze state - could not set execution state flags (EXECUTION_STATE.ES_CONTINUOUS)")]
        ClearStopSnoozeFailed
    }

    public enum WaitExitResult
    {
        Success = 0,
        Other,
        ProcessNotFound,
        ProcessStartFailed,
        Timeout
    }

    public enum ExitWaitTime
    {
        Indefinite = -1,
        Maximum = Int32.MaxValue
    }

    /// <summary>
    /// Attribute for a String message applied to an enum member.
    /// Allows an error code (enum value) to be directly paired with an error message.
    /// </summary>
    public class EnumMessage : Attribute
    {
        public string Value { get; private set; }
        public EnumMessage(string value)
        {
            Value = value;
        }
    }

}
