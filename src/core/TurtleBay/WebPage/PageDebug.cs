using TurtleBay.Model;
using WebExpress.WebApp.WebPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebPage
{
    [ID("Debug")]
    [Title("turtlebay:turtlebay.debug.label")]
    [Segment("debug", "turtlebay:turtlebay.debug.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("debug")]
    public sealed class PageDebug : PageWebApp
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageDebug()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        /// <param name="context">Der Kontext</param>
        public override void Initialization(IResourceContext context)
        {
            base.Initialization(context);
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        /// <param name="context">Der Kontext zum Rendern der Seite</param>
        public override void Process(RenderContextWebApp context)
        {
            base.Process(context);

            ViewModel.Instance.Settings.DebugMode = !ViewModel.Instance.Settings.DebugMode;
            ViewModel.Instance.SaveSettings();

            Redirecting(context.Request.Uri.Root.Append("log"));
        }
    }
}
