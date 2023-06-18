using TurtleBay.Model;
using WebExpress.WebApp.WebPage;
using WebExpress.WebAttribute;
using WebExpress.WebComponent;
using WebExpress.WebResource;
using WebExpress.WebScope;

namespace TurtleBay.WebPage
{
    [Title("turtlebay:turtlebay.reset.label")]
    [Segment("reset", "turtlebay:turtlebay.reset.label")]
    [ContextPath("/")]
    [Module<Module>]
    public sealed class PageReset : PageWebApp, IScope
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

            Redirecting(ComponentManager.SitemapManager.GetUri<PageDashboard>());
        }
    }
}
