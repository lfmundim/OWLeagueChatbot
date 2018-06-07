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
using Take.Blip.Client;

namespace OWLeagueBot.Tests.Extensions
{
    [TestFixture]
    public class SenderExtensionsUnitTests
    {
        private ISender _sender;
        private QuickReplyBuilder _quickReplyBuilder;
        private Node node;
        private CancellationToken cancellationToken;

        [OneTimeSetUp]
        public void Config()
        {
            _sender = UnitTestBuilder.GetSender();
            _quickReplyBuilder = new QuickReplyBuilder();
            node = UnitTestBuilder.GetUserNode();
            cancellationToken = UnitTestBuilder.GetCancellationToken();
        }
        [Test, Category("Short")]
        public async Task SendDelayedComposingAsync()
        {
            var result = await _sender.SendDelayedComposingAsync(node, 2000, cancellationToken);
            result.ShouldBe(true);
        }
        [Test, Category("Short")]
        public async Task SendBackQuickReplyAsync()
        {
            var result = await _sender.SendBackQuickReplyAsync(node, _quickReplyBuilder, cancellationToken);
            result.ShouldBe(true);
        }
    }
}
