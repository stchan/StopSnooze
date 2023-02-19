using System.ComponentModel;
using System.Diagnostics;

using StopSnooze.Core.Exceptions;

namespace StopSnooze.Core
{

    public class PowerTask : IPowerTaskSetSuccess, IPowerTaskClear, IPowerTask
    {
        private PowerTask()
        {
        }

        public EXECUTION_STATE? PreviousPowerState { get; private set; }
        public EXECUTION_STATE? PowerState { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <returns>IPowerTask interface</returns>
        public static IPowerTask Create()
        {
            return new PowerTask();
        }

        /// <summary>
        /// Changes execution state to prevent the system from sleeping
        /// </summary>
        /// <exception cref="StateChangeException">Thrown if the execution state could not be changed</exception>
        /// <returns><see cref="IPowerTaskSetSuccess"/></returns>
        public IPowerTaskSetSuccess Set(bool allowDisplaySleep)
        {
            EXECUTION_STATE newState = (EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
            if (allowDisplaySleep) 
            {
                newState = (EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
            }

            uint previousState = NativeMethods.SetThreadExecutionState(newState);
            if (previousState == 0)
            {
                // Failed to change execution state
                throw new StateChangeException(StateChangeFailure.SetStopSnoozeFailed.Message(), newState);
            }
            else
            {
                this.PowerState = newState;
            }
            this.PreviousPowerState = (EXECUTION_STATE)previousState;
            return this;
        }

        /// <summary>
        /// Changes execution state to allow the system to sleep
        /// </summary>
        /// <exception cref="StateChangeException">Thrown if the execution state could not be changed</exception>
        public void Clear()
        {
            uint previousState = NativeMethods.SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            if (previousState == 0)
            {
                // Failed to change execution state
                throw new StateChangeException(StateChangeFailure.SetStopSnoozeFailed.Message(), EXECUTION_STATE.ES_CONTINUOUS);
            }
            else
            {
                this.PowerState = EXECUTION_STATE.ES_CONTINUOUS;
            }
            this.PreviousPowerState = (EXECUTION_STATE)previousState;
        }

        /// <summary>
        /// Puts the thread to sleep for the specified amount time
        /// </summary>
        /// <param name="milliseconds">Time to wait in milliseconds</param>
        /// <returns><see cref="IPowerTaskClear"/></returns>
        public IPowerTaskClear Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processId">Process id (PID) to wait on</param>
        /// <param name="waitTime">Maximum time to wait in milliseconds. Omit to wait indefinitely for process exit.</param>
        /// <returns><see cref="IPowerTaskClear"/></returns>
        /// <exception cref="WaitException"></exception>
        public IPowerTaskClear WaitProcess(int processId, int waitTime = (int)ExitWaitTime.Indefinite)
        {
            try
            {
                using (Process waitProcess = Process.GetProcessById(processId))
                {
                    bool exited = waitProcess.WaitForExit(waitTime);
                    if (!exited)
                    {
                        throw new WaitException(WaitExitResult.Timeout, $"Process id {processId} did not exit before the specified wait time of {waitTime} milliseconds.");
                    }
                }
            }
            // Exception thrown by Process.GetProcessById
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                // Process not found
                throw new WaitException(WaitExitResult.ProcessNotFound, $"Process Id {processId} not found.", ex);
            }
            // Exception thrown by WaitForExit
            catch (Exception ex) when (ex is Win32Exception || ex is SystemException)
            {
                string message;
                if (ex is Win32Exception)
                    message = $"Could not wait for process id {processId} to exit - Win32Exception: {ex.Message}.";
                else
                    message = $"Could not wait for process id {processId} to exit.";

                throw new WaitException(WaitExitResult.Other, message, ex);
            }
            return this;
        }

    }
}
