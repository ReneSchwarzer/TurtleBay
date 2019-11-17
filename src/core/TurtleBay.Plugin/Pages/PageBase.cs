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

            Head.Content.Add(new ControlImage(this)
            {
                Source = "/Assets/img/Logo.png",
                Height = 50,
                HorizontalAlignment = TypesHorizontalAlignment.Left
            });

            Head.Content.Add(new ControlPanelCenter(this, new ControlText(this)
            {
                Text = Title,
                Color = TypesTextColor.White,
                Format = TypesTextFormat.H1,
                Size = TypesSize.Default,
                Class = "p-1",
                Style = "font-size:190%; height: 50px;"
            }));

            Main.Content.Add(new ControlTabMenue(this));
            Main.Content.Add(new ControlLine(this));

            Foot.Content.Add(new ControlText(this)
            {
                Text = string.Format("{0}", ViewModel.Instance.Now),
                Color = TypesTextColor.Muted,
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
