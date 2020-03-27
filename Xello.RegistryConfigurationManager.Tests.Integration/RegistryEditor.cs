using Microsoft.Win32;
using System.Linq;

namespace Xello.RegistryConfigurationManager.Tests.Integration
{
    public class RegistryEditor
    {
        private readonly RegistryKey _hkey;
        private readonly string _subkey;

        public RegistryEditor()
        {
            _hkey = Registry.LocalMachine;
            _subkey = @"SOFTWARE\xello";
        }

        public RegistryEditor(RegistryKey hkey, string subkey)
        {
            _hkey = hkey;
            _subkey = subkey;
        }

        public string Subkey => _subkey;

        public void Cleanup()
        {
            _hkey.DeleteSubKeyTree(Subkey);
        }

        public string GetValue(string name)
        {
            var key = _hkey.CreateSubKey(Subkey);
            var results = key.GetValue(name, string.Empty).ToString();

            key.Close();

            return results;
        }
        public void SetValue(string name, string value)
        {
            var key = _hkey.CreateSubKey(Subkey);
            key.SetValue(name, value);            
            key.Close();            
        }
    }
}
