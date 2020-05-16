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
            Layout = TypeLayoutTab.Tab;
            HorizontalAlignment = TypeHorizontalAlignmentTab.Center;

            Items.Add(new ControlLink(Page)
            {
                Text = "Tag",
                Uri = Page.Uri.Root.Append("settings"),
                Active = Page is PageSettings ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Sun)
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Heizung",
                Uri = Page.Uri.Take("Einstellungen").Append("heating"),
                Active = Page is PageSettingsHeating ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Fire)
            });

            Items.Add(new ControlLink(Page)
            {
                Text = "Scheinwerfer",
                Uri = Page.Uri.Take("Einstellungen").Append("lighting"),
                Active = Page is PageSettingsLighting ? TypeActive.Active : TypeActive.None,
                Icon = new PropertyIcon(TypeIcon.Lightbulb)
            });
        }
    }
}

