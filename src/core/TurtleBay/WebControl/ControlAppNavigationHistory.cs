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
    public sealed class ControlAppNavigationHistory : ControlNavigationItemLink, IComponent
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlAppNavigationHistory()
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
            Text = context.I18N("turtlebay.history.label");
            Uri = context.Page.Uri.Root.Append("history");
            Active = context.Page is IPageHistory ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.ChartBar);

            return base.Render(context);
        }
    }
}
