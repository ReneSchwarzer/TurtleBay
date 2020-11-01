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
                    new ControlImage()
                    {
                        Source = Uri.Root.Append("Assets/img/Reboot.png"),
                        Width = 200
                    },
                    new ControlText()
                    {
                        Text = "Der Rechner wird neu gestartet! Bitte warten Sie einen Augenblick.",
                        TextColor = new PropertyColorText(TypeColorText.Danger)
                    }
                    ,
                    new ControlText()
                    {
                        Text = "...",
                        TextColor = new PropertyColorText(TypeColorText.Primary)
                    }
                )
            );

            ViewModel.Instance.Reboot();
        }
    }
}
