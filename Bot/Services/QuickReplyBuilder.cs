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
        public QuickReplyBuilder()
        {
        }
        public Message GetDivisionQuickReply(Flow flow, CancellationToken cancellationToken)
        {
            try
            {
                if (flow != Flow.Onboarding && flow != Flow.Alerts) throw new Exception();
                var prefix = GetPrefixFromFlow(flow);
                var menu = new Message()
                {
                    Content = new Select()
                    {
                        Text = flow == Flow.Onboarding ? "So, tell me, which division is the team you cheer for on?" : "Which division’s alerts do you wish to manage?",
                        Scope = SelectScope.Immediate,
                        Options = GetOptionsList(prefix)
                    }
                };
                return menu;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Message GetBackQuickReply()
        {
            var back = new Message()
            {
                Content = new Select()
                {
                    Text = "Go back:",
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
            return back;
        }

        public Message GetYesNoQuickReply()
        {
            var YesNo = new Message()
            {
                Content = new Select()
                {
                    Scope = SelectScope.Immediate,
                    Options = new SelectOption[]
                    {
                        new SelectOption
                        {
                            Text = PlainText.Parse("Yes"),
                            Value = PlainText.Parse("")
                        },
                        new SelectOption
                        {
                            Text = PlainText.Parse("No"),
                            Value = PlainText.Parse("")
                        }
                    }
                }
            };
            return YesNo;
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
                    return "#Alert_";
            }
        }
    }
}
