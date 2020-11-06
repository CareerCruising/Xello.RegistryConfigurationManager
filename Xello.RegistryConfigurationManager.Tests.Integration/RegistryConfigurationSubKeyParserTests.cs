using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using System.Collections.Generic;

namespace Xello.RegistryConfigurationManager.Tests.Integration
{
    [TestClass]
    public class RegistryConfigurationSubKeyParserTests
    {
        private string _path = @"SOFTWARE\xellotest";


        [TestMethod]
        public void Can_Parse_String()
        {
            var editor = new RegistryEditor(Registry.LocalMachine, _path);

            var valueName = "Test:String";
            var expectedValue = "This is a test string";

            editor.SetValue(valueName, expectedValue);

            var Data = RegistryConfigurationSubKeyParser.Parse(_path);

            editor.Cleanup();

            Assert.IsTrue(Data.ContainsKey(valueName));

            Assert.AreEqual(expectedValue, Data[valueName]);
        }


        [TestMethod]
        public void Can_Parse_Array()
        {
            var editor = new RegistryEditor(Registry.LocalMachine, _path);

            var valueName = "Test:Array";
            var expectedValues = new string[] { "a", "b", "c", "d", "e" };

            editor.SetValue(valueName, expectedValues, RegistryValueKind.MultiString);

            var Data = RegistryConfigurationSubKeyParser.Parse(_path);

            editor.Cleanup();

            for (int i = 0; i < expectedValues.Length; ++i)
            {
                var valueIndex = valueName + ":" + i.ToString();
                Assert.IsTrue(Data.ContainsKey(valueIndex));
                Assert.AreEqual(expectedValues[i], Data[valueIndex]);
            }
        }
    }
}
