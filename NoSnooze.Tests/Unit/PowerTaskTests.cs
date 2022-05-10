using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using NoSnooze.Core;
using NoSnooze.Core.Exceptions;

namespace NoSnooze.Tests.Unit
{
    public class PowerTaskTests
    {
        [Fact]
        public void Set()
        {
            var powerTaskSetResult = PowerTask.Create().Set();
            IPowerTask? powerTask = powerTaskSetResult as IPowerTask;
            Assert.NotNull(powerTask);
            Assert.True(powerTask!.PreviousPowerState != 0);
            Assert.True(powerTask.PowerState == (EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED));
            NativeMethods.SetThreadExecutionState(Core.EXECUTION_STATE.ES_CONTINUOUS);
        }

        [Fact]
        public void Clear()
        {
            var powerTaskSetResult = PowerTask.Create().Set();
            IPowerTask? powerTask = powerTaskSetResult as IPowerTask;
            Assert.NotNull(powerTask);
            Assert.True(powerTask!.PowerState == (EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_DISPLAY_REQUIRED));
            powerTaskSetResult.Clear();
            Assert.True(powerTask.PowerState == EXECUTION_STATE.ES_CONTINUOUS);
        }

        [Fact]
        public void WaitProcess_Success()
        {
            var powerTaskSetResult = PowerTask.Create().Set();
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

        [Fact]
        public void WaitProcess_Notfound()
        {
            var powerTaskSetResult = PowerTask.Create().Set();
            IPowerTask? powerTask = powerTaskSetResult as IPowerTask;
            Assert.NotNull(powerTask);
            // Note this test relies on Windows Process IDs being a multiple of 4.
            // This may not be true of future Windows kernels
            var waitException = Assert.Throws<WaitException>(() => powerTaskSetResult.WaitProcess(1001));
        }

        [Fact]
        public void WaitProcess_TimeoutExceeded()
        {
            var powerTaskSetResult = PowerTask.Create().Set();
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
