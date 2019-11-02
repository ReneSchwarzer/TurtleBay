﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServer.Html
{
    public class HtmlElementBody : HtmlElement
    {
        /// <summary>
        /// Liefert die Elemente
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Liefert oder setzt die Script-Elemente
        /// </summary>
        public List<string> Scripts { get; set; }

        /// <summary>
        /// Liefert oder setzt das text/javascript
        /// </summary>
        public List<string> ScriptLinks
        {
            get => (from x in ElementScriptLinks select x.Src).ToList();
            set
            {
                ElementScriptLinks.Clear(); ElementScriptLinks.AddRange(from x in value
                                                                        select new HtmlElementScript() { Language = "javascript", Src = x, Type = "text/javascript" });
            }
        }

        /// <summary>
        /// Liefert oder setzt die externen Scripts
        /// </summary>
        private List<HtmlElementScript> ElementScriptLinks { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public HtmlElementBody()
            : base("body")
        {
            ElementScriptLinks = new List<HtmlElementScript>();
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="nodes">Der Inhalt</param>
        public HtmlElementBody(params IHtmlNode[] nodes)
            : this()
        {
            Elements.AddRange(nodes);
        }

        /// <summary>
        /// In String konvertieren unter Zuhilfenahme eines StringBuilder
        /// </summary>
        /// <param name="builder">Der StringBuilder</param>
        /// <param name="deep">Die Aufrufstiefe</param>
        public override void ToString(StringBuilder builder, int deep)
        {
            ToPreString(builder, deep);

            foreach (var v in Elements)
            {
                v.ToString(builder, deep + 1);
            }

            foreach (var v in ElementScriptLinks)
            {
                v.ToString(builder, deep + 1);
            }

            if (ScriptLinks.Count > 0)
            {
                new HtmlElementScript(string.Join(Environment.NewLine, from x in Scripts select x)).ToString(builder, deep + 1);
            }

            ToPostString(builder, deep, true);
        }
    }
}
