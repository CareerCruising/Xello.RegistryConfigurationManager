using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xello.RegistryConfigurationManager
{
    public class RegistryConfigurationProvider : ConfigurationProvider
    {
        private readonly string _path;

        public RegistryConfigurationProvider(string path)
        {   
            _path = path;
        }
        
        public override void Load()
        {
            var registry = Registry
                            .LocalMachine
                            .CreateSubKey(_path); //will create if not exists. If we decide to error on non-exist, use OpenSubKey instead

            var keys = registry.GetValueNames();

            Data = keys.ToDictionary(key=> key, key => registry.GetValue(key).ToString());

            registry.Close();
        }

        public override void Set(string name, string value)
        {
            var key = Registry
                .LocalMachine
                .CreateSubKey(_path);

            key.SetValue(name, value);

            key.Close();

            base.Set(name, value);
        }
    }
}
