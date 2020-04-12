using System.Linq;
using TurtleBay.Plugin.Controls;
using TurtleBay.Plugin.Model;
using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageStatus : Page
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageStatus()
            : base("Status")
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
        }

        /// <summary>
        /// In String konvertieren
        /// </summary>
        /// <returns>Das Objekt als String</returns>
        public override string ToString()
        {
            var layout = TypesLayoutCard.Success;
            var temp = ViewModel.Instance.PrimaryTemperature;

            return new ControlCardCounter(this, "temperature")
            {
                Text = "Aktuelle Temperatur",
                Value = string.Format("{0} °C", temp.ToString("0.0")),
                Icon = Icon.ThermometerQuarter,
                Color = TypesTextColor.White,
                Layout = layout
            }.ToHtml().ToString();
        }
    }
}
