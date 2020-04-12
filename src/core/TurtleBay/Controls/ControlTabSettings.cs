using TurtleBay.Plugin.Pages;
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
                Url = Page.GetPath(0, "settings"),
                Class = Page is PageSettings ? "active" : string.Empty,
                Icon = Icon.Sun
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Heizung",
                Url = Page.GetPath("settings", "heating"),
                Class = Page is PageSettingsHeating ? "active" : string.Empty,
                Icon = Icon.Fire
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Scheinwerfer",
                Url = Page.GetPath("settings", "lighting"),
                Class = Page is PageSettingsLighting ? "active" : string.Empty,
                Icon = Icon.Lightbulb
            });
        }
    }
}

