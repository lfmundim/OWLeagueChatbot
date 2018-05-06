using Lime.Messaging.Contents;
using Lime.Protocol;
using NUnit.Framework;
using OWLeagueBot.Models;
using OWLeagueBot.Models.Responses;
using OWLeagueBot.Services;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static OWLeagueBot.Models.Enumerations;
using OWLeagueBot.Services;

namespace OWLeagueBot.Tests.Services
{
    [TestFixture]
    public class CarouselBuilderUnitTests
    {
        private OWLFilter _service;
        public ICarouselBuilder _carouselBuilder;

        [OneTimeSetUp]
        public void Config()
        {
            _service = new OWLFilter(GetClient());
            _carouselBuilder = new CarouselBuilder(_service);
        }
        private IOWLApiService GetClient()
        {
            return OWLApiFactory.Build("https://api.overwatchleague.com");
        }

        [Test, Category("Short")]
        [TestCase(DivisionIds.AtlanticDivision)]
        [TestCase(DivisionIds.PacificDivision)]
        public async Task BuildOnboardingTeamCarouselAsync(DivisionIds division)
        {
            var carousel = await _carouselBuilder.GetOnboardingTeamCarouselAsync(division, CancellationToken.None);
            carousel.Content.ShouldNotBeNull();
        }
    }
}
