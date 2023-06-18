using TurtleBay.WebResource;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;
using WebExpress.WebComponent;
using WebExpress.WebHtml;
using WebExpress.WebPage;

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
                Text = "turtlebay:turtlebay.setting.day.label",
                Uri = ComponentManager.SitemapManager.GetUri<PageSettings>(),
                Active = context.Page is PageSettings ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Sun)
            });

            Items.Add(new ControlNavigationItemLink()
            {
                Text = "turtlebay:turtlebay.setting.heating.label",
                Uri = ComponentManager.SitemapManager.GetUri<PageSettingsHeating>(),
                Active = context.Page is PageSettingsHeating ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Fire)
            });

            Items.Add(new ControlNavigationItemLink()
            {
                Text = "turtlebay:turtlebay.setting.lighting.label",
                Uri = ComponentManager.SitemapManager.GetUri<PageSettingsLighting>(),
                Active = context.Page is PageSettingsLighting ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Lightbulb)
            });

            Items.Add(new ControlNavigationItemLink()
            {
                Text = "turtlebay:turtlebay.setting.socket1.label",
                Uri = ComponentManager.SitemapManager.GetUri<PageSettingsSocket1>(),
                Active = context.Page is PageSettingsSocket1 ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Plug)
            });

            Items.Add(new ControlNavigationItemLink()
            {
                Text = "turtlebay:turtlebay.setting.socket2.label",
                Uri = ComponentManager.SitemapManager.GetUri<PageSettingsSocket2>(),
                Active = context.Page is PageSettingsSocket2 ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Plug)
            });

            return base.Render(context);
        }
    }
}

