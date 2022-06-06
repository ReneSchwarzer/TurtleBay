using WebExpress.Html;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;
using WebExpress.WebPage;

namespace TurtleBay.WebControl
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
            BackgroundColor = new PropertyColorButton(TypeColorButton.Warning);
        }

        /// <summary>
        /// In HTML konvertieren
        /// </summary>
        /// <param name="context">Der Kontext, indem das Steuerelement dargestellt wird</param>
        /// <returns>Das Control als HTML</returns>
        public override IHtmlNode Render(RenderContext context)
        {
            Text = "turtlebay:turtlebay.reset.description";

            Modal = new PropertyModal(TypeModal.Modal, new ControlModal
            (
                "reset",
                context.I18N("turtlebay.reset.label"),
                new ControlText()
                {
                    Text = "turtlebay:turtlebay.reset.help"
                },
                new ControlButton()
                {
                    Text = "turtlebay:turtlebay.reset.label",
                    Icon = new PropertyIcon(TypeIcon.Undo),
                    Margin = new PropertySpacingMargin(PropertySpacing.Space.One),
                    BackgroundColor = new PropertyColorButton(TypeColorButton.Warning),
                    OnClick = new PropertyOnClick($"window.location.href = '{ context.Uri.Root.Append("reset") }'")
                }
            ));

            return base.Render(context);
        }
    }
}
