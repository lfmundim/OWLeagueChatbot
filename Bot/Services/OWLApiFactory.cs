using RestEase;
using System;
using System.Collections.Generic;
using System.Text;

namespace OWLeagueBot.Services
{
    public static class OWLApiFactory
    {
        public static IOWLApiService Build(string baseUrl)
        {
            return new RestClient(baseUrl).For<IOWLApiService>();
        }
    }
}
