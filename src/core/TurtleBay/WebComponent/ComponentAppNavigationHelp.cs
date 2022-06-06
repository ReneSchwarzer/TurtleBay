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
    [Section(Section.AppNavigationSecondary)]
    [Application("TurtleBay")]
    public sealed class ComponentAppNavigationHelp : ComponentControlNavigationItemLink
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ComponentAppNavigationHelp()
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
            Text = "turtlebay:turtlebay.help.label";
            Uri = context.Request.Uri.Root.Append("help");
            Active = context.Page is IPageHelp ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.InfoCircle);

            return base.Render(context);
        }

    }
}
