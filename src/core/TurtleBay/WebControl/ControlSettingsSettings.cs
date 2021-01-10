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
    [Section(Section.AppSettingsSecondary)]
    [Application("TurtleBay")]
    public sealed class ControlSettingsSettings : ControlDropdownItemLink, IComponent
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlSettingsSettings()
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
            Text = context.I18N("turtlebay.settings.label");
            Uri = context.Page.Uri.Root.Append("settings");
            Active = context.Page is IPageSetting ? TypeActive.Active : TypeActive.None;
            Icon = new PropertyIcon(TypeIcon.Cog);

            return base.Render(context);
        }

    }
}
