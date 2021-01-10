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
    [Section(Section.AppNavigationSecondary)]
    [Application("TurtleBay")]
    public sealed class ControlAppNavigationHelp : ControlNavigationItemLink, IComponent
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlAppNavigationHelp()
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
            Text = context.I18N("turtlebay.help.label");
            Uri = context.Page.Uri.Root.Append("help");
            Active = context.Page is IPageHelp ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.InfoCircle);

            return base.Render(context);
        }

    }
}
