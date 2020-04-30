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
            using (RegistryKey regKey = Registry.LocalMachine.OpenSubKey(_path))
            {
                if (regKey != null)
                {
                    var keys = regKey.GetValueNames();
                    Data = keys.ToDictionary(key => key, key => regKey.GetValue(key).ToString());
                }
            }
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
