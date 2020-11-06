using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

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
            Data = RegistryConfigurationSubKeyParser.Parse(_path);
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
