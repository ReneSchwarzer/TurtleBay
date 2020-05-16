using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Controls
{
    public class ControlButtonReset : ControlButton
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="page">Die zugehörige Seite</param>
        public ControlButtonReset(IPage page)
            : base(page)
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
                Page,
                "reset",
                "Zurücksetzen",
                new ControlText(Page)
                {
                    Text = "Möchten Sie die Dauerwerte wirklich zurücksetzen?"
                },
                new ControlButton(Page)
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
