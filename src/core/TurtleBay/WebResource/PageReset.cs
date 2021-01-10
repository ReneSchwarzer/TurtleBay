using TurtleBay.Model;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("Reset")]
    [Title("turtlebay.reset.label")]
    [Segment("reset", "turtlebay.reset.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("reset")]
    public sealed class PageReset : PageTemplateWebApp
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageReset()
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

            ViewModel.Instance.ResetCounter();

            Redirecting(Uri.Root);
        }
    }
}
