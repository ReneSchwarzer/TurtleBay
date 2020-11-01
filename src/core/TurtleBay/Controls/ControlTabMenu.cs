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
        {
            Init(page);
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        /// <param name="page">Die zugehörige Seite</param>
        private void Init(IPage page)
        {
            Layout = TypeLayoutTab.Pill;
            HorizontalAlignment = TypeHorizontalAlignmentTab.Center;

            Items.Add(new ControlLink()
            {
                Text = "Home",
                Uri = page.Uri.Root,
                Active = page is PageDashboard ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Home)
            });

            Items.Add(new ControlLink()
            {
                Text = "Verlauf",
                Uri = page.Uri.Root.Append("history"),
                Active = page is PageHistory ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.ChartBar)
            });

            Items.Add(new ControlLink()
            {
                Text = "DS18B201",
                Uri = page.Uri.Root.Append("ds"),
                Active = page is PageDS18B201 ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Microchip)
            });

            Items.Add(new ControlLink()
            {
                Text = "Einstellungen",
                Uri = page.Uri.Root.Append("settings"),
                Active = page is PageSettings || page is PageSettingsHeating || page is PageSettingsLighting ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Cog)
            });

            Items.Add(new ControlLink()
            {
                Text = "Logging",
                Uri = page.Uri.Root.Append("log"),
                Active = page is PageLog ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Book)
            });

            Items.Add(new ControlLink()
            {
                Text = "Hilfe",
                Uri = page.Uri.Root.Append("help"),
                Active = page is PageHelp ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.InfoCircle)
            });
        }
    }
}

