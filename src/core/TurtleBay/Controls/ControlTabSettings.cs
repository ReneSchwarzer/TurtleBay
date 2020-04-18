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
                Uri = Page.Uri.Root.Append("settings"),
                Class = Page is PageSettings ? "active" : string.Empty,
                Icon = Icon.Sun
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Heizung",
                Uri = Page.Uri.Take("Einstellungen").Append("heating"),
                Class = Page is PageSettingsHeating ? "active" : string.Empty,
                Icon = Icon.Fire
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Scheinwerfer",
                Uri = Page.Uri.Take("Einstellungen").Append("lighting"),
                Class = Page is PageSettingsLighting ? "active" : string.Empty,
                Icon = Icon.Lightbulb
            });
        }
    }
}

