using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TurtleBay.Plugin.Model;
using TurtleBay.Plugin.Pages;
using WebExpress.Pages;
using WebExpress.Workers;

namespace TurtleBay.Plugin
{
    public class TurtleBay : WebExpress.Plugins.Plugin
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public TurtleBay()
        : base("TurtleBay")
        {
        }

        /// <summary>
        /// Initialisierung des Prozesszustandes. Hier können z.B. verwaltete Ressourcen geladen werden. 
        /// </summary>
        /// <param name="configFileName">Der Dateiname der Konfiguration oder null</param>
        public override void Init(string configFileName = null)
        {
            ViewModel.Instance.Context = Context;
            ViewModel.Instance.Init();
            Context.Log.Info(MethodBase.GetCurrentMethod(), "TurtleBay initialisierung");

            Register(new WorkerFile(new Path(Context, "", "Assets/.*"), Context.AssetBaseFolder));

            var root = new VariationPath(Context, "dashboard", new PathItem("Zentrale"));
            var history = new VariationPath(root, "history", new PathItem("Verlauf", "history"));
            var ds = new VariationPath(root, "ds", new PathItem("DS18B201", "ds"));
            var settings = new VariationPath(root, "settings", new PathItem("Einstellungen", "settings"));
            var heating = new VariationPath(settings, "heating", new PathItem("Heizung", "heating"));
            var lighting = new VariationPath(settings, "lighting", new PathItem("Scheinwerfer", "lighting"));
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
            heating.GetUrls("Heizung").ForEach(x => Register(new WorkerPage<PageSettingsHeating>(x) { }));
            lighting.GetUrls("Scheinwerfer").ForEach(x => Register(new WorkerPage<PageSettingsLighting>(x) { }));
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
            ViewModel.Instance.Update();
        }
    }
}
