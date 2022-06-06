using TurtleBay.WebResource;
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
    [Section(Section.AppSettingsSecondary)]
    [Application("TurtleBay")]
    public sealed class ComponentAppSettingsSettings : ComponentControlDropdownItemLink
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ComponentAppSettingsSettings()
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
            Text = "turtlebay:turtlebay.settings.label";
            Uri = context.Request.Uri.Root.Append("settings");
            Active = context.Page is IPageSetting ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.Cog);

            return base.Render(context);
        }

    }
}
