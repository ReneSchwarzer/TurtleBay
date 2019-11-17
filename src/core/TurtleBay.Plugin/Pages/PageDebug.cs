﻿using TurtleBay.Plugin.Model;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageDebug : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageDebug()
            : base("Debug")
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

            ViewModel.Instance.Settings.DebugMode = !ViewModel.Instance.Settings.DebugMode;
            ViewModel.Instance.SaveSettings();

            Redirecting("/log");
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
