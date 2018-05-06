using Lime.Protocol;
using Lime.Protocol.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using OWLeagueBot.Models;
using OWLeagueBot.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client;
using Take.Blip.Client.Activation;
using Take.Blip.Client.Extensions.Bucket;

namespace OWLeagueBot.Tests.Services
{
    [TestFixture]
    public class ContextManagerUnitTests
    {
        private IBucketExtension _bucket;
        private IContextManager _contextManager;
        private ISender _sender;
        private Node node;
        private CancellationToken cancellationToken;

        [OneTimeSetUp]
        public void Config()
        {
            var settings = UnitTestBuilder.GetSettings();
            cancellationToken = UnitTestBuilder.GetCancellationToken();
            _sender = UnitTestBuilder.GetSender();
            _bucket = new BucketExtension(_sender);
            _contextManager = new ContextManager(_bucket);
            node = UnitTestBuilder.GetUserNode();

            TypeUtil.RegisterDocument<UserContext>();
        }

        [Test, Category("Short")]
        public async Task GetBucketKey()
        {
            var key = _contextManager.GetBucketKey(node);
            key.ShouldNotBeNull();
            key.ShouldBe("testUser_UserContext");
        }

        [Test, Category("Short")]
        public async Task SetUserContextAsync()
        {
            try
            {
                var userContext = new UserContext()
                {
                    FirstInteraction = true,
                    MainTeam = "Dallas Fuel",
                    TeamDivision = "Pacific"
                };
                await _contextManager.SetUserContextAsync(node, userContext, cancellationToken);
            }
            catch (Exception ex)
            {
                ex.ShouldBeNull(); // returns failed test if any exception is thrown
            }
        }

        [Test, Category("Short")]
        public async Task GetUserContextAsync()
        {
            var context = await _contextManager.GetUserContextAsync(node, cancellationToken);
            context.FirstInteraction.ShouldNotBeNull();
            context.TeamDivision.ShouldNotBeNull();
            context.MainTeam.ShouldNotBeNull();
        }
    }
}
