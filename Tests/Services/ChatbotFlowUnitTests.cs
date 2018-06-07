using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lime.Messaging.Contents;
using Lime.Protocol;
using Lime.Protocol.Serialization;
using NUnit.Framework;
using OWLeagueBot.Models;
using OWLeagueBot.Services;
using Shouldly;

namespace OWLeagueBot.Tests.Services
{
    [TestFixture]
    public class ChatbotFlowUnitTests
    {
        private IChatbotFlowService flowService;
        private IContextManager contextManager;

        [OneTimeSetUp]
        public void Config()
        {
            flowService = UnitTestBuilder.GetFlowService();
            contextManager = UnitTestBuilder.GetContextManager();
            TypeUtil.RegisterDocument<UserContext>();
        }

        [Test]
        public async Task StandingsReceiverUnitTest()
        {
            var message = new Message
            {
                From = UnitTestBuilder.GetUserNode(),
                To = UnitTestBuilder.GetBotNode(),
                Content = PlainText.Parse("UnitTests")
            };
            bool success = false;
            try
            {
                success = await flowService.SendStandingsFlowAsync(message, CancellationToken.None);
            }
            catch(Exception ex)
            {
                ex.ShouldBeNull();
            }
            finally
            {
                success.ShouldBeTrue();
            }
        }

        [Test]
        public async Task NewsReceiverUnitTest()
        {
            var message = new Message
            {
                From = UnitTestBuilder.GetUserNode(),
                To = UnitTestBuilder.GetBotNode(),
                Content = PlainText.Parse("UnitTests")
            };
            bool success = false;
            try
            {
                success = await flowService.SendNewsFlowAsync(message, CancellationToken.None);
            }
            catch(Exception ex)
            {
                ex.ShouldBeNull();
            }
            finally
            {
                success.ShouldBeTrue();
            }
        }

        [Test]
        public async Task AgendaReceiverUnitTest()
        {
            var message = new Message
            {
                From = UnitTestBuilder.GetUserNode(),
                To = UnitTestBuilder.GetBotNode(),
                Content = PlainText.Parse("UnitTests")
            };
            
            var context = new UserContext
            {
                AlertTeams = new List<string>()
                {
                    {"DAL"},
                    {"BOS"},
                    {"HOU"},
                    {"NYE"},
                    {"SHD"},
                    {"VAL"},
                    {"GLA"},
                    {"LON"},
                    {"FLA"},
                    {"SEO"},
                    {"PHI"},
                    {"SFS"},
                }
            };

            bool success = false;
            try
            {
                success = await flowService.SendAgendaFlowAsync(context, message, CancellationToken.None);
            }
            catch(Exception ex)
            {
                ex.ShouldBeNull();
            }
            finally
            {
                success.ShouldBeTrue();
            }
        }

        [Test]
        [TestCase("NotDefault", "")]
        [TestCase("SelectMainTeam_", "DAL")]
        public async Task SelectMainTeamReceiverUnitTest(string text, string teamTag)
        {
            var message = new Message
            {
                From = UnitTestBuilder.GetUserNode(),
                To = UnitTestBuilder.GetBotNode(),
                Content = PlainText.Parse(text+teamTag)
            };
            var context = new UserContext
            {
                MainTeam = ""
            };

            bool success = false;
            try
            {
                success = await flowService.SendSelectMainTeamFlowAsync(message, context, CancellationToken.None);
                context = await contextManager.GetUserContextAsync(message.From, CancellationToken.None);
            }
            catch(Exception ex)
            {
                ex.ShouldBeNull();
            }
            finally
            {
                success.ShouldBeTrue();
                context.MainTeam.ShouldBe(teamTag);
            }
        }

        [Test] 
        [TestCase(true, "")]
        [TestCase(false, "79")]
        [TestCase(false, "80")]
        public async Task OnboardingReceiverUnitTest(bool isFirst, string teamDivision)
        {
            var message = new Message
            {
                From = UnitTestBuilder.GetUserNode(),
                To = UnitTestBuilder.GetBotNode(),
                Content = PlainText.Parse("UnitTest_"+teamDivision)
            };
            var userContext = new UserContext
            {
                FirstInteraction = isFirst
            };
            bool success = false;
            try
            {
                success = await flowService.SendOnboardingFlowAsync(userContext, message, CancellationToken.None);
                userContext = await contextManager.GetUserContextAsync(message.From, CancellationToken.None);
            }
            catch(Exception ex)
            {
                ex.ShouldBeNull();
            }
            finally
            {
                success.ShouldBeTrue();
                if(!isFirst)
                    userContext.TeamDivision.ShouldBe(teamDivision);
            }
        }

        [Test] // TODO
        [TestCase("Main", "")]
        [TestCase("Add_", "DAL")]
        [TestCase("Remove_", "DAL")]
        [TestCase("Division_79", "")]
        [TestCase("Division_80", "")]
        public async Task AlertReceiverUnitTest(string flow, string teamTag)
        {
            var message = new Message
            {
                From = UnitTestBuilder.GetUserNode(),
                To = UnitTestBuilder.GetBotNode(),
                Content = PlainText.Parse(flow)
            };
            var team = flow.Contains("Remove") ? teamTag : string.Empty;
            var userContext = new UserContext
            {
                AlertTeams = new List<string>
                {
                    { team }
                }
            };
            bool success = false;
            try
            {
                success = await flowService.SendAlertFlowAsync(userContext, message, CancellationToken.None);
                userContext = await contextManager.GetUserContextAsync(message.From, CancellationToken.None);
            }
            catch(Exception ex)
            {
                ex.ShouldBeNull();
            }
            finally
            {
                success.ShouldBeTrue();
                if(flow.Contains("Remove"))
                    userContext.AlertTeams.ShouldNotContain(teamTag);
                else if(flow.Contains("Add"))
                    userContext.AlertTeams.ShouldContain(teamTag);
            }
        }
    }
}