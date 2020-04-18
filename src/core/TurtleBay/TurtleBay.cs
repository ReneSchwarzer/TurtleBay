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
        : base("TurtleBay", "/Asserts/img/Turtle.svg")
        {
        }

        /// <summary>
        /// Initialisierung des Prozesszustandes. Hier können z.B. verwaltete Ressourcen geladen werden. 
        /// </summary>
        /// <param name="configFileName">Der Dateiname der Konfiguration oder null</param>
        public override void Init(string configFileName = null)
        {
            base.Init(configFileName);

            ViewModel.Instance.Context = Context;
            ViewModel.Instance.Init();
            Context.Log.Info(MethodBase.GetCurrentMethod(), "TurtleBay initialisierung");

            var siteMap = new SiteMap(Context);

            siteMap.AddPage("Assets", "Assets", (x) => { return new WorkerFile(x, Context.AssetBaseFolder); });
            
            siteMap.AddPage("Home", (x) => { return new WorkerPage<PageDashboard>(x); });
            siteMap.AddPage("Dashboard", "dashboard", (x) => { return new WorkerPage<PageDashboard>(x); });
            siteMap.AddPage("Verlauf", "history", (x) => { return new WorkerPage<PageHistory>(x); });
            siteMap.AddPage("DS18B201", "ds", (x) => { return new WorkerPage<PageDS18B201>(x); });
            siteMap.AddPage("Einstellungen", "settings", (x) => { return new WorkerPage<PageSettings>(x); });
            siteMap.AddPage("Heizung", "heating", (x) => { return new WorkerPage<PageSettingsHeating>(x); });
            siteMap.AddPage("Scheinwerfer", "lighting", (x) => { return new WorkerPage<PageSettingsLighting>(x); });
            siteMap.AddPage("Logging", "log", (x) => { return new WorkerPage<PageLog>(x); });
            siteMap.AddPage("Hilfe", "help", (x) => { return new WorkerPage<PageHelp>(x); });
            siteMap.AddPage("Info", "info", (x) => { return new WorkerPage<PageHelp>(x); });
            siteMap.AddPage("Reset", "reset", (x) => { return new WorkerPage<PageReset>(x); });
            siteMap.AddPage("Reboot", "reboot", (x) => { return new WorkerPage<PageReboot>(x); });
            siteMap.AddPage("API", "api", (x) => { return new WorkerPage<PageApiBase>(x); });
            siteMap.AddPage("Debug", "debug", (x) => { return new WorkerPage<PageDebug>(x); });

            siteMap.AddPath("Assets", true);
            siteMap.AddPath("Home");
            siteMap.AddPath("Home/Dashboard");
            siteMap.AddPath("Home/Verlauf");
            siteMap.AddPath("Home/DS18B201");
            siteMap.AddPath("Home/Einstellungen");
            siteMap.AddPath("Home/Einstellungen/Heizung");
            siteMap.AddPath("Home/Einstellungen/Scheinwerfer");
            siteMap.AddPath("Home/Logging");
            siteMap.AddPath("Home/Hilfe");
            siteMap.AddPath("Home/Info");
            siteMap.AddPath("Home/Reset");
            siteMap.AddPath("Home/Reboot");
            siteMap.AddPath("Home/API");
            siteMap.AddPath("Home/Debug");

            Register(siteMap);

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
