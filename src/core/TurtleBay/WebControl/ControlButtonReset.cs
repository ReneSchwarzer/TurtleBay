using WebExpress.Html;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;

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
            Text = context.I18N("turtlebay.reset.description");

            Modal = new ControlModal
            (
                "reset",
                context.I18N("turtlebay.reset.label"),
                new ControlText()
                {
                    Text = context.I18N("turtlebay.reset.help")
                },
                new ControlButton()
                {
                    Text = context.I18N("turtlebay.reset.label"),
                    Icon = new PropertyIcon(TypeIcon.Undo),
                    Margin = new PropertySpacingMargin(PropertySpacing.Space.One),
                    BackgroundColor = new PropertyColorButton(TypeColorButton.Warning),
                    OnClick = $"window.location.href = '{ context.Uri.Root.Append("reset") }'"
                }
            );

            return base.Render(context);
        }
    }
}
