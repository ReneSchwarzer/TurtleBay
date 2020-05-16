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
            Layout = TypeLayoutTab.Pill;
            HorizontalAlignment = TypeHorizontalAlignmentTab.Center;

            Items.Add(new ControlLink(Page)
            {
                Text = "Home",
                Uri = Page.Uri.Root,
                Active = Page is PageDashboard ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Home)
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Verlauf",
                Uri = Page.Uri.Root.Append("history"),
                Active = Page is PageHistory ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.ChartBar)
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "DS18B201",
                Uri = Page.Uri.Root.Append("ds"),
                Active = Page is PageDS18B201 ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Microchip)
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Einstellungen",
                Uri = Page.Uri.Root.Append("settings"),
                Active = Page is PageSettings || Page is PageSettingsHeating || Page is PageSettingsLighting ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Cog)
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Logging",
                Uri = Page.Uri.Root.Append("log"),
                Active = Page is PageLog ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Book)
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Hilfe",
                Uri = Page.Uri.Root.Append("help"),
                Active = Page is PageHelp ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.InfoCircle)
            });
        }
    }
}

