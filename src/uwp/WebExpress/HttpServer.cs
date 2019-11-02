﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebExpress.Config;
using WebExpress.Messages;
using WebExpress.Plugins;

namespace WebExpress
{
    /// <summary>
    /// siehe RFC 2616
    /// </summary>
    public class HttpServer : IHost
    {
        private const int ConnectionLimit = 100;

        /// <summary>
        /// Setzt oder Liefert den 
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Setzt oder liefert den Listener
        /// </summary>
        private TcpListener Listener { get; set; }

        /// <summary>
        /// Warteschlange für noch nicht verarbeitete Anfragen
        /// </summary>
        private Queue<TcpClient> Queue { get; set; }

        /// <summary>
        /// Threadbeendigung
        /// </summary>
        private readonly CancellationTokenSource serverTokenSource = new CancellationTokenSource();
        private readonly CancellationTokenSource clientTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Liefert oder setzt die Liste der Plugins
        /// </summary>
        public List<IPlugin> Plugins { get; private set; }

        /// <summary>
        /// Liefert oder setzt die Konfiguration
        /// </summary>
        public HttpServerConfig Config { get; set; }

        /// <summary>
        /// Liefert oder setzt den Kontext
        /// </summary>
        public IPluginContext Context { get; protected set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="port">Der Port auf dem der Server höhren soll</param>
        public HttpServer(int port)
        {
            Port = port;
            Queue = new Queue<TcpClient>();
            Plugins = new List<IPlugin>();

            Context = new HttpServerContext(Environment.CurrentDirectory, Path.Combine(Environment.CurrentDirectory, "Config"));
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="port">Der Port auf dem der Server höhren soll</param>
        /// <param name="context">Der Serverkontext</param>
        public HttpServer(int port, HttpServerContext context)
            : this(port)
        {
            Context = context;
        }

        /// <summary>
        /// Registriert einen Worker 
        /// </summary>
        /// <param name="plugin"></param>
        public void RegisterPlugin(IPlugin plugin)
        {
            Plugins.Add(plugin);
            plugin.Host = this;

            //if (Context != null && Context.Log != null)
            //{
            //    Context.Log.Info(MethodInfo.GetCurrentMethod(), "Plugin '" + plugin.Name + "' wurde registriert");
            //}
        }

        /// <summary>
        /// Startet den HTTPServer
        /// </summary>
        public void Start()
        {
            //if (Context != null && Context.Log != null)
            //{
            //    Context.Log.Info(MethodInfo.GetCurrentMethod(), "Starte HttpServer auf Port " + Port);
            //}

            if (Config != null)
            {
                LoadPlugins(Config.StageDirectory);
            }
            else
            {
                //LoadPlugins(Environment.CurrentDirectory);
            }

            Listener = new TcpListener(IPAddress.Any, Port);
            Listener.Start();

            var task = Task.Factory.StartNew(() =>
            {
                Run();
            }, serverTokenSource.Token);

        }

        /// <summary>
        /// Beginnt auf dem Port zu lauschen
        /// Wird nebenläufig ausgeführt
        /// </summary>
        private void Run()
        {
            // Client-Task
            var clientTask = Task.Factory.StartNew(() =>
            {
                while (!clientTokenSource.IsCancellationRequested)
                {
                    TcpClient client = null;

                    lock (Queue)
                    {
                        client = Queue.Count > 0 ? Queue.Dequeue() : null;
                    }

                    if (client != null)
                    {
                        // Worker-Task
                        var workerTask = Task.Factory.StartNew(() =>
                        {
                            HandleClient(client);
                        });
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
            }, clientTokenSource.Token);

            // Server-Task
            while (!serverTokenSource.IsCancellationRequested)
            {

                if (Listener == null)
                {
                    break;
                }

                try
                {
                    var client = Listener.AcceptTcpClient();

                    lock (Queue)
                    {
                        if (Queue.Count < ConnectionLimit)
                        {
                            Queue.Enqueue(client);
                        }
                        else
                        {
                            client.Close();
                        }
                    }
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Startet den HTTPServer
        /// </summary>
        public void Stop()
        {
            // Laufende Threads beenden
            clientTokenSource.Cancel();
            serverTokenSource.Cancel();

            lock (Queue)
            {
                Listener.Stop();
                Listener = null;
            }
        }

        /// <summary>
        /// Behandelt einen eingehenden Anforderung
        /// Wird nebenläufig ausgeführt
        /// </summary>
        /// <param name="client"></param>
        private void HandleClient(TcpClient client)
        {
            var ip = client.Client.RemoteEndPoint;

            //if (Context != null && Context.Log != null)
            //{
            //    Context.Log.Info(MethodInfo.GetCurrentMethod(), ip + ": Client wurde verbunden");
            //}

            using (var reader = new StreamReader(client.GetStream()))
            {
                var requestStr = "";

                try
                {

                    while (!reader.EndOfStream && reader.Peek() != 13)
                    {
                        requestStr += reader.ReadLine() + "\n";
                    }
                }
                catch (Exception)
                {
                    //Context.Log.Error(MethodBase.GetCurrentMethod(), ip + ": " + ex.Message);
                }

                Request request = null;
                Response response = new ResponseNotFound();

                try
                {
                    //Context.Log.Debug(MethodBase.GetCurrentMethod(), ip + ": Anfrage '" + requestStr + "'");

                    if (!string.IsNullOrWhiteSpace(requestStr))
                    {
                        request = Request.Parse(requestStr, ip.ToString());
                        if (request.HeaderFields.ContentLength > 0 && request.Method == RequestMethod.POST)
                        {
                            var buf = new char[request.HeaderFields.ContentLength];

                            reader.ReadLine();
                            reader.Read(buf, 0, buf.Count());

                            var param = new string(buf).Replace('+', ' ');

                            foreach (var v in param.Split('&'))
                            {
                                var s = v.Split('=');
                                request.AddParam(s[0], !string.IsNullOrWhiteSpace(s[1]) ? s[1] : string.Empty);
                            }
                        }

                        foreach (var p in Plugins)
                        {
                            response = p.PreProcess(request);
                            if (response == null)
                            {
                                response = p.Process(request);
                                response = p.PostProcess(request, response);
                            }

                            if (request != null && response != null && !(response is ResponseNotFound))
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        //Context.Log.Debug(MethodBase.GetCurrentMethod(), ip + ": Unerwartete Anfrage '" + requestStr + "'");
                        response = new ResponseInternalServerError();
                    }
                }
                catch (Exception)
                {
                    //if (Context != null && Context.Log != null)
                    //{
                    //    Context.Log.Exception(MethodInfo.GetCurrentMethod(), ex);
                    //}
                    response = new ResponseInternalServerError();
                }

                try
                {
                    using (var writer = new StreamWriter(client.GetStream()))
                    {
                        writer.Write(response.GetHeader());
                        writer.Flush();

                        if (response.Content is byte[])
                        {
                            using (var bw = new BinaryWriter(writer.BaseStream))
                            {
                                bw.Write((byte[])response.Content);
                            }
                        }
                        else
                        {
                            writer.Write(response.Content ?? "");
                        }

                        writer.Flush();
                    }
                }
                catch (Exception)
                {
                    //Context.Log.Error(MethodBase.GetCurrentMethod(), ip + ": " + ex.Message);
                }
            }

            client.Close();

            //Context.Log.Info(MethodInfo.GetCurrentMethod(), ip + ": Anfrage wurde bearbeitet. Status=" + response.Status);
        }

        /// <summary>
        /// Lädt und registriert die Plugins
        /// </summary>
        /// <param name="path">Das Verzeichnis, indem sich die Plugins befinden</param>
        private void LoadPlugins(string path)
        {
            foreach (var plugin in Directory.EnumerateFiles(path, "*.dll"))
            {
                var factory = GetFactoryObjectFromAssembly(plugin);

                if (factory != null)
                {
                    var p = factory.Create(this, null);
                    Plugins.Add(p);
                }
            }
        }

        /// <summary>
        /// Lädt die angegebene Assembly und gibt das darin enthaltene Factory-Objekt zurück
        /// </summary>
        /// <param name="assembly">Der Pfad und Dateiname, des zu ladenen Plugin</param>
        private IPluginFactory GetFactoryObjectFromAssembly(string assembly)
        {
            try
            {
                var lib = Assembly.LoadFrom(assembly);
                foreach (var type in lib.GetExportedTypes())
                {
                    if (type.IsClass && type.IsPublic && type.GetInterface("IPluginFactory") != null)
                    {
                        var result = (IPluginFactory)lib.CreateInstance(type.FullName);

                        //if (Context != null && Context.Log != null)
                        //{
                        //    Context.Log.Info(MethodBase.GetCurrentMethod(), string.Format("'{0}.dll' wurde geladen. Version='{1}'", plugin.Assembly, lib.GetName().Version.ToString()));
                        //}

                        return result;
                    }
                }
            }
            catch
            {

            }

            return null;
        }
    }
}
