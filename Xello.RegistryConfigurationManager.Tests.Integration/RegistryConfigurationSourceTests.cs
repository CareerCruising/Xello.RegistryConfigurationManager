using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;

namespace Xello.RegistryConfigurationManager.Tests.Integration
{
    [TestClass]
    public class RegistryProviderTests
    {
        private RegistryEditor _editor;
        private string _path = @"SOFTWARE\xellotest";
        
        public RegistryProviderTests()
        {
            
            
        }
        
        [TestCleanup]
        public void CleanUp()
        {

        }

        [TestMethod]
        public void Can_Read_From_Registry()
        {
            var editor = new RegistryEditor(Registry.LocalMachine, _path);
            var name = "read_test";
            var value = "lorum ipsum";

            editor.SetValue(name, value);

            var provider = new RegistryConfigurationProvider(_path);
            provider.Load();

            var result = string.Empty;
            provider.TryGet(name, out result);

            Assert.AreEqual(value, result);

            editor.Cleanup();
        }

        [TestMethod]
        public void Can_Set_Registry()
        {
            var editor = new RegistryEditor(Registry.LocalMachine, _path);
            var name = "write_test";
            var value = "lorum ipsum";

            var provider = new RegistryConfigurationProvider(_path);
            provider.Load();
            provider.Set(name, value);

            var result = editor.GetValue(name);
            Assert.AreEqual(value, result);

            editor.Cleanup();
        }
    }
}
