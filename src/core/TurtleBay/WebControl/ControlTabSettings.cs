using TurtleBay.WebResource;
using WebExpress.Html;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;

namespace TurtleBay.WebControl
{
    public class ControlTabSettings : ControlNavigation
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlTabSettings()
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
            Layout = TypeLayoutTab.Tab;
            HorizontalAlignment = TypeHorizontalAlignmentTab.Center;

            Items.Add(new ControlNavigationItemLink()
            {
                Text = context.I18N("turtlebay.setting.day.label"),
                Uri = context.Uri.Root.Append("settings"),
                Active = context.Page is PageSettings ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Sun)
            });

            Items.Add(new ControlNavigationItemLink()
            {
                Text = context.I18N("turtlebay.setting.heating.label"),
                Uri = context.Uri.Root.Append("settings").Append("heating"),
                Active = context.Page is PageSettingsHeating ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Fire)
            });

            Items.Add(new ControlNavigationItemLink()
            {
                Text = context.I18N("turtlebay.setting.lighting.label"),
                Uri = context.Uri.Root.Append("settings").Append("lighting"),
                Active = context.Page is PageSettingsLighting ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Lightbulb)
            });

            return base.Render(context);
        }
    }
}

