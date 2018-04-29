using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OWLeagueBot.Services;
using Shouldly;
using System.Threading.Tasks;

namespace OWLeagueBot.Tests
{
    [TestFixture]
    public class OWLApiUnitTests
    {
        private IOWLApiService client;

        [OneTimeSetUp]
        public void Config()
        {
            client = GetClient();
        }
        [Test]
        public async Task GetNewsAsync()
        {
            // Arrange

            // Act
            var result = await client.GetNewsAsync();

            // Assert
            result.ShouldNotBeNull();
            result.Blogs.Length.ShouldBe(5);
        }

        [Test]
        public async Task GetRankingAsync()
        {
            // Arrange

            // Act
            var result = await client.GetRankingAsync();

            // Assert
            result.ShouldNotBeNull();
            result.Content.Length.ShouldBe(12);
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
        public async Task GetTeamAsync(int teamId)
        {
            // Arrange

            // Act
            var result = await client.GetTeamAsync(teamId);

            // Assert
            result.ShouldNotBeNull();
            result.AbbreviatedName.ShouldNotBeNull();
            result.Id.ShouldNotBeNull();
            result.Logo.ShouldNotBeNull();
            result.Name.ShouldNotBeNull();
            result.HomeLocation.ShouldNotBeNull();
        }

        [Test]
        public async Task GetTeamsAsync()
        {
            // Arrange

            // Act
            var result = await client.GetTeamsAsync();

            // Assert
            result.ShouldNotBeNull();
            result.Competitors.Length.ShouldBe(12);
        }

        private IOWLApiService GetClient()
        {
            return OWLApiFactory.Build("https://api.overwatchleague.com");
        }
    }
}
