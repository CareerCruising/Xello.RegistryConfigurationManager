

using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace Xello.RegistryConfigurationManager
{
    public class RegistryConfigurationSource : IConfigurationSource
    {
        public string _path;
        
        private RegistryConfigurationSource()
        {

        }
        public RegistryConfigurationSource(string path)
        {
            _path = path;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RegistryConfigurationProvider(_path);
        }
    }
}
