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

            Items.Add(new ControlNavigationItemLink()
            {
                Text = context.I18N("turtlebay.setting.socket1.label"),
                Uri = context.Uri.Root.Append("settings").Append("socket1"),
                Active = context.Page is PageSettingsSocket1 ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Plug)
            });

            Items.Add(new ControlNavigationItemLink()
            {
                Text = context.I18N("turtlebay.setting.socket2.label"),
                Uri = context.Uri.Root.Append("settings").Append("socket2"),
                Active = context.Page is PageSettingsSocket2 ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Plug)
            });

            return base.Render(context);
        }
    }
}

