using TurtleBay.WebPage;
using TurtleBay.WebPageSetting;
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
    [Section(Section.AppSettingsPrimary)]
    [Module<Module>]
    public sealed class FragmentSettingsDS18B201 : FragmentControlDropdownItemLink
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public FragmentSettingsDS18B201()
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
            Text = "turtlebay:turtlebay.ds18b201.label";
            Uri = ComponentManager.SitemapManager.GetUri<PageDS18B201>();
            Active = context.Page is IPageDS18B201 ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.Microchip);

            return base.Render(context);
        }
    }
}
