using NUnit.Framework;
using OWLeagueBot.Models;
using OWLeagueBot.Services;
using Shouldly;
using System;
using System.Threading.Tasks;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Tests
{
    [TestFixture]
    public class OWLFilterUnitTests
    {
        private OWLFilter _service;

        [OneTimeSetUp]
        public void Config()
        {
            _service = new OWLFilter(GetClient());
        }

        private IOWLApiService GetClient()
        {
            return OWLApiFactory.Build("https://api.overwatchleague.com");
        }

        [Test]
        [TestCase(4523)]
        [TestCase(4524)]
        [TestCase(4525)]
        [TestCase(4402)]
        [TestCase(4403)]
        [TestCase(4404)]
        [TestCase(4405)]
        [TestCase(4406)]
        [TestCase(4407)]
        [TestCase(4408)]
        [TestCase(4409)]
        [TestCase(4410)]
        public async Task GetFutureMatchesAsync(TeamIds teamId)
        {
            // Arrange

            // Act
            var result = await _service.GetFutureMatchesAsync(teamId);

            // Assert
            result.ShouldNotBeNull();
        }

        [Test]
        [TestCase(4523)]
        [TestCase(4524)]
        [TestCase(4525)]
        [TestCase(4402)]
        [TestCase(4403)]
        [TestCase(4404)]
        [TestCase(4405)]
        [TestCase(4406)]
        [TestCase(4407)]
        [TestCase(4408)]
        [TestCase(4409)]
        [TestCase(4410)]
        public async Task GetLastMatchupAsync(TeamIds mainTeamId)
        {
            foreach(TeamIds team in MyConstants.AllTeams)
            {
                // Act
                var result = await _service.GetLastMatchupAsync(mainTeamId, team);

                // Assert
                result.ShouldNotBeNull();
            }
        }

        [Test]
        [TestCase(4523)]
        [TestCase(4524)]
        [TestCase(4525)]
        [TestCase(4402)]
        [TestCase(4403)]
        [TestCase(4404)]
        [TestCase(4405)]
        [TestCase(4406)]
        [TestCase(4407)]
        [TestCase(4408)]
        [TestCase(4409)]
        [TestCase(4410)]
        public async Task GetNextMatchAsync(TeamIds teamId)
        {
            // Act
            var result = await _service.GetNextMatchAsync(teamId);

            //Assert
            result.ShouldNotBeNull();
        }

        [Test]
        public async Task GetNewsAsync()
        {
            // Act
            var result = await _service.GetNewsAsync();

            //Assert
            result.ShouldNotBeNull();
            result.Blogs.Length.ShouldBe(5);
        }

        [Test]
        public async Task GetRankingAsync()
        {
            // Act
            var result = await _service.GetRankingAsync();

            //Assert
            result.ShouldNotBeNull();
            result.Content.Length.ShouldBe(12);
        }

        [Test]
        [TestCase(79)]
        [TestCase(80)]
        public async Task GetTeamsByDivisionAsync(DivisionIds divisionId)
        {
            // Act
            var result = await _service.GetTeamsByDivisionAsync(divisionId);

            //Assert
            result.ShouldNotBeNull();
            result.Length.ShouldBe(6);
        }
    }
}
