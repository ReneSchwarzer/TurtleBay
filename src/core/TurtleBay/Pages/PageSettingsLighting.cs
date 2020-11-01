using System;
using TurtleBay.Plugin.Controls;
using TurtleBay.Plugin.Model;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageSettingsLighting : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSettingsLighting()
            : base("Scheinwerfer")
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

            Main.Content.Add(new ControlFormLighting()
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
