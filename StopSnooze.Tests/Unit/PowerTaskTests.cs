using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using StopSnooze.Core;
using StopSnooze.Core.Exceptions;

namespace StopSnooze.Tests.Unit
{
    public class PowerTaskTests
    {
        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public void Set(bool allowDisplaySleep)
        {
            EXECUTION_STATE newState = (EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED);
            if (allowDisplaySleep)
                newState = (EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
            var powerTaskSetResult = PowerTask.Create().Set(allowDisplaySleep);
            IPowerTask? powerTask = powerTaskSetResult as IPowerTask;
            Assert.NotNull(powerTask);
            Assert.True(powerTask!.PreviousPowerState != 0);
            Assert.True(powerTask.PowerState == newState);
            NativeMethods.SetThreadExecutionState(Core.EXECUTION_STATE.ES_CONTINUOUS);
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public void Clear(bool allowDisplaySleep)
        {
            EXECUTION_STATE newState = (EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED);
            if (allowDisplaySleep)
                newState = (EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
            var powerTaskSetResult = PowerTask.Create().Set(allowDisplaySleep);
            IPowerTask? powerTask = powerTaskSetResult as IPowerTask;
            Assert.NotNull(powerTask);
            Assert.True(powerTask!.PowerState == newState);
            powerTaskSetResult.Clear();
            Assert.True(powerTask.PowerState == EXECUTION_STATE.ES_CONTINUOUS);
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public void WaitProcess_Success(bool allowDisplaySleep)
        {
            var powerTaskSetResult = PowerTask.Create().Set(allowDisplaySleep);
            IPowerTask? powerTask = powerTaskSetResult as IPowerTask;
            Assert.NotNull(powerTask);
            ProcessStartInfo testStartInfo = new ProcessStartInfo() { FileName = "timeout.exe", Arguments = "/t 20 /nobreak" };
            using (Process? testWaitProcess = Process.Start(testStartInfo))
            {
                Assert.NotNull(testWaitProcess);
                powerTaskSetResult.WaitProcess(testWaitProcess!.Id);
                Assert.True(testWaitProcess.HasExited);
            }
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public void WaitProcess_Notfound(bool allowDisplaySleep)
        {
            var powerTaskSetResult = PowerTask.Create().Set(allowDisplaySleep);
            IPowerTask? powerTask = powerTaskSetResult as IPowerTask;
            Assert.NotNull(powerTask);
            // Note this test relies on Windows Process IDs being a multiple of 4.
            // This may not be true of future Windows kernels
            var waitException = Assert.Throws<WaitException>(() => powerTaskSetResult.WaitProcess(1001));
        }

        [InlineData(true)]
        [InlineData(false)]
        [Theory]
        public void WaitProcess_TimeoutExceeded(bool allowDisplaySleep)
        {
            var powerTaskSetResult = PowerTask.Create().Set(allowDisplaySleep);
            IPowerTask? powerTask = powerTaskSetResult as IPowerTask;
            Assert.NotNull(powerTask);
            ProcessStartInfo testStartInfo = new ProcessStartInfo() { FileName = "timeout.exe", Arguments = "/t 20 /nobreak", LoadUserProfile = false };
            using (Process? testWaitProcess = Process.Start(testStartInfo))
            {
                Assert.NotNull(testWaitProcess);
                var waitException = Assert.Throws<WaitException>(() => powerTaskSetResult.WaitProcess(testWaitProcess!.Id, 1000));
            }

        }
    }
}
