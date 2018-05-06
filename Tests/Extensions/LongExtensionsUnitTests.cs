using NUnit.Framework;
using OWLeagueBot.Extensions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OWLeagueBot.Tests.Extensions
{
    [TestFixture]
    public class LongExtensionsUnitTests
    {
        [OneTimeSetUp]
        public void Config()
        {
        }
        [Test, Category("Short")]
        [TestCase(null)]
        [TestCase(1525627351485)]
        public async Task ConvertLongIntoDateTime(long? ticks)
        {
            var result = ticks.ConvertLongIntoDateTime();
            result.ShouldNotBeNull();
        }

        [Test, Category("Short")]
        [TestCase(1525627351485)]
        public async Task ConvertLongIntoDateTime(long ticks)
        {
            var result = ticks.ConvertLongIntoDateTime();
            result.ShouldNotBeNull();
        }
    }
}
