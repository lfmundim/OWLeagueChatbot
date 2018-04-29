using Lime.Protocol;
using OWLeagueBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OWLeagueBot.Services
{
    public interface IContextManager
    {
        Task SetUserContextAsync(Node from, UserContext value, CancellationToken cancellationToken);
        Task<UserContext> GetUserContextAsync(Node from, CancellationToken cancellationToken);
        string GetBucketKey(Node node);
    }
}
