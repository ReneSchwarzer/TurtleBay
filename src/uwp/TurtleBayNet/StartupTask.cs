using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using WebExpress;
using Windows.ApplicationModel.Background;
using Windows.Storage;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace TurtleBayNet
{
    public sealed class StartupTask : IBackgroundTask
    {
        /// <summary>
        /// Der HttpServer
        /// </summary>
        private static HttpServer HttpServer { get; set; }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            CreateSample();

            // Initialisierung des WebServers
            Init(80);

            // Start des WebServers
            Start();

            // Beenden
            Exit();
        }

        // <summary>
        /// Beispieldaten bereitstellen
        /// </summary>
        /// <returns></returns>
        public async void CreateSample()
        {
            var appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var assetsFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("assets", CreationCollisionOption.OpenIfExists);

            // css kopieren
            var assets = await appInstalledFolder.GetFolderAsync("Assets\\css");
            var cssFolder = await assetsFolder.CreateFolderAsync("css", CreationCollisionOption.OpenIfExists);
            foreach (var file in from x in await assets.GetFilesAsync() select x)
            {
                // Kopieren
                await file.CopyAsync
                (
                    cssFolder,
                    file.Name,
                    NameCollisionOption.ReplaceExisting
                );
            }

            // fonts kopieren
            assets = await appInstalledFolder.GetFolderAsync("Assets\\fonts");
            var fontsFolder = await assetsFolder.CreateFolderAsync("fonts", CreationCollisionOption.OpenIfExists);
            foreach (var file in from x in await assets.GetFilesAsync() select x)
            {
                // Kopieren
                await file.CopyAsync
                (
                    fontsFolder,
                    file.Name,
                    NameCollisionOption.ReplaceExisting
                );
            }

            // js kopieren
            assets = await appInstalledFolder.GetFolderAsync("Assets\\js");
            var jsFolder = await assetsFolder.CreateFolderAsync("js", CreationCollisionOption.OpenIfExists);
            foreach (var file in from x in await assets.GetFilesAsync() select x)
            {
                // Kopieren
                await file.CopyAsync
                (
                    jsFolder,
                    file.Name,
                    NameCollisionOption.ReplaceExisting
                );
            }

            // img kopieren
            assets = await appInstalledFolder.GetFolderAsync("Assets\\img");
            var imgFolder = await assetsFolder.CreateFolderAsync("img", CreationCollisionOption.OpenIfExists);
            foreach (var file in from x in await assets.GetFilesAsync() select x)
            {
                // Kopieren
                await file.CopyAsync
                (
                    imgFolder,
                    file.Name,
                    NameCollisionOption.ReplaceExisting
                );
            }
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Anwendung mittels Ctrl+C beendet werden soll
        /// </summary>
        /// <param name="sender">Der Auslöser des Events</param>
        /// <param name="e">Die Eventargumente</param>
        private static void OnCancel(object sender, ConsoleCancelEventArgs e)
        {
            Exit();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        /// <param name="port">Der Port</param>
        private static void Init(int port)
        {
            var context = new HttpServerContext();
            HttpServer = new HttpServer(port, context)
            {
                Config = null
            };

            HttpServer.RegisterPlugin(new Plugin.TurtleBay());

            Console.CancelKeyPress += OnCancel;
        }

        /// <summary>
        /// Start des WebServers
        /// </summary>
        private static void Start()
        {
            HttpServer.Start();

            Thread.CurrentThread.Join();
        }

        /// <summary>
        /// Beendet die Anwendung
        /// </summary>
        private static void Exit()
        {
            HttpServer.Stop();
        }

        /// <summary>
        /// Liefert die Programmversion
        /// </summary>
        public static string Version => Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
