﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.Html;
using WebExpress.Messages;
using WebExpress.Workers;

namespace WebExpress.Pages
{
    public class Page : IPage
    {
        /// <summary>
        /// Liefert oder setzt die URL der Seite
        /// </summary>
        public string Url { get; protected set; }

        /// <summary>
        /// Liefert oder setzt den Pfad der Seite
        /// </summary>
        public Path Path { get; protected set; }

        /// <summary>
        /// Liefert oder setzt die Session
        /// </summary>
        public Session Session { get; private set; }

        /// <summary>
        /// Liefert oder setzt den Inhalt der Seite
        /// </summary>
        public Request Request { get; set; }

        /// <summary>
        /// Liefert oder setzt den Titel
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Liefert oder setzt das Favicon
        /// </summary>
        public List<Favicon> Favicons { get; set; }

        /// <summary>
        /// Liefert oder setzt das internes Stylesheet  
        /// </summary>
        public List<string> Styles { get; set; }

        /// <summary>
        /// Liefert oder setzt die Links auf die zu verwendenden JavaScript-Dateien, welche im Header eingefügt werden
        /// </summary>
        public List<string> HeaderScriptLinks { get; set; }

        /// <summary>
        /// Liefert oder setzt die Links auf die zu verwendenden JavaScript-Dateien
        /// </summary>
        public List<string> ScriptLinks { get; set; }

        /// <summary>
        /// Liefert oder setzt die Links auf die zu verwendenden JavaScript-Dateien, welche im Header eingefügt werden
        /// </summary>
        public List<string> HeaderScripts { get; set; }

        /// <summary>
        /// Liefert oder setzt die Links auf die zu verwendenden JavaScript-Dateien
        /// </summary>
        protected Dictionary<string, string> Scripts { get; private set; }

        /// <summary>
        /// Liefert oder setzt die Links auf die zu verwendenden Css-Dateien
        /// </summary>
        public List<string> CssLinks { get; set; }

        /// <summary>
        /// Liefert oder setzt die Metainformationen
        /// </summary>
        public List<KeyValuePair<string, string>> Meta { get; private set; }

        /// <summary>
        /// Liefert oder setzt die Parameter der Seite
        /// </summary>
        public Dictionary<string, Parameter> Params { get; private set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public Page()
        {
            Params = new Dictionary<string, Parameter>();

            Favicons = new List<Favicon>();
            Styles = new List<string>();
            HeaderScriptLinks = new List<string>();
            ScriptLinks = new List<string>();
            HeaderScripts = new List<string>();
            Scripts = new Dictionary<string, string>();
            CssLinks = new List<string>();

            Meta = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("charset", "UTF-8")
            };
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="title">Der Name der Seite</param>
        public Page(string title)
            : this()
        {
            Title = title;
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        /// <param name="path">Der Pfad</param>
        /// <param name="url">Die Url der Seite</param>
        /// <param name="session">Die aktuelle Session</param>
        public virtual void Init(Path path, string url, Session session)
        {
            Path = path;
            Url = url;
            Session = session;

            Init();
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public virtual void Process()
        {

        }

        /// <summary>
        /// Fügt ein Parameter hinzu. Der Wert wird aus dem Request ermittelt
        /// </summary>
        /// <param name="name">Der Name des Parametern</param>
        /// <param name="scope">Der Gültigkeitsbereich des Parameters</param>
        public void AddParam(string name, ParameterScope scope = ParameterScope.Global)
        {
            AddParam(name, Request.GetParam(name), scope);
        }

        /// <summary>
        /// Fügt ein Parameter hinzu.
        /// </summary>
        /// <param name="name">Der Name des Parametern</param>
        /// <param name="value">Der Wert</param>
        /// <param name="scope">Der Gültigkeitsbereich des Parameters</param>
        public void AddParam(string name, string value, ParameterScope scope = ParameterScope.Global)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                AddParam(new Parameter(name, value) { Scope = scope });
            }
        }

        /// <summary>
        /// Fügt ein Parameter hinzu. 
        /// </summary>
        /// <param name="param">Der Parameter</param>
        /// <param name="scope">Der Gültigkeitsbereich des Parameters</param>
        public void AddParam(Parameter param)
        {
            var key = param.Key.ToLower();

            if (param.Scope != ParameterScope.Session)
            {
                if (!Params.ContainsKey(key))
                {
                    Params.Add(key, param);
                }
                else
                {
                    Params[key] = param;
                }

                return;
            }

            // alternativ Parameter in Session speichern
            var session = Session.GetProperty<SessionPropertyParameter>();
            if (session != null && session.Params != null && session.Params.ContainsKey(key))
            {
                session.Params[key] = param;
            }
            else
            {
                session.Params[key] = param;
            }
        }

        /// <summary>
        /// Liefert ein Parameter anhand seines Namens
        /// </summary>
        /// <param name="name">Der Name des Parameters</param>
        /// <returns>Der Wert</returns>
        public string GetParam(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return string.Empty;
            }

            if (Params.ContainsKey(name.ToLower()))
            {
                return Params[name.ToLower()].Value;
            }

            if (Request.HasParam(name))
            {
                return Request.GetParam(name);
            }

            if (Session == null)
            {
                return string.Empty;
            }

            // alternativ Parameter aus der Session ermitteln
            var session = Session.GetProperty<SessionPropertyParameter>();
            if (session != null && session.Params != null && session.Params.ContainsKey(name.ToLower()))
            {
                return session.Params[name.ToLower()].Value;
            }

            return string.Empty;
        }

