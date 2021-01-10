using TurtleBay.WebResource;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.Internationalization;
using WebExpress.UI.Attribute;
using WebExpress.UI.Component;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.Components;

namespace TurtleBay.WebControl
{
    [Section(Section.AppNavigationPrimary)]
    [Application("TurtleBay")]
    public sealed class ControlAppNavigationLog : ControlNavigationItemLink, IComponent
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlAppNavigationLog()
            : base()
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        private void Init()
        {
        }

        /// <summary>
        /// In HTML konvertieren
        /// </summary>
        /// <param name="context">Der Kontext, indem das Steuerelement dargestellt wird</param>
        /// <returns>Das Control als HTML</returns>
        public override IHtmlNode Render(RenderContext context)
        {
            Text = context.I18N("turtlebay.log.label");
            Uri = context.Page.Uri.Root.Append("log");
            Active = context.Page is IPageLog ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.Book);

            return base.Render(context);
        }
    }
}
