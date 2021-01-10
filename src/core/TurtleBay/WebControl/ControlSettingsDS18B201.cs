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
    [Section(Section.AppSettingsPrimary)]
    [Application("TurtleBay")]
    public sealed class ControlSettingsDS18B201 : ControlDropdownItemLink, IComponent
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlSettingsDS18B201()
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
            Text = context.I18N("turtlebay.ds18b201.label");
            Uri = context.Page.Uri.Root.Append("ds18b201");
            Active = context.Page is IPageDS18B201 ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.Microchip);

            return base.Render(context);
        }
    }
}
