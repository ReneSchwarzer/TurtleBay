﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using WebExpress.Messages;
using WebExpress.Workers;

namespace WebExpress.Plugins
{
    /// <summary>
    /// Repräsentiert ein Plugin
    /// </summary>
    public class Plugin : IPlugin
    {
        /// <summary>
        /// Der zum Plugin zugehörige Host
        /// </summary>
        public IHost Host { get; set; }

        /// <summary>
        /// Liefert den Namen der PipeStage
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Liefert oder setzt die Liste der Worker
        /// </summary>
        public Dictionary<string, IWorker> Workers { get; private set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Plugin()
        {
            Workers = new Dictionary<string, IWorker>();
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="name">Name des Prozesszustandes</param>
        public Plugin(string name)
            : this()
        {
            Name = name;
        }

        /// <summary>
        /// Initialisierung des Prozesszustandes. Hier können z.B. verwaltete Ressourcen geladen werden. 
        /// </summary>
        /// <param name="configFileName">Der Dateiname der Konfiguration oder null</param>
        public virtual void Init(string configFileName)
        {
        }

        /// <summary>
        /// Registriert einen Worker 
        /// </summary>
        /// <param name="worker"></param>
        public void Register(IWorker worker)
        {
            worker.HostContext = Host?.Context;

            var key = Regex.Replace(worker.Path.ToString(), @"\$[0-9A-Za-z]+", "([0-9A-Za-z.]*)");

            if (!Workers.ContainsKey(key))
            {
                Workers.Add(key, worker);
            }
            else
            {
                Workers[key] = worker;
            }
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
        public virtual Response Process(Request request)
        {
            if (Workers == null)
            {
                // Kein Worker gefunden => 404 Not Found
                return new ResponseNotFound();
            }

            foreach (var w in Workers)
            {
                if (Regex.IsMatch(request.URL, "^" + w.Key + "$", RegexOptions.IgnoreCase))
                {
                    var response = w.Value.PreProcess(request);
                    if (response == null)
                    {
                        response = w.Value.Process(request);
                        response = w.Value.PostProcess(request, response);
                    }

                    return response;
                }
            }

            // Kein Worker gefunden => 404 Not Found
            return new ResponseNotFound();
        }

        /// <summary>
        /// Nachverarbeitung
        /// </summary>
        /// <param name="request">Die Anfrage</param>
        /// <param name="response">Die Antwort</param>
        /// <returns>Die Antwort</returns>
        public virtual Response PostProcess(Request request, Response response)
        {
            return response;
        }

        /// <summary>
        /// Freigeben von nicht verwalteten Ressourcen, welche bei der Initialisierung reserviert wurden.
        /// </summary>
        /// <param name="data">Die Eingabedaten</param>
        public virtual void Dispose()
        {
        }
    }
}
