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
        {
            Init(page);
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        /// <param name="page">Die zugehörige Seite</param>
        private void Init(IPage page)
        {
            Layout = TypeLayoutTab.Tab;
            HorizontalAlignment = TypeHorizontalAlignmentTab.Center;

            Items.Add(new ControlLink()
            {
                Text = "Tag",
                Uri = page.Uri.Root.Append("settings"),
                Active = page is PageSettings ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Sun)
            });

            Items.Add(new ControlLink()
            {
                Text = "Heizung",
                Uri = page.Uri.Take("Einstellungen").Append("heating"),
                Active = page is PageSettingsHeating ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Fire)
            });

            Items.Add(new ControlLink()
            {
                Text = "Scheinwerfer",
                Uri = page.Uri.Take("Einstellungen").Append("lighting"),
                Active = page is PageSettingsLighting ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Lightbulb)
            });
        }
    }
}

