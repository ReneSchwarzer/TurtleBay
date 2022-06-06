using TurtleBay.Model;
using WebExpress.WebApp.WebPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebPage
{
    [ID("Socket2")]
    [Title("turtlebay:turtlebay.socket2.label")]
    [Segment("socket2", "turtlebay:turtlebay.socket2.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("socket")]
    public sealed class PageSocket2 : PageWebApp
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSocket2()
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

            ViewModel.Instance.Socket2Switch = !ViewModel.Instance.Socket2Switch;

            Redirecting(context.Request.Uri.Root);
        }
    }
}
