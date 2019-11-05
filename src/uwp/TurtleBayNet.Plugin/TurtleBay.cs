﻿using System.Threading;
using System.Threading.Tasks;
using TurtleBayNet.Plugin.Model;
using TurtleBayNet.Plugin.Pages;
using WebExpress.Pages;
using WebExpress.Workers;
using Windows.Storage;

namespace TurtleBayNet.Plugin
{
    public class TurtleBay : WebExpress.Plugins.Plugin
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public TurtleBay()
        : base("TurtleBay")
        {
            Init(null);
        }

        /// <summary>
        /// Initialisierung des Prozesszustandes. Hier können z.B. verwaltete Ressourcen geladen werden. 
        /// </summary>
        /// <param name="configFileName">Der Dateiname der Konfiguration oder null</param>
        public override void Init(string configFileName)
        {
            ViewModel.Instance.Logging.Add(new LogItem(LogItem.LogLevel.Info, "TurtleBay initialisierung"));

            Register(new WorkerFile(new Path("", "Assets/.*"), ApplicationData.Current.LocalFolder.Path));

            var root = new VariationPath("dashboard", new PathItem("Zentrale"));
            var history = new VariationPath(root, "history", new PathItem("Verlauf", "history"));
            var ds = new VariationPath(root, "ds", new PathItem("DS18B201", "ds"));
            var settings = new VariationPath(root, "settings", new PathItem("Einstellungen", "settings"));
            var log = new VariationPath(root, "log", new PathItem("Logging", "log"));
            var info = new VariationPath(root, "info", new PathItem("info", "info"));
            var reset = new VariationPath(root, "reset", new PathItem("Reset", "reset"));
            var reboot = new VariationPath(root, "reboot", new PathItem("Reboot", "reboot"));
            var api = new VariationPath(root, "api", new PathItem("API", "api"));
            var debug = new VariationPath(root, "debug", new PathItem("Debug", "debug"));

            root.GetUrls("Zentrale").ForEach(x => Register(new WorkerPage<PageDashboard>(x) { }));
            history.GetUrls("Verlauf").ForEach(x => Register(new WorkerPage<PageHistory>(x) { }));
            ds.GetUrls("DS18B201").ForEach(x => Register(new WorkerPage<PageDS18B201>(x) { }));
            settings.GetUrls("Einstellungen").ForEach(x => Register(new WorkerPage<PageSettings>(x) { }));
            log.GetUrls("Logging").ForEach(x => Register(new WorkerPage<PageLog>(x) { }));
            info.GetUrls("Info").ForEach(x => Register(new WorkerPage<PageInfo>(x) { }));
            reset.GetUrls("Reset").ForEach(x => Register(new WorkerPage<PageReset>(x) { }));
            reboot.GetUrls("Reboot").ForEach(x => Register(new WorkerPage<PageReboot>(x) { }));
            api.GetUrls("API").ForEach(x => Register(new WorkerPage<PageApiBase>(x) { }));
            debug.GetUrls("Debug").ForEach(x => Register(new WorkerPage<PageDebug>(x) { }));

            Task.Run(() => { Run(); });
        }

        /// <summary>
        /// Diese Methode wird aufgerufen, nachdem das Fenster aktiv ist.
        /// </summary>
        private void Run()
        {
            // Loop
            while (true)
            {
                try
                {
                    Update();
                }
                finally
                {
                    Thread.Sleep(5000);
                }
            }
        }

        /// <summary>
        /// Diese Methode wird aufgerufen, nachdem das Fenster aktiv ist.
        /// </summary>
        private void Update()
        {
            ViewModel.Instance.UpdateAsync();
        }
    }
}