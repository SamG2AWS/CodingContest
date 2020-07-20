using Microsoft.Extensions.Configuration;
using Scenario.Models;
using System;

namespace Scenario.Configuration
{
    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration configuration;
        public AppSettings()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
        }

        public int FlightCapacity
        {
            get
            {
                var section = configuration.GetSection(nameof(FlightParameters));
                var flightcapacity = section[nameof(FlightParameters.FlightCapacity)];
                if (!string.IsNullOrEmpty(flightcapacity) && Int32.TryParse(flightcapacity, out int flCapacity))
                {
                    return flCapacity;
                }

                return 20;
            }
        }
    }
}
