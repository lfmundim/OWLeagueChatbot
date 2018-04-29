using Lime.Messaging.Contents;
using Lime.Protocol;
using OWLeagueBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client;
using static OWLeagueBot.Models.Enumerations;

namespace OWLeagueBot.Services
{
    public class QuickReplyBuilder : IQuickReplyBuilder
    {
        private readonly ISender _sender;

        public QuickReplyBuilder(ISender sender)
        {
            _sender = sender;
        }
        public async Task SendDivisionQuickReplyAsync(Message message, Flow flow, CancellationToken cancellationToken)
        {
            var prefix = GetPrefixFromFlow(flow);
            var menu = new Message()
            {
                To = message.From,
                Content = new Select()
                {
                    Text = flow == Flow.Onboarding ? "So, tell me, which division is the team you cheer for on?" : "Which division’s alerts do you wish to manage?",
                    Scope = SelectScope.Immediate,
                    Options = GetOptionsList(prefix)
                }
            };
            await _sender.SendMessageAsync(menu, cancellationToken);
        }
        public async Task SendBackQuickReplyAsync(Message message, CancellationToken cancellationToken)
        {
            var back = new Message()
            {
                To = message.From,
                Content = new Select()
                {
                    Scope = SelectScope.Immediate,
                    Options = new SelectOption[]
                    {
                        new SelectOption
                        {
                            Text = PlainText.Parse("🔙"),
                            Value = PlainText.Parse("#MainMenu_")
                        }
                    }
                }
            };
            await _sender.SendMessageAsync(back, cancellationToken);
        }

        private static SelectOption[] GetOptionsList(string prefix)
        {
            return new SelectOption[]
            {
                new SelectOption()
                {
                    Text = "Atlantic",
                    Value = $"{prefix}79"
                },
                new SelectOption()
                {
                    Text = "Pacific",
                    Value = $"{prefix}80"
                },
                new SelectOption()
                {
                    Text = "None",
                    Value = $"{prefix}0"
                }
            };
        }

        private string GetPrefixFromFlow(Flow flow)
        {
            switch (flow)
            {
                case Flow.Onboarding:
                    return "#Onboarding_";
                case Flow.Alerts:
                default:
                    return "#Alerts_";
            }
        }
    }
}
