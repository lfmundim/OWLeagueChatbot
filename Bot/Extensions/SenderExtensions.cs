using Lime.Messaging.Contents;
using Lime.Protocol;
using OWLeagueBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client;

namespace OWLeagueBot.Extensions
{
    public static class SenderExtensions
    {
        public static async Task<bool> SendDelayedComposingAsync(this ISender sender, Node destination, int millisecondsDelay, CancellationToken cancellationToken)
        {
            try
            {
                await sender.SendMessageAsync(
                    new Message
                    {
                        Id = null,
                        To = destination,
                        Content = new ChatState { State = ChatStateEvent.Composing }
                    },
                    cancellationToken);
                await Task.Delay(millisecondsDelay);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static async Task<bool> SendBackQuickReplyAsync(this ISender sender, Node destination, IQuickReplyBuilder quickReplyBuilder, CancellationToken cancellationToken)
        {
            try
            {
                var backMessage = quickReplyBuilder.GetBackQuickReply();
                backMessage.To = destination;
                await sender.SendMessageAsync(backMessage, cancellationToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
