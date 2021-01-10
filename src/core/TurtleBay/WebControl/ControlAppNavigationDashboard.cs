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
    [Section(Section.AppNavigationPreferences)]
    [Application("TurtleBay")]
    public sealed class ControlAppNavigationDashboard : ControlNavigationItemLink, IComponent
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlAppNavigationDashboard()
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
            Text = context.I18N("turtlebay.dashboard.label");
            Uri = context.Page.Uri.Root;
            Active = context.Page is IPageDashboard ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.TachometerAlt);

            return base.Render(context);
        }
    }
}
