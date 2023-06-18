using TurtleBay.WebPage;
using WebExpress.Internationalization;
using WebExpress.UI.WebAttribute;
using WebExpress.UI.WebControl;
using WebExpress.UI.WebFragment;
using WebExpress.WebApp.WebFragment;
using WebExpress.WebAttribute;
using WebExpress.WebComponent;
using WebExpress.WebHtml;
using WebExpress.WebPage;

namespace TurtleBay.WebFragment
{
    [Section(Section.AppNavigationPrimary)]
    [Module<Module>]
    public sealed class FragmentAppNavigationLog : FragmentControlNavigationItemLink
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public FragmentAppNavigationLog()
            : base()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        /// <param name="context">Der Kontext</param>
        /// <param name="page">Die Seite, indem die Komonente aktiv ist</param>
        public override void Initialization(IFragmentContext context, IPage page)
        {
            base.Initialization(context, page);

        }

        /// <summary>
        /// In HTML konvertieren
        /// </summary>
        /// <param name="context">Der Kontext, indem das Steuerelement dargestellt wird</param>
        /// <returns>Das Control als HTML</returns>
        public override IHtmlNode Render(RenderContext context)
        {
            Text = "turtlebay:turtlebay.log.label";
            Uri = ComponentManager.SitemapManager.GetUri<PageLog>();
            Active = context.Page is IPageLog ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.Book);

            return base.Render(context);
        }
    }
}
