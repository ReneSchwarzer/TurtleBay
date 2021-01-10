using WebExpress.Html;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;

namespace TurtleBay.WebControl
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
            
            Margin = new PropertySpacingMargin(PropertySpacing.Space.One);
            Icon = new PropertyIcon(TypeIcon.PowerOff);
            BackgroundColor = new PropertyColorButton(TypeColorButton.Danger);
        }

        /// <summary>
        /// In HTML konvertieren
        /// </summary>
        /// <param name="context">Der Kontext, indem das Steuerelement dargestellt wird</param>
        /// <returns>Das Control als HTML</returns>
        public override IHtmlNode Render(RenderContext context)
        {
            Text = context.I18N("turtlebay.reboot.label");

            Modal = new ControlModal
            (
                "reboot",
                context.I18N("turtlebay.reboot.label"),
                new ControlText()
                {
                    Text = context.I18N("turtlebay.reboot.help")
                },
                new ControlButton()
                {
                    Text = context.I18N("turtlebay.reboot.label"),
                    Icon = new PropertyIcon(TypeIcon.PowerOff),
                    Margin = new PropertySpacingMargin(PropertySpacing.Space.One),
                    BackgroundColor = new PropertyColorButton(TypeColorButton.Danger),
                    OnClick = $"window.location.href = '{ context.Uri.Root.Append("reboot") }'"
                }
            );

            return base.Render(context);
        }
    }
}
