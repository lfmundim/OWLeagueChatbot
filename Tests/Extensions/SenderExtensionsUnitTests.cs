using Lime.Protocol;
using NUnit.Framework;
using OWLeagueBot.Extensions;
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
        private Node node;
        private CancellationToken cancellationToken;

        [OneTimeSetUp]
        public void Config()
        {
            _sender = UnitTestBuilder.GetSender();
            node = UnitTestBuilder.GetUserNode();
            cancellationToken = UnitTestBuilder.GetCancellationToken();
        }
        [Test, Category("Short")]
        public async Task SendDelayedComposingAsync()
        {
            var result = await _sender.SendDelayedComposingAsync(node, 2000, cancellationToken);
            result.ShouldBe(true);
        }
    }
}
