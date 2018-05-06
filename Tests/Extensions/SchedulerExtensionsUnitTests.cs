using Lime.Protocol;
using NUnit.Framework;
using OWLeagueBot.Extensions;
using OWLeagueBot.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client.Extensions.Scheduler;

namespace OWLeagueBot.Tests.Extensions
{
    [TestFixture]
    public class SchedulerExtensionsUnitTests
    {
        private ISchedulerExtension scheduler;
        private IOWLFilter filter;
        private Node node;
        private CancellationToken cancellationToken;

        [OneTimeSetUp]
        public void Config()
        {
            var client = UnitTestBuilder.GetClient();
            filter = new OWLFilter(client);
            var sender = UnitTestBuilder.GetSender();
            scheduler = new SchedulerExtension(sender);
            node = UnitTestBuilder.GetUserNode();
            cancellationToken = UnitTestBuilder.GetCancellationToken();
        }
        [Test, Category("Long")]
        public async Task UpdateBroadcastMessagesAsync()
        {
            var settings = new OWLeagueBot.Settings
            {
                BroadcastDomain = "@broadcast.msging.net",
                BroadcastMessageIdPrefix = "Match-Id-",
                OWLLogo = "https://icdn2.digitaltrends.com/image/overwatch-league-720x720.jpg",
                OWLTwitch = "https://www.twitch.tv/overwatchleague"
            };
            var result = await scheduler.UpdateBroadcastMessagesAsync(filter, settings);
            result.ShouldBe(true);
        }
    }
}
