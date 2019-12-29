using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Controls
{
    public class ControlTabMenue : ControlTab
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="page">Die zugehörige Seite</param>
        public ControlTabMenue(IPage page)
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
                Url = Page.GetUrl(0),
                Class = Page.Url == "/" ? "active" : string.Empty,
                Icon = Icon.TachometerAlt
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Verlauf",
                Url = Page.GetUrl(0, "history"),
                Class = Page.Url == "/history" ? "active" : string.Empty,
                Icon = Icon.ChartBar
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "DS18B201",
                Url = Page.GetUrl(0, "ds"),
                Class = Page.Url == "/ds" ? "active" : string.Empty,
                Icon = Icon.Microchip
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Einstellungen",
                Url = Page.GetUrl(0, "settings"),
                Class = Page.Url.StartsWith("/settings") ? "active" : string.Empty,
                Icon = Icon.Cog
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Logging",
                Url = Page.GetUrl(0, "log"),
                Class = Page.Url == "/log" ? "active" : string.Empty,
                Icon = Icon.Book
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Info",
                Url = Page.GetUrl(0, "info"),
                Class = Page.Url == "/info" ? "active" : string.Empty,
                Icon = Icon.InfoCircle
            });
        }
    }
}

