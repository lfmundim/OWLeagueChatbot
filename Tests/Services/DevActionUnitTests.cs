using Lime.Messaging.Contents;
using Lime.Protocol;
using NUnit.Framework;
using OWLeagueBot.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Take.Blip.Client.Extensions.Bucket;
using Take.Blip.Client.Extensions.Scheduler;

namespace OWLeagueBot.Tests.Services
{
    [TestFixture]
    public class DevActionUnitTests
    {
        IDevActionHandler devActionHandler;

        [OneTimeSetUp]
        public void Config()
        {
            var sender = UnitTestBuilder.GetSender();
            var filter = new OWLFilter(UnitTestBuilder.GetClient());
            var bucket = new BucketExtension(sender);
            var scheduler = new SchedulerExtension(sender);
            var contextManager = new ContextManager(bucket);
            var settings = new OWLeagueBot.Settings();
            devActionHandler = new DevActionHandler(contextManager, bucket, scheduler, filter, settings, sender);
        }

        [Test]
        [TestCase("DEVDELETEUSER", "TestUser", true)]
        [TestCase("DEVREFRESHMESSAGES", "1700444659974924", false)]
        public async Task HandleAsync(string action, string identity, bool expectedResult)
        {
            var message = new Message()
            {
                Content = PlainText.Parse(action),
                From = new Node(identity, "UnitTests.io", "")
            };
            var result = await devActionHandler.HandleAsync(message, UnitTestBuilder.GetCancellationToken());
            result.ShouldBe(expectedResult);
        }
    }
}
