using TurtleBay.Model;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("Debug")]
    [Title("turtlebay.debug.label")]
    [Segment("debug", "turtlebay.debug.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("debug")]
    public sealed class PageDebug : PageTemplateWebApp
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
        public override void Initialization()
        {
            base.Initialization();

            Favicons.Add(new Favicon(Uri.Root.Append("/Assets/img/Favicon.png").ToString(), TypeFavicon.PNG));
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();

            ViewModel.Instance.Settings.DebugMode = !ViewModel.Instance.Settings.DebugMode;
            ViewModel.Instance.SaveSettings();

            Redirecting(Uri.Root.Append("log"));
        }
    }
}
