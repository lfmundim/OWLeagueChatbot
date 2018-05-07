using Lime.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Take.Blip.Client.Extensions.ArtificialIntelligence;
using Takenet.Iris.Messaging.Resources.ArtificialIntelligence;

namespace OWLeagueBot.Services
{
    public class NLPService : INLPService
    {
        private readonly IArtificialIntelligenceExtension _artificialIntelligence;

        public NLPService(IArtificialIntelligenceExtension artificialIntelligence)
        {
            _artificialIntelligence = artificialIntelligence;
        }
        public async Task ProcessAsync(Message message, CancellationToken cancellationToken)
        {
            var analysisRequest = new AnalysisRequest()
            {
                Text = message.Content.ToString()
            };
            var nlpResponse = await _artificialIntelligence.AnalyzeAsync(analysisRequest);
        }
    }
}
