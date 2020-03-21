using Microsoft.Win32;
using System.Linq;

namespace Xello.RegistryConfigurationManager.Tests.Integration
{
    public class RegistryEditor
    {
        private readonly RegistryKey _hkey;
        private readonly string _subkey;
        
        public RegistryEditor(RegistryKey hkey, string subkey)
        {
            _hkey = hkey;
            _subkey = subkey;
        }

        public void Cleanup()
        {
            _hkey.DeleteSubKeyTree(_subkey);
        }

        public string GetValue(string name)
        {
            var key = _hkey.CreateSubKey(_subkey);
            var results = key.GetValue(name, string.Empty).ToString();

            key.Close();

            return results;
        }
        public void SetValue(string name, string value)
        {
            var key = _hkey.CreateSubKey(_subkey);
            key.SetValue(name, value);            
            key.Close();            
        }
    }
}
