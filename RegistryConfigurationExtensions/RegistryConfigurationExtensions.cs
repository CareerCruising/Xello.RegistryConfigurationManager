using System;
using Microsoft.Extensions.Configuration;

namespace RegistryConfigurationExtensions
{
    public static class RegistryConfigurationExtensions
    {
        public static IConfigurationBuilder AddEnvironmentVariables(this IConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Add(new EnvironmentVariablesConfigurationSource());
            return configurationBuilder;
        }

    }
}
