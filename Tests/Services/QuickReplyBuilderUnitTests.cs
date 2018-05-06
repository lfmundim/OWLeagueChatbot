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

namespace OWLeagueBot.Tests.Services
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

        [Test, Category("Short")]
        [TestCase(Flow.Onboarding)]
        [TestCase(Flow.Alerts)]
        public async Task GetGetDivisionQuickReplyAsync(Flow flow)
        {
            var menu = _quickReplyBuilder.GetDivisionQuickReply(flow, CancellationToken.None);
            menu.ShouldNotBeNull();
            menu.Content.ShouldBeOfType(typeof(Select));
        }

        [Test, Category("Short")]
        public async Task GetBackQuickReplyAsync()
        {
            var back = _quickReplyBuilder.GetBackQuickReply(CancellationToken.None);
            back.ShouldNotBeNull();
            back.Content.ShouldBeOfType(typeof(Select));
        }
    }
}
