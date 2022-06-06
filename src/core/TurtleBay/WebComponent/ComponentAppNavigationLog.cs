using TurtleBay.WebPage;
using WebExpress.Html;
using WebExpress.Internationalization;
using WebExpress.UI.WebAttribute;
using WebExpress.UI.WebComponent;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebComponent;
using WebExpress.WebAttribute;
using WebExpress.WebPage;

namespace TurtleBay.WebComponent
{
    [Section(Section.AppNavigationPrimary)]
    [Application("TurtleBay")]
    public sealed class ComponentAppNavigationLog : ComponentControlNavigationItemLink
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ComponentAppNavigationLog()
            : base()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        /// <param name="context">Der Kontext</param>
        /// <param name="page">Die Seite, indem die Komonente aktiv ist</param>
        public override void Initialization(IComponentContext context, IPage page)
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
            Uri = context.Request.Uri.Root.Append("log");
            Active = context.Page is IPageLog ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.Book);

            return base.Render(context);
        }
    }
}
