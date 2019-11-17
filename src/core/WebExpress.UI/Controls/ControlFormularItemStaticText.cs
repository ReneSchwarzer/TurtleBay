﻿using System.Collections.Generic;
using System.Linq;
using WebServer.Html;

namespace WebExpress.UI.Controls
{
    public class ControlFormularItemStaticText : ControlFormularItem, IControlFormularLabel
    {
        /// <summary>
        /// Liefert oder setzt Beschriftung
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Liefert oder setzt die Beschreibung
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="formular">Das zugehörige Formular</param>
        /// <param name="id">Die ID</param>
        public ControlFormularItemStaticText(ControlPanelFormular formular, string id = null)
            : base(formular, id)
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        private void Init()
        {
        }

        /// <summary>
        /// In HTML konvertieren
        /// </summary>
        /// <returns>Das Control als HTML</returns>
        public override IHtmlNode ToHtml()
        {
            var classes = new List<string>
            {
                Class
            };

            var c = new List<string>
            {
                "form-control-static"
            };

            var html = new HtmlElementP()
            {
                Text = Text,
                Role = Role,
                Class = string.Join(" ", classes.Where(x => !string.IsNullOrWhiteSpace(x))),
                Style = Style
            };

            return html;
        }
    }
}
