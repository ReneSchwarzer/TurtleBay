using TurtleBay.Model;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("Status")]
    [Title("turtlebay.status.label")]
    [Segment("status", "turtlebay.status.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("status")]
    public sealed class PageStatus : PageTemplateWebApp
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
        public override void Initialization()
        {
            base.Initialization();

            Favicons.Add(new Favicon(Uri.Root.Append("/Assets/img/Favicon.png").ToString(), TypeFavicon.PNG));
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();
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
            }.Render(new RenderContext(this)).ToString();
        }
    }
}
