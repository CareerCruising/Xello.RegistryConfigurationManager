using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Xello.RegistryConfigurationManager.Tests.Integration
{
    [TestClass]
    public class RegistryConfigurationSubKeyParserTests
    {

        [TestMethod]
        public void Can_Parse_String()
        {
            var Data = RegistryConfigurationSubKeyParser.Parse(@"SOFTWARE\xellotest");

            var valueName = "Test:String";
            var expectedValue = "This is a test string";
            Assert.IsTrue(Data.ContainsKey(valueName));

            Assert.AreEqual(expectedValue, Data[valueName]);
        }


        [TestMethod]
        public void Can_Parse_Array()
        {
            var Data = RegistryConfigurationSubKeyParser.Parse(@"SOFTWARE\xellotest");

            var valueName = "Test:Array";
            var expectedValues = new List<string> { "a", "b", "c", "d", "e" };

            for (int i = 0; i < expectedValues.Count; ++i)
            {
                var valueIndex = valueName + ":" + i.ToString();
                Assert.IsTrue(Data.ContainsKey(valueIndex));
                Assert.AreEqual(expectedValues[i], Data[valueIndex]);
            }
        }
    }
}
