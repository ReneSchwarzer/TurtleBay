﻿namespace WebExpress.Plugins
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
        string AssetBaseFolder { get; }

        /// <summary>
        /// Liefert das Log, zum schreiben von Statusnachrichten auf die Konsole und in eine Log-Datei
        /// </summary>
        Log Log { get; }
    }
}
