using System;
using TurtleBay.Plugin.Controls;
using TurtleBay.Plugin.Model;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageSettingsHeating : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSettingsHeating()
            : base("Heizung")
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

            Main.Content.Add(new ControlTabSettings(this)
            {

            });

            Main.Content.Add(new ControlFormHeating(this)
            {

            });
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
