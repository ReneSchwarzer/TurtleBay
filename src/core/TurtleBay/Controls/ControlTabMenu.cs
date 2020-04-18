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
                Text = "Home",
                Uri = Page.Uri.Root,
                Class = Page is PageDashboard ? "active" : string.Empty,
                Icon = Icon.Home
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Verlauf",
                Uri = Page.Uri.Root.Append("history"),
                Class = Page is PageHistory ? "active" : string.Empty,
                Icon = Icon.ChartBar
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "DS18B201",
                Uri = Page.Uri.Root.Append("ds"),
                Class = Page is PageDS18B201 ? "active" : string.Empty,
                Icon = Icon.Microchip
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Einstellungen",
                Uri = Page.Uri.Root.Append("settings"),
                Class = Page is PageSettings || Page is PageSettingsHeating || Page is PageSettingsLighting ? "active" : string.Empty,
                Icon = Icon.Cog
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Logging",
                Uri = Page.Uri.Root.Append("log"),
                Class = Page is PageLog ? "active" : string.Empty,
                Icon = Icon.Book
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Hilfe",
                Uri = Page.Uri.Root.Append("help"),
                Class = Page is PageHelp ? "active" : string.Empty,
                Icon = Icon.InfoCircle
            });
        }
    }
}

