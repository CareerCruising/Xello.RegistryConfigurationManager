using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("Xello.RegistryConfigurationManager.Tests.Integration")]
namespace Xello.RegistryConfigurationManager
{

    internal class RegistryConfigurationSubKeyParser
    {
        private RegistryConfigurationSubKeyParser() { }

        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private readonly Stack<string> _context = new Stack<string>();
        private string _currentPath;

        public static IDictionary<string, string> Parse(string keyPath)
            => new RegistryConfigurationSubKeyParser().ParseKeyPath(keyPath);

        private IDictionary<string, string> ParseKeyPath(string keyPath)
        {
            _data.Clear();

            VisitSubKey(keyPath);

            return _data;
        }

        private void VisitSubKey(string keyPath)
        {
            using (RegistryKey subKey = Registry.LocalMachine.OpenSubKey(keyPath))
            {
                if (subKey != null)
                {
                    foreach (var keyName in subKey.GetSubKeyNames())
                    {
                        EnterContext(keyName);
                        VisitSubKey(keyPath + "\\" + keyName);
                        ExitContext();
                    }

                    foreach (var valueName in subKey.GetValueNames())
                    {
                        EnterContext(valueName);
                        VisitValue(subKey, valueName);
                        ExitContext();
                    }
                }
            }
        }

        private void VisitValue(RegistryKey subKey, string valueName)
        {
            switch (subKey.GetValueKind(valueName))
            {
                case RegistryValueKind.MultiString:
                    VisitMultiStringValue(subKey, valueName);
                    break;

                case RegistryValueKind.String:
                    VisitStringValue(subKey, valueName);
                    break;

                default:
                    throw new FormatException($"Unsupported {nameof(RegistryValueKind)}");
            }
        }

        private void VisitMultiStringValue(RegistryKey subKey, string valueName)
        {
            if (subKey.GetValue(valueName) is string[] array)
            {
                for (int index = 0; index < array.Length; index++)
                {
                    EnterContext(index.ToString());

                    string key = _currentPath;
                    if (_data.ContainsKey(key))
                    {
                        throw new FormatException($"Key Already Exists in Data ({key})");
                    }
                    _data[key] = array[index];

                    ExitContext();
                }
            }
        }

        private void VisitStringValue(RegistryKey subKey, string valueName)
        {
            string key = _currentPath;
            string value = subKey.GetValue(valueName).ToString();

            if (_data.ContainsKey(key))
            {
                throw new FormatException($"Key Already Exists in Data ({key})");
            }

            _data[key] = value.ToString(CultureInfo.InvariantCulture);
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }
}
