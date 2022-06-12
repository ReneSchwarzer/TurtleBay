using TurtleBay.Model;
using WebExpress.WebApp.WebPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebPage
{
    [Id("Reset")]
    [Title("turtlebay:turtlebay.reset.label")]
    [Segment("reset", "turtlebay:turtlebay.reset.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("reset")]
    public sealed class PageReset : PageWebApp
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

            ViewModel.Instance.ResetCounter();

            Redirecting(context.Request.Uri.Root);
        }
    }
}
