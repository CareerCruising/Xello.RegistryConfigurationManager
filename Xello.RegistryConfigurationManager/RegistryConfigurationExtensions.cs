using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;

namespace Xello.RegistryConfigurationManager
{
    public static class RegistryConfigurationExtensions
    {
        public static string DefaultSubKey { get; set; } = @"SOFTWARE\xello";

        public static IConfigurationBuilder AddRegistry(this IConfigurationBuilder builder)
        { 
            
            builder.AddRegistry(RegistryConfigurationExtensions.DefaultSubKey);
            return builder;

        }

        public static IConfigurationBuilder AddRegistry(this IConfigurationBuilder builder, string path)
        {
            builder.Add(new RegistryConfigurationSource(path));
            return builder;
        }
    }
}
