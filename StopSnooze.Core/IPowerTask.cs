using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StopSnooze.Core
{
    public interface IPowerTask : IPowerTaskSetSuccess, IPowerTaskWait, IPowerTaskClear
    {
        EXECUTION_STATE? PreviousPowerState { get; }
        EXECUTION_STATE? PowerState { get; }
        public IPowerTaskSetSuccess Set();
    }

    public interface IPowerTaskSetSuccess : IPowerTaskWait, IPowerTaskClear
    { }

    public interface IPowerTaskWait
    {
        IPowerTaskClear Wait(int milliseconds);
        IPowerTaskClear WaitProcess(int processId, int milliseconds = (int)ExitWaitTime.Indefinite);
    }

    public interface IPowerTaskClear
    {
        void Clear();
    }
}
