using TurtleBay.Plugin.Controls;
using TurtleBay.Plugin.Model;
using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageInfo : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageInfo()
            : base("")
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
                        Source = GetPath(0, "/Assets/img/Turtle.png"),
                        Width = 200
                    },
                    new ControlText(this)
                    {
                        Text = string.Format("Version"),
                        Color = TypesTextColor.Primary
                    },
                    new ControlText(this)
                    {
                        Text = string.Format("{0}", Context.Version),
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
                        Url = new PathExtern("mailto:rene_schwarzer@hotmail.de"),
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
