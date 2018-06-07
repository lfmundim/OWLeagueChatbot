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
        Task<Message> GetCarousel(DivisionIds division, CancellationToken cancellationToken, Flow flow);
        Task<Message> GetCarousel(List<string> teams, CancellationToken cancellationToken);
        Task<Message> GetCarousel(NewsResponse news, CancellationToken cancellationToken);
        Task<Message> GetCarousel(RankingResponse standings);
    }


}
