using TurtleBayNet.Plugin;
using WebExpress;
using WebExpress.Plugins;

namespace TurtleBayNet
{
    public class TurtleBayFactory : PluginFactory
    {
        /// <summary>
        /// Erstellt eine neue Instanz eines Prozesszustandes
        /// </summary>
        /// <param name="host">Der Benutzer</param>
        /// <param name="configFileName">Der Dateiname der Konfiguration oder null</param>
        /// <returns>Die Instanz des Prozesszustandes</returns>
        public override IPlugin Create(IHost host, string configFileName)
        {
            return Create<TurtleBay>(host, configFileName);
        }
    }
}
