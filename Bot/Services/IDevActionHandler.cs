using Lime.Protocol;
using System.Threading;
using System.Threading.Tasks;

namespace OWLeagueBot.Services
{
    public interface IDevActionHandler
    {
        Task<bool> HandleAsync(Message message, CancellationToken cancellationToken);
    }
}