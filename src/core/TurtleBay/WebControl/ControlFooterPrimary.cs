using TurtleBay.Model;
using WebExpress.UI.WebAttribute;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebFragment;
using WebExpress.WebAttribute;
using WebExpress.WebHtml;
using WebExpress.WebPage;

namespace TurtleBay.WebControl
{
    [Section(Section.FooterPrimary)]
    [Application("TurtleBay")]
    public sealed class ControlFooterPrimary : ControlText
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlFooterPrimary()
            : base()
        {
            Text = string.Format("{0}", ViewModel.Now);
            TextColor = new PropertyColorText(TypeColorText.Muted);
            Format = TypeFormatText.Center;
            Size = new PropertySizeText(TypeSizeText.Small);
        }

        /// <summary>
        /// In HTML konvertieren
        /// </summary>
        /// <param name="context">Der Kontext, indem das Steuerelement dargestellt wird</param>
        /// <returns>Das Control als HTML</returns>
        public override IHtmlNode Render(RenderContext context)
        {
            return base.Render(context);
        }
    }
}
