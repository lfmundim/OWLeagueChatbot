using OWLeagueBot.Services;
using RestEase;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Text;
using Take.Blip.Client.Activation;

namespace OWLeagueBot
{
    public class ServiceProvider : Container, IServiceContainer
    {
        public ServiceProvider()
        {
            // Register them singletons here!
            RegisterSingleton<IOWLFilter, OWLFilter>();
            RegisterSingleton<ICarouselBuilder, CarouselBuilder>();
            RegisterSingleton<IContextManager, ContextManager>();
            RegisterSingleton<IQuickReplyBuilder, QuickReplyBuilder>();
        }
        public void RegisterService(Type serviceType, object instance)
        {
            RegisterSingleton(serviceType, instance);

            if (serviceType == typeof(Settings))
            {
                var settings = (Settings)instance;
                RegisterSingleton(() => RestClient.For<IOWLApiService>(settings.OWLEndpoint));
            }
        }

        public void RegisterService(Type serviceType, Func<object> instanceFactory)
        {
            RegisterSingleton(serviceType, instanceFactory);
        }
    }
}
