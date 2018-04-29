using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OWLeagueBot.Services
{
    public interface ICarouselBuilder
    {
        Task SendOnboardingTeamCarouselAsync(Message message, CancellationToken cancellationToken);
    }
}
