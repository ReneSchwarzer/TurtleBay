using TurtleBay.Plugin.Pages;
using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Controls
{
    public class ControlTabMenu : ControlTab
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="page">Die zugehörige Seite</param>
        public ControlTabMenu(IPage page)
            : base(page)
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        private void Init()
        {
            Layout = TypesLayoutTab.Pill;
            HorizontalAlignment = TypesTabHorizontalAlignment.Center;

            Items.Add(new ControlLink(Page)
            {
                Text = "Zentrale",
                Url = Page.GetPath(0),
                Class = Page is PageDashboard ? "active" : string.Empty,
                Icon = Icon.TachometerAlt
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Verlauf",
                Url = Page.GetPath(0, "history"),
                Class = Page is PageHistory ? "active" : string.Empty,
                Icon = Icon.ChartBar
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "DS18B201",
                Url = Page.GetPath(0, "ds"),
                Class = Page is PageDS18B201 ? "active" : string.Empty,
                Icon = Icon.Microchip
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Einstellungen",
                Url = Page.GetPath(0, "settings"),
                Class = Page is PageSettings || Page is PageSettingsHeating || Page is PageSettingsLighting ? "active" : string.Empty,
                Icon = Icon.Cog
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Logging",
                Url = Page.GetPath(0, "log"),
                Class = Page is PageLog ? "active" : string.Empty,
                Icon = Icon.Book
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Info",
                Url = Page.GetPath(0, "info"),
                Class = Page is PageInfo ? "active" : string.Empty,
                Icon = Icon.InfoCircle
            });
        }
    }
}

