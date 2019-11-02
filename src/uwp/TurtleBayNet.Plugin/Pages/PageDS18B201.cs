using TurtleBayNet.Plugin.Model;
using WebExpress.UI.Controls;

namespace TurtleBayNet.Plugin.Pages
{
    public sealed class PageDS18B201 : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageDS18B201()
            : base("DS18B201")
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

            Main.Content.Add(new ControlText(this) { Text = string.Format("ID des Temperaturfühlers: {0}", ViewModel.Instance.DeviceId) });
            Main.Content.Add(new ControlText(this) { Text = string.Format("Aktuelle Temperatur: {0} °C", ViewModel.Instance.Temperature) });
        }

        /// <summary>
        /// In String konvertieren
        /// </summary>
        /// <returns>Das Objekt als String</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
