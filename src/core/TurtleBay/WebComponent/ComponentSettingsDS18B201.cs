using TurtleBay.WebPageSetting;
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
    [Section(Section.AppSettingsPrimary)]
    [Application("TurtleBay")]
    public sealed class ComponentSettingsDS18B201 : ComponentControlDropdownItemLink
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ComponentSettingsDS18B201()
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
            Text = "turtlebay:turtlebay.ds18b201.label";
            Uri = context.Request.Uri.Root.Append("ds18b201");
            Active = context.Page is IPageDS18B201 ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.Microchip);

            return base.Render(context);
        }
    }
}
