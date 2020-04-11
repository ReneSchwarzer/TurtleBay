using TurtleBay.Plugin;
using WebExpress;
using WebExpress.Plugins;

namespace TurtleBayNet
{
    public class TurtleBayFactory : PluginFactory
    {
        /// <summary>
        /// Liefert den Dateinamen der Konfigurationsdatei
        /// </summary>
        public override string ConfigFileName => "turtlebay.config.xml";

        /// <summary>
        /// Erstellt eine neue Instanz eines Prozesszustandes
        /// </summary>
        /// <param name="context">Der Benutzer</param>
        /// <param name="configFileName">Der Dateiname der Konfiguration oder null</param>
        /// <returns>Die Instanz des Prozesszustandes</returns>
        public override IPlugin Create(HttpServerContext context, string configFileName)
        {
            var plugin = Create<TurtleBay.Plugin.TurtleBay>(context, configFileName);
            return plugin;
        }
    }
}
