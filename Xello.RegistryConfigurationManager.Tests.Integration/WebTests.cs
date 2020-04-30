using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Features;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Xello.RegistryConfigurationManager.Tests.Integration
{
    [TestClass]
    public class WebTests
    {
        public WebTests()
        {

        }

        [TestCleanup]
        public void Cleanup()
        {
            KvpStub.Key = null;
            KvpStub.RegValue = null;

        }

        [TestMethod]
        public void Can_Configure_From_Registry()
        {
            var value = "test_value";
            var key = RegistryConfigurationExtensions.DefaultSubKey;
            var name = "test_name";

            Assert.IsTrue(Test(key, name, value, true));

        }

        [TestMethod]
        public void Prefers_Registry_With_Default_Webhost()
        {
            var value = "test_value";
            var key = RegistryConfigurationExtensions.DefaultSubKey;
            var name = "test_name";
            
            Assert.IsTrue(Test(key, name, value, true));
        }

        [TestMethod]
        public void Can_Still_Configure_From_Appsetting_With_Default_Webhost()
        {
            var name = "test_appsettings_name";//from appsettings.json
            var key = RegistryConfigurationExtensions.DefaultSubKey;
            var value = "test_appsetting_value";
            
            Assert.IsTrue(Test(key, name, value));
        }

        public bool Test(string key, string name, string value, bool register)
        {
            if (!register)
            {
                Test(key, name, value);
            }

            var regedit = new RegistryEditor(Registry.LocalMachine, key);
            regedit.SetValue(name, value);
            
            var ret = Test(key, name, value);

            regedit.Cleanup();

            return ret;
        }

        public bool Test(string key, string name, string value)
        {   
            KvpStub.Key = name;

            var web = WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) => config.AddRegistry(key))
                .UseStartup<StartupRegistry>()
                .Build();

            web.Start();

            var ret = String.Equals(value, KvpStub.RegValue);

            web.Dispose();
            
            return ret;
        }
    }
    
    public class KvpStub
    {
        public static string RegValue { get; set; }

        public static string Key { get; set; }

        public KvpStub(string value)
        {
            RegValue = value;
        }
    }
    public class StartupRegistry : IStartup
    {
        public IConfiguration Configuration { get; }

        public StartupRegistry(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
        }
        public void Configure(IApplicationBuilder app)
        {
               
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(c =>
            {
                //set the value to test against
                return new KvpStub(Configuration[KvpStub.Key]);
            });

            
            var provider = services.BuildServiceProvider();

            //get the service to trigger the test
            var stub = provider.GetService<KvpStub>();

            return provider;
        }
    }
}
