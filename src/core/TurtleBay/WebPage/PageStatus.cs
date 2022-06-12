using TurtleBay.Model;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebPage;
using WebExpress.WebAttribute;
using WebExpress.WebPage;
using WebExpress.WebResource;

namespace TurtleBay.WebPage
{
    [Id("Status")]
    [Title("turtlebay:turtlebay.status.label")]
    [Segment("status", "turtlebay:turtlebay.status.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("status")]
    public sealed class PageStatus : PageWebApp
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageStatus()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        /// <param name="context">Der Kontext</param>
        public override void Initialization(IResourceContext context)
        {
            base.Initialization(context);
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        /// <param name="context">Der Kontext zum Rendern der Seite</param>
        public override void Process(RenderContextWebApp context)
        {
            base.Process(context);
        }

        /// <summary>
        /// In String konvertieren
        /// </summary>
        /// <returns>Das Objekt als String</returns>
        public override string ToString()
        {
            var layout = TypeColorBackground.Success;
            var temp = ViewModel.Instance.PrimaryTemperature;

            return new ControlCardCounter("temperature")
            {
                Text = "Aktuelle Temperatur",
                Value = string.Format("{0} °C", temp.ToString("0.0")),
                Icon = new PropertyIcon(TypeIcon.ThermometerQuarter),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(layout)
            }.Render(new RenderContext()).ToString();
        }
    }
}
