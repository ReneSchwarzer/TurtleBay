using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Controls
{
    public class ControlTabSettings : ControlTab
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="page">Die zugehörige Seite</param>
        public ControlTabSettings(IPage page)
            : base(page)
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        private void Init()
        {
            Layout = TypesLayoutTab.Tab;
            HorizontalAlignment = TypesTabHorizontalAlignment.Center;

            Items.Add(new ControlLink(Page)
            {
                Text = "Tag",
                Url = Page.GetUrl(0, "settings"),
                Class = Page.Url == "/settings" ? "active" : string.Empty,
                Icon = Icon.Sun
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Heizung",
                Url = Page.GetUrl("settings", "heating"),
                Class = Page.Url == "/settings/heating" ? "active" : string.Empty,
                Icon = Icon.Fire
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Scheinwerfer",
                Url = Page.GetUrl("settings", "lighting"),
                Class = Page.Url == "/settings/lighting" ? "active" : string.Empty,
                Icon = Icon.Lightbulb
            });
        }
    }
}

