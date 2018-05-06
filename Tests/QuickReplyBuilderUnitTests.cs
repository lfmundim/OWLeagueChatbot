using Lime.Messaging.Contents;
using NUnit.Framework;
using OWLeagueBot.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Tests
{
    [TestFixture]
    public class QuickReplyBuilderUnitTests
    {
        private OWLFilter _service;
        public IQuickReplyBuilder _quickReplyBuilder;

        [OneTimeSetUp]
        public void Config()
        {
            _service = new OWLFilter(GetClient());
            _quickReplyBuilder = new QuickReplyBuilder();
        }
        private IOWLApiService GetClient()
        {
            return OWLApiFactory.Build("https://api.overwatchleague.com");
        }

        [Test]
        [TestCase(Flow.Onboarding)]
        [TestCase(Flow.Alerts)]
        public async Task GetGetDivisionQuickReplyAsync(Flow flow)
        {
            var menu = await _quickReplyBuilder.GetDivisionQuickReplyAsync(flow, CancellationToken.None);
            menu.ShouldNotBeNull();
            menu.Content.ShouldBeOfType(typeof(Select));
        }

        [Test]
        public async Task GetBackQuickReplyAsync()
        {
            var back = await _quickReplyBuilder.GetBackQuickReplyAsync(CancellationToken.None);
            back.ShouldNotBeNull();
            back.Content.ShouldBeOfType(typeof(Select));
        }
    }
}
