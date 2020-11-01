using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Controls
{
    public class ControlButtonReset : ControlButton
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlButtonReset()
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        private void Init()
        {
            Text = "Zähler zurücksetzen";
            Margin = new PropertySpacingMargin(PropertySpacing.Space.One);
            Icon = new PropertyIcon(TypeIcon.Undo);
            Color = new PropertyColorButton(TypeColorButton.Warning);

            Modal = new ControlModal
            (
                "reset",
                "Zurücksetzen",
                new ControlText()
                {
                    Text = "Möchten Sie die Dauerwerte wirklich zurücksetzen?"
                },
                new ControlButton()
                {
                    Text = "Zurücksetzen",
                    Icon = new PropertyIcon(TypeIcon.Undo),
                    Margin = new PropertySpacingMargin(PropertySpacing.Space.One),
                    Color = new PropertyColorButton(TypeColorButton.Warning),
                    OnClick = "window.location.href = '/reset'"
                }
            );
        }
    }
}
