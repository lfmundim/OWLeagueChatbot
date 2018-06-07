using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using OWLeagueBot.Models;

namespace OWLeagueBot.Services
{
    public interface IChatbotFlowService
    {
        Task<bool> SendAgendaFlowAsync(UserContext context, Message message, CancellationToken cancellationToken);
        Task<bool> SendAlertFlowAsync(UserContext context, Message message, CancellationToken cancellationToken);
        Task<bool> SendNewsFlowAsync(Message message, CancellationToken cancellationToken);
        Task<bool> SendOnboardingFlowAsync(UserContext userContext, Message message, CancellationToken cancellationToken);
        Task<bool> SendSelectMainTeamFlowAsync(Message message, UserContext userContext, CancellationToken cancellationToken);
        Task<bool> SendStandingsFlowAsync(Message message, CancellationToken cancellationToken);
    }
}