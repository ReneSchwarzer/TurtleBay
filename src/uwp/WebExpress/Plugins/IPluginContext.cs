namespace WebExpress.Plugins
{
    /// <summary>
    /// Der Kontext
    /// </summary>
    public interface IPluginContext
    {
        /// <summary>
        /// Liefert das Konfigurationserzeichnis
        /// </summary>
        string ConfigBaseFolder { get; }

        /// <summary>
        /// Liefert das Dokumentenverzeichnis
        /// </summary>
        string AssertBaseFolder { get; }
    }
}