        /// <summary>
        /// Liefert ein Parameter anhand seines Namens
        /// </summary>
        /// <param name="name">Der Name des Parameters</param>
        /// <param name="defaultValue">Standardwert</param>
        /// <returns>Der Wert</returns>
        public int GetParam(string name, int defaultValue)
        {
            if (int.TryParse(GetParam(name), out var v))
            {
                return v;
            }

            return defaultValue;
        }

        /// <summary>
        /// Prüft, ob ein Parameter vorhanden ist
        /// </summary>
        /// <param name="name">Der Name des Parameters</param>
        /// <returns>true wenn Parameter vorhanden ist, false sonst</returns>
        public bool HasParam(string name)
        {
            if (Params.ContainsKey(name.ToLower()))
            {
                return true;
            }

            if (Request.HasParam(name))
            {
                return true;
            }

            if (Session == null)
            {
                return true;
            }

            // alternativ Parameter aus der Session ermitteln
            var session = Session.GetProperty<SessionPropertyParameter>();
            if (session != null && session.Params != null && session.Params.ContainsKey(name.ToLower()))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Liefert die URL der Seite
        /// </summary>
        /// <param name="extention">Die Url-Erweiterung</param>
        public string GetUrl(string extention)
        {
            return GetUrl(null, extention);
        }

        /// <summary>
        /// Liefert die URL der Seite
        /// </summary>
        /// <param name="index">N-te Teilstück der Url</param>
        /// <param name="extention">Die Url-Erweiterung</param>
        public string GetUrl(int? index = null, string extention = null)
        {
            var path = Path;

            if (index.HasValue && index < 0)
            {
                // Entfernt n Elemente aus dem Pfad
                path = new Path(Path.Items.Take(Path.Items.Count() + index.Value));
            }
            else if (index.HasValue && index > 0)
            {
                // liefert die ersten n Elemente aus dem Url-Pfad
                path = new Path(Path.Items.Take(index.Value));
            }
            else if (index.HasValue && index == 0)
            {
                return "/" + (extention ?? string.Empty);
            }

            if (string.IsNullOrWhiteSpace(extention))
            {
                return path.ToString();
            }

            if (path.Items.Count <= 1)
            {
                return "/" + extention != null ? extention : string.Empty;
            }

            return string.Join("/", path.ToString(), extention);
        }

        /// <summary>
        /// Liefert Findet eine Url anhand des Tag und gibt diese zurück
        /// </summary>
        /// <param name="tag">suche nach dem Tag</param>
        /// <param name="extention">Die Url-Erweiterung</param>
        /// <param name="reverseSearch">Suche Nach Tag in umgekehrter Reihenfolge</param>
        public string GetUrl(string tag, string extention, bool reverseSearch = true)
        {
            var index = 0;

            if (reverseSearch)
            {
                index = Path.Items.FindLastIndex(x => !string.IsNullOrWhiteSpace(x.Tag) && x.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase));

                return GetUrl(index + 1, extention);
            }

            index = Path.Items.FindIndex(x => !string.IsNullOrWhiteSpace(x.Tag) && x.Tag.Equals(tag, StringComparison.OrdinalIgnoreCase));

            return GetUrl(index + 1, extention);
        }

        /// <summary>
        /// Fügt eine Java-Script hinzu oder sersetzt dieses, falls vorhanden
        /// </summary>
        /// <param name="key">Der Schlüssel</param>
        /// <param name="code">Der Code</param>
        public virtual void AddScript(string key, string code)
        {
            var k = key.ToLower();
            if (Scripts.ContainsKey(k))
            {
                Scripts[k] = code;
            }
            else
            {
                Scripts.Add(k, code);
            }
        }

        /// <summary>
        /// Fügt eine Java-Script hinzu
        /// </summary>
        /// <param name="url">Der Link</param>
        public virtual void AddScriptLink(string url)
        {
            ScriptLinks.Add(url);
        }

        /// <summary>
        /// In String konvertieren
        /// </summary>
        /// <returns>Das Objekt als String</returns>
        public override string ToString()
        {
            return string.Empty;
        }

        /// <summary>
        /// Weiterleitung an eine andere Seite
        /// Die Funktion löst die RedirectException aus
        /// </summary>
        /// <param name="url">Die URL zu der weitergeleitet werden soll</param>
        public void Redirecting(string url)
        {
            throw new RedirectException(url);
        }
    }
}
