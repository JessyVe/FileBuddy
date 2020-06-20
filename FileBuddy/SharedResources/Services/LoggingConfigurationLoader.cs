using System.IO;
using System.Reflection;
using System.Xml;

namespace SharedRessources.Services
{
    public static class LoggingConfigurationLoader
    {
        public static void LoadLoggingConfiguration(Assembly assembly)
        {
            var log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead("log4net.config"));
            var repo = log4net.LogManager.CreateRepository(assembly,
                       typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }
    }
}
