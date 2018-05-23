using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Services
{
    public interface ICarouselBuilder
    {
    	Message GetMainMenuCarousel();
        Task<Message> GetOnboardingTeamCarouselAsync(DivisionIds division, CancellationToken cancellationToken);
    }

}
