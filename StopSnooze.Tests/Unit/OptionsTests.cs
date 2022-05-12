using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using StopSnooze.Runner;

namespace StopSnooze.Tests.Unit
{
    public class OptionsTests
    {
        [Fact]
        public void Validate()
        {
            IOptions testOptions = new Options();
            // Nothing set
            bool validateResult = testOptions.Validates();
            Assert.True(validateResult);
            Random rnd = new Random();
            // Only valid PID values
            for (int loop = 0; loop < 1024; loop++)
            {
                testOptions.PID = rnd.Next(0, Int32.MaxValue);
                validateResult = testOptions.Validates();
                Assert.True(validateResult);
            }
            // Only valid wait times
            testOptions = new Options();
            for (int loop = 0; loop < 1024; loop++)
            {
                testOptions.MaxWaitTime = rnd.Next(1, 2147483);
                validateResult = testOptions.Validates();
                Assert.True(validateResult);
            }
            // Both valid
            testOptions = new Options();
            for (int loop = 0; loop < 1024; loop++)
            {
                testOptions.PID = rnd.Next(0, Int32.MaxValue);
                testOptions.MaxWaitTime = rnd.Next(1, 2147483);
                validateResult = testOptions.Validates();
                Assert.True(validateResult);
            }

        }

        [Fact]
        public void Validate_InvalidPID()
        {
            IOptions testOptions = new Options();
            bool validateResult;
            Random rnd = new Random();
            // Only invalid PID values
            for (int loop = 0; loop < 1024; loop++)
            {
                testOptions.PID = rnd.Next(Int32.MinValue, -1);
                validateResult = testOptions.Validates();
                Assert.False(validateResult);
            }

        }

        [InlineData(Int32.MinValue, 0)] // Lower range of invalid
        [InlineData(2147484, Int32.MaxValue)] // Upper range of invalid
        [Theory]
        public void Validate_InvalidMaxWaitTime(int lowerBound, int upperBound)
        {
            IOptions testOptions = new Options();
            bool validateResult;
            Random rnd = new Random();
            // Invalid MaxWaitTime value bounds set in params
            for (int loop = 0; loop < 1024; loop++)
            {
                testOptions.MaxWaitTime = rnd.Next(lowerBound, upperBound);
                validateResult = testOptions.Validates();
                Assert.False(validateResult);
            }

        }

    }
}
