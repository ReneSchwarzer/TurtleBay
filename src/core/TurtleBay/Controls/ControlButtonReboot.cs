using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Controls
{
    public class ControlButtonReboot : ControlButton
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlButtonReboot()
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        private void Init()
        {
            Text = "Reboot";
            Margin = new PropertySpacingMargin(PropertySpacing.Space.One);
            Icon = new PropertyIcon(TypeIcon.PowerOff);
            Color = new PropertyColorButton(TypeColorButton.Danger);

            Modal = new ControlModal
            (
                "reboot",
                "Neustart",
                new ControlText()
                {
                    Text = "Möchten Sie wirklich den Rechner neu starten?"
                },
                new ControlButton()
                {
                    Text = "Neustart",
                    Icon = new PropertyIcon(TypeIcon.PowerOff),
                    Margin = new PropertySpacingMargin(PropertySpacing.Space.One),
                    Color = new PropertyColorButton(TypeColorButton.Danger),
                    OnClick = "window.location.href = '/reboot'"
                }
            );
        }
    }
}
