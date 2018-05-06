using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client.Extensions.Broadcast;

namespace OWLeagueBot.Extensions
{
    public static class BroadcastExtensions
    {
        public static async Task<bool> UpdateDistributionListAsync(this IBroadcastExtension broadcast, string teamListName, Identity user, CancellationToken cancellationToken)
        {
            try
            {
                if (teamListName.IsNullOrWhiteSpace()) throw new Exception();
                await broadcast.CreateDistributionListAsync(teamListName, cancellationToken);
                await broadcast.AddRecipientAsync(teamListName, user, cancellationToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
