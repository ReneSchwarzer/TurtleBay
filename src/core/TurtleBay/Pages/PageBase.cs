using System.Collections.Generic;
using TurtleBay.Plugin.Controls;
using TurtleBay.Plugin.Model;
using WebExpress.Html;
using WebExpress.UI.Controls;
using WebExpress.UI.Pages;

namespace TurtleBay.Plugin.Pages
{
    public class PageBase : PageTemplateWebApp
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="title">Der Titel der Seite</param>
        public PageBase(string title)
            : base()
        {
            Title = "TurtleBay";

            if (!string.IsNullOrWhiteSpace(title))
            {
                Title += " - " + title;
            }
            
            Favicons.Add(new Favicon("/Assets/img/Favicon.png", TypesFavicon.PNG));
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Init()
        {
            base.Init();

            base.Init();
            Head.Styles = new List<string>(new[] { "position: sticky; top: 0; z-index: 99;" });
            Head.Content.Add(HamburgerMenu);
            HamburgerMenu.HorizontalAlignment = TypesHorizontalAlignment.Left;
            HamburgerMenu.Image = Uri?.Root.Append("Assets/img/Logo.png");
            HamburgerMenu.Add(new ControlLink(this) { Text = "Home", Icon = Icon.Home, Uri = Uri.Root });
            HamburgerMenu.Add(new ControlLink(this) { Text = "Verlauf", Icon = Icon.ChartBar, Uri = Uri.Root.Append("history") });
            HamburgerMenu.AddSeperator();
            HamburgerMenu.Add(new ControlLink(this) { Text = "Logging", Icon = Icon.Book, Uri = Uri.Root.Append("log") });
            HamburgerMenu.Add(new ControlLink(this) { Text = "Einstellungen", Icon = Icon.Cog, Uri = Uri.Root.Append("settings") });
            HamburgerMenu.AddSeperator();
            HamburgerMenu.Add(new ControlLink(this) { Text = "Hilfe", Icon = Icon.InfoCircle, Uri = Uri.Root.Append("help") });

            // SideBar
            ToolBar = new ControlToolBar(this)
            {
                BackgroundColor = new PropertyColorBackground("#553322"),
                HorizontalAlignment = TypesHorizontalAlignment.Left
            };
            ToolBar.Classes.Add("sidebar");

            Head.Content.Add(new ControlPanelCenter(this, new ControlText(this)
            {
                Text = Title,
                Color = new PropertyColorText(TypesTextColor.White),
                Format = TypesTextFormat.H1,
                Size = TypesSize.Default,
                Padding = new PropertySpacingPadding(PropertySpacing.Space.One),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.None, PropertySpacing.Space.None, PropertySpacing.Space.None, PropertySpacing.Space.Null),
                Styles = new List<string>(new[] { "font-size:190%; height: 50px;" })
            })); ;

            Main.Classes.Add("content");
            PathCtrl.Classes.Add("content");

            Main.Content.Add(new ControlTabMenu(this));
            Main.Content.Add(new ControlLine(this));

            Foot.Content.Add(new ControlText(this, "now")
            {
                Text = string.Format("{0}", ViewModel.Instance.Now),
                Color = new PropertyColorText(TypesTextColor.Muted),
                Format = TypesTextFormat.Center,
                Size = TypesSize.Small
            });
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();


        }
    }
}
