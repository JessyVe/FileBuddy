using System.IO;
using System.Reflection;
using System.Xml;

namespace SharedRessources.Services
{
    /// <summary>
    /// Loads a log4net configuration with the given
    /// name from the given assembly.
    /// </summary>
    public static class LoggingConfigurationLoader
    {
        public static void LoadLoggingConfiguration(Assembly assembly, string configurationName = "log4net.config")
        {
            var log4netConfig = new XmlDocument();
            log4netConfig.Load(File.OpenRead(configurationName));
            var repo = log4net.LogManager.CreateRepository(assembly,
                       typeof(log4net.Repository.Hierarchy.Hierarchy));

            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
        }
    }
}
