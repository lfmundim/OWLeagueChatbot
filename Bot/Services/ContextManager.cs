using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lime.Protocol;
using OWLeagueBot.Models;
using Take.Blip.Client.Extensions.Bucket;

namespace OWLeagueBot.Services
{
    public class ContextManager : IContextManager
    {
        private const string UserContext = nameof(UserContext);

        private readonly IBucketExtension _bucket;

        public ContextManager(IBucketExtension bucket)
        {
            _bucket = bucket;
        }
        public string GetBucketKey(Node from) => $"{from.Name}_{UserContext}";

        public async Task<UserContext> GetUserContextAsync(Node from, CancellationToken cancellationToken)
        {
            try
            {
                var context = await _bucket.GetAsync<UserContext>(GetBucketKey(from), cancellationToken);

                if (context == null)
                {
                    context = new UserContext();
                    await _bucket.SetAsync(GetBucketKey(from), context, cancellationToken: cancellationToken);
                }

                return context;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task SetUserContextAsync(Node from, UserContext value, CancellationToken cancellationToken)
        {
            return _bucket.SetAsync(GetBucketKey(from), value, cancellationToken: cancellationToken);
        }
    }
}
