using TurtleBay.Plugin.Model;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageReboot : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageReboot()
            : base("Reboot")
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
                        Source = Uri.Root.Append("Assets/img/Reboot.png"),
                        Width = 200
                    },
                    new ControlText(this)
                    {
                        Text = "Der Rechner wird neu gestartet! Bitte warten Sie einen Augenblick.",
                        Color = TypesTextColor.Danger
                    }
                    ,
                    new ControlText(this)
                    {
                        Text = "...",
                        Color = TypesTextColor.Primary
                    }
                )
            );

            ViewModel.Instance.Reboot();
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
