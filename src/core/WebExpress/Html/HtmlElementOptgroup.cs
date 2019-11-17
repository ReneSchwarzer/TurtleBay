﻿using System.Collections.Generic;
using System.Text;

namespace WebServer.Html
{
    /// <summary>
    /// Verschachtelte Auswahllisten 
    /// <select name="top5" size="5">
    ///  <optgroup label="Namen mit A">
    ///   <option value="1">Michael Jackson</option>
    ///   <option value="2" selected>Tom Waits</option>
    ///  </optgroup>
    /// </select>
    /// </summary>
    public class HtmlElementOptgroup : HtmlElement, IHtmlFormularItem
    {
        /// <summary>
        /// Liefert die Elemente
        /// </summary>
        public new List<IHtmlNode> Elements => base.Elements;

        /// <summary>
        /// Liefert oder setzt einen Wert
        /// </summary>
        public string Label
        {
            get => GetAttribute("label");
            set => SetAttribute("label", value);
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public HtmlElementOptgroup()
            : base("optgroup")
        {

        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="nodes">Der Inhalt</param>
        public HtmlElementOptgroup(params IHtmlNode[] nodes)
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
            base.ToString(builder, deep);
        }
    }
}
