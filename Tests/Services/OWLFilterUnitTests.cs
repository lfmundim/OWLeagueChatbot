using NUnit.Framework;
using OWLeagueBot.Models;
using OWLeagueBot.Services;
using Shouldly;
using System.Runtime;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using static OWLeagueBot.Models.Enumerations;
using OWLeagueBot.Models.Requests;
using System.Linq;
using OWLeagueBot.Models.Responses;
using System.Collections;
using OWLeagueBot.Extensions;
using System;

namespace OWLeagueBot.Tests.Services
{
    [TestFixture]
    public class OWLFilterUnitTests
    {
        private OWLFilter _service;

        [OneTimeSetUp]
        public void Config()
        {
            _service = new OWLFilter(UnitTestBuilder.GetClient());
        }

        [Test, Category("Long")]
        [TestCase(TeamIds.BostonUprising)]
        [TestCase(TeamIds.DallasFuel)]
        [TestCase(TeamIds.FloridaMayhem)]
        [TestCase(TeamIds.HoustonOutlaws)]
        [TestCase(TeamIds.LondonSpitfire)]
        [TestCase(TeamIds.LosAngelesGladiators)]
        [TestCase(TeamIds.LosAngelesValiant)]
        [TestCase(TeamIds.NewYorkExcelsior)]
        [TestCase(TeamIds.PhiladelphiaFusion)]
        [TestCase(TeamIds.SanFranciscoShock)]
        [TestCase(TeamIds.SeoulDynasty)]
        [TestCase(TeamIds.ShanghaiDragons)]
        public async Task GetFutureMatchesAsync(TeamIds teamId)
        {
            // Arrange

            // Act
            var result = await _service.GetFutureMatchesAsync(teamId);

            // Assert
            result.ShouldNotBeNull();
        }

        [Test, Category("Long")]
        [TestCase(TeamIds.BostonUprising)]
        [TestCase(TeamIds.DallasFuel)]
        [TestCase(TeamIds.FloridaMayhem)]
        [TestCase(TeamIds.HoustonOutlaws)]
        [TestCase(TeamIds.LondonSpitfire)]
        [TestCase(TeamIds.LosAngelesGladiators)]
        [TestCase(TeamIds.LosAngelesValiant)]
        [TestCase(TeamIds.NewYorkExcelsior)]
        [TestCase(TeamIds.PhiladelphiaFusion)]
        [TestCase(TeamIds.SanFranciscoShock)]
        [TestCase(TeamIds.SeoulDynasty)]
        [TestCase(TeamIds.ShanghaiDragons)]
        public async Task GetLastMatchupAsync(TeamIds mainTeamId)
        {
            foreach (TeamIds team in MyConstants.AllTeams)
            {
                // Arrange 
                var query = new MatchupRequest()
                {
                    firstTeamId = (int)mainTeamId,
                    secondTeamId = (int)team
                };

                // Act
                var result = await _service.GetLastMatchupAsync((TeamIds)query.firstTeamId, (TeamIds)query.secondTeamId);

                // Assert
                result.ShouldNotBeNull();
            }
        }

        [Test, Category("Long")]
        [TestCase(TeamIds.BostonUprising)]
        [TestCase(TeamIds.DallasFuel)]
        [TestCase(TeamIds.FloridaMayhem)]
        [TestCase(TeamIds.HoustonOutlaws)]
        [TestCase(TeamIds.LondonSpitfire)]
        [TestCase(TeamIds.LosAngelesGladiators)]
        [TestCase(TeamIds.LosAngelesValiant)]
        [TestCase(TeamIds.NewYorkExcelsior)]
        [TestCase(TeamIds.PhiladelphiaFusion)]
        [TestCase(TeamIds.SanFranciscoShock)]
        [TestCase(TeamIds.SeoulDynasty)]
        [TestCase(TeamIds.ShanghaiDragons)]
        public async Task GetNextMatchAsync(TeamIds teamId)
        {
            //Arrange
            var query = new MatchRequest()
            {
                teamId = (int)teamId
            };

            // Act
            var result = await _service.GetNextMatchAsync((TeamIds)query.teamId);

            //Assert
            result.ShouldNotBeNull();
            result.Competitors.Length.ShouldBe(2);
            result.Id.ShouldNotBeNull();
            result.Games.Length.ShouldBeGreaterThan(3);
            result.State.ShouldBe("PENDING");
            result.Winner.ShouldBeNull();
            result.ConclusionStrategy.ShouldNotBeNull();
            result.ConclusionStrategy.ShouldBeOfType(typeof(string));
        }

        [Test, Category("Short")]
        public async Task GetNewsAsync()
        {
            // Act
            var result = await _service.GetNewsAsync();

            //Assert
            result.ShouldNotBeNull();
            result.Blogs.Length.ShouldBe(5);
            foreach (Blog b in result.Blogs)
            {
                b.Status.ShouldBe("live");
                b.Summary.ShouldNotBeNull();
                b.Author.ShouldNotBeNull();
                var publishDate = b.Publish.ConvertLongIntoDateTime();
                DateTime.UtcNow.ShouldBeGreaterThanOrEqualTo(publishDate);
                b.BlogId.ShouldNotBeNull();
                b.Title.ShouldNotBeNull();
                b.Thumbnail.Url.ShouldNotBeNull();
            }
        }

        [Test, Category("Short")]
        public async Task GetRankingAsync()
        {
            // Act
            var result = await _service.GetRankingAsync();

            //Assert
            result.ShouldNotBeNull();
            result.Content.Length.ShouldBe(12);
            for (int i = 0; i < result.Content.Length; i++)
            {
                result.Content[i].Placement.ShouldBe(i + 1);
            }
        }

        [Test, Category("Short")]
        [TestCase("79")]
        [TestCase("80")]
        public async Task GetTeamsByDivisionAsync(string division)
        {
            // Arrange
            var divisionId = GetDivisionFromText(division);

            // Act
            var result = await _service.GetTeamsByDivisionAsync(divisionId);

            //Assert
            result.ShouldNotBeNull();
            result.Length.ShouldBe(6);
        }
    }
}
