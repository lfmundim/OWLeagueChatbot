using Lime.Protocol;
using System.Threading;
using System.Threading.Tasks;

namespace OWLeagueBot.Services
{
    public interface INLPService
    {
        Task ProcessAsync(Message message, CancellationToken cancellationToken);
    }
}