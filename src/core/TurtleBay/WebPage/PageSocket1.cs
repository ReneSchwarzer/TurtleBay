
using TurtleBay.Model;
using WebExpress.WebApp.WebPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebPage
{
    [ID("Socket1")]
    [Title("turtlebay:turtlebay.socket1.label")]
    [Segment("socket1", "turtlebay:turtlebay.socket1.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("socket")]
    public sealed class PageSocket1 : PageWebApp
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSocket1()
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

            ViewModel.Instance.Socket1Switch = !ViewModel.Instance.Socket1Switch;

            Redirecting(context.Request.Uri.Root);
        }
    }
}
