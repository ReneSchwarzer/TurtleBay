﻿using WebExpress.Messages;
using WebExpress.Pages;
using WebExpress.Plugins;

namespace WebExpress.Workers
{
    /// <summary>
    /// Arbeitet eine Anfrage ab. Dies erfolgt nebenläufig
    /// </summary>
    public abstract class Worker : IWorker
    {
        /// <summary>
        /// Liefert oder setzt die URL, auf dem der Worker reagiert
        /// </summary>
        public Path Path { get; protected set; }

        /// <summary>
        /// Liefert oder setzt den Kontext des Plugins
        /// </summary>
        public IPluginContext HostContext { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="path">Die URL</param>
        public Worker(Path path)
        {
            Path = path;
        }

        /// <summary>
        /// Vorverarbeitung
        /// </summary>
        /// <param name="request">Die Anfrage</param>
        /// <returns>Die Antwort aus der Vorverarbeitung oder null</returns>
        public virtual Response PreProcess(Request request)
        {
            return null;
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        /// <param name="request">Die Anfrage</param>
        /// <returns>Die Antwort</returns>
        public abstract Response Process(Request request);

        /// <summary>
        /// Vorverarbeitung
        /// </summary>
        /// <param name="request">Die Anfrage</param>
        /// <param name="response">Die Antwort</param>
        /// <returns>Die Antwort</returns>
        public virtual Response PostProcess(Request request, Response response)
        {
            return response;
        }
    }
}
