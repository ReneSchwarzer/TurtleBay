using TurtleBayNet.Plugin.Controls;
using TurtleBayNet.Plugin.Model;
using WebExpress.UI.Controls;

namespace TurtleBayNet.Plugin.Pages
{
    public sealed class PageInfo : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageInfo()
            : base("TurtleBay")
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Init()
        {
            base.Init();
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();

            Main.Content.Add
            (
                new ControlPanelCenter
                (
                    this,
                    new ControlImage(this)
                    {
                        Source = "/Assets/img/Turtle.png",
                        Width = 200
                    },
                    new ControlText(this)
                    {
                        Text = string.Format("Version"),
                        Color = TypesTextColor.Primary
                    },
                    new ControlText(this)
                    {
                        Text = string.Format("{0}", ViewModel.Instance.Version),
                        Color = TypesTextColor.Dark
                    },
                    new ControlText(this)
                    {
                        Text = string.Format("Kontakt"),
                        Color = TypesTextColor.Primary
                    },
                    new ControlLink(this)
                    {
                        Text = string.Format("rene_schwarzer@hotmail.de"),
                        Url = "mailto:rene_schwarzer@hotmail.de",
                        Color = TypesTextColor.Dark
                    },
                    new ControlLine(this),
                    new ControlButtonReboot(this)
                )
            );


        }

        /// <summary>
        /// In String konvertieren
        /// </summary>
        /// <returns>Das Objekt als String</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
