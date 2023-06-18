using TurtleBay.Model;
using TurtleBay.WebResource;
using WebExpress.WebApp.WebPage;
using WebExpress.WebAttribute;
using WebExpress.WebComponent;
using WebExpress.WebResource;
using WebExpress.WebScope;

namespace TurtleBay.WebPage
{
    [Title("turtlebay:turtlebay.socket2.label")]
    [Segment("socket2", "turtlebay:turtlebay.socket2.label")]
    [ContextPath("/")]
    [Module<Module>]
    public sealed class PageSocket2 : PageWebApp, IScope
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

            Redirecting(ComponentManager.SitemapManager.GetUri<PageDashboard>());
        }
    }
}
