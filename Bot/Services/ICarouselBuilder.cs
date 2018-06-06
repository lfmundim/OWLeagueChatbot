using Lime.Protocol;
using OWLeagueBot.Models.Responses;
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
        Task<Message> GetAgendaCarouselAsync(List<string> teams, CancellationToken cancellationToken);
        Task<Message> GetAlertTeamCarouselAsync(DivisionIds division, CancellationToken cancellationToken);
        Task<Message> GetNewsCarouselAsync(NewsResponse news, CancellationToken cancellationToken);
        Task<Message> GetStandingsCarousel(RankingResponse standings);
    }


}
