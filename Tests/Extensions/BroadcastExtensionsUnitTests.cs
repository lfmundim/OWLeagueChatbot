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
using Take.Blip.Client.Extensions.Broadcast;

namespace OWLeagueBot.Tests.Extensions
{
    [TestFixture]
    public class BroadcastExtensionsUnitTests
    {
        private IBroadcastExtension broadcastExtension;
        private Node node;
        private CancellationToken cancellationToken;

        [OneTimeSetUp]
        public void Config()
        {
            var sender = UnitTestBuilder.GetSender();
            broadcastExtension = new BroadcastExtension(sender);
            node = UnitTestBuilder.GetUserNode();
            cancellationToken = UnitTestBuilder.GetCancellationToken();
        }
        [Test, Category("Short")]
        public async Task UpdateDistributionListAsync()
        {
            var result = await broadcastExtension.UpdateDistributionListAsync("unitTests", node.ToIdentity(), cancellationToken);
            result.ShouldBe(true);
        }
    }
}
