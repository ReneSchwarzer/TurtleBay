﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebExpress.Messages;
using WebExpress.Pages;
using WebServer.Html;

namespace WebExpress.UI.Controls
{
    public class ControlPagination : Control
    {
        /// <summary>
        /// Liefert oder setzt die Anzahl der Seiten
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Liefert oder setzt die Seitengröße
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Liefert oder setzt die aktuelle Seite
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Liefert oder setzt die maximale Anzahl der Seitenschaltflächen
        /// </summary>
        public int MaxDisplayCount { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="page">Die zugehörige Seite</param>
        /// <param name="id">Die ID</param>
        public ControlPagination(IPage page, string id = null)
            : base(page, id)
        {
            MaxDisplayCount = 5;

            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        private void Init()
        {
            //AddParam("count");
            AddParam("size", ParameterScope.Session);
            AddParam("offset", ParameterScope.Local);

            //Count = GetParam("count", 0);
            Size = GetParam("size", 50);
            Offset = GetParam("offset", 0);
        }

        /// <summary>
        /// In HTML konvertieren
        /// </summary>
        /// <returns>Das Control als HTML</returns>
        public override IHtmlNode ToHtml()
        {
            var classes = new List<string>
            {
                Class,
                "pagination"
            };

            switch (HorizontalAlignment)
            {
                case TypesHorizontalAlignment.Left:
                    classes.Add("float-left");
                    break;
                case TypesHorizontalAlignment.Right:
                    classes.Add("float-right");
                    break;
            }

            var html = new HtmlElementUl()
            {
                Class = string.Join(" ", classes.Where(x => !string.IsNullOrWhiteSpace(x)))
            };

            if (Offset >= Count)
            {
                Offset = Count - 1;
            }

            if (Offset < 0)
            {
                Offset = 0;
            }

            if (Offset > 0 && Count > 1)
            {
                html.Elements.Add
                (
                    new HtmlElementLi
                    (
                        new ControlLink(Page, null)
                        {
                            Params = Parameter.Create(new Parameter("offset", Offset - 1) { Scope = ParameterScope.Local }),
                            Class = "page-link fas fa-angle-left"
                        }.ToHtml()
                    )
                    {
                        Class = "page-item"
                    }
                );
            }
            else
            {
                html.Elements.Add
                (
                    new HtmlElementLi
                    (
                        new ControlLink(Page, null)
                        {
                            Params = Parameter.Create(),
                            Class = "page-link fas fa-angle-left"
                        }.ToHtml()
                    )
                    {
                        Class = "page-item disabled"
                    }
                );
            }

            var buf = new List<int>(MaxDisplayCount);

            var j = 0;
            var k = 0;

            buf.Add(Offset);
            while (buf.Count < Math.Min(Count, MaxDisplayCount))
            {
                if (Offset + j + 1 < Count)
                {
                    j += 1;
                    buf.Add(Offset + j);
                }

                if (Offset - k - 1 >= 0)
                {
                    k += 1;
                    buf.Add(Offset - k);
                }
            }

            buf.Sort();

            foreach (var v in buf)
            {
                if (v == Offset)
                {
                    html.Elements.Add
                    (
                        new HtmlElementLi
                        (
                            new ControlLink(Page, null, (v + 1).ToString())
                            {
                                Params = Parameter.Create(new Parameter("offset", v) { Scope = ParameterScope.Local }),
                                Class = "page-link"
                            }.ToHtml()
                        )
                        {
                            Class = "page-item active"
                        }
                    );
                }
                else
                {
                    html.Elements.Add
                    (
                        new HtmlElementLi
                        (
                            new ControlLink(Page, null, (v + 1).ToString())
                            {
                                Params = Parameter.Create(new Parameter("offset", v) { Scope = ParameterScope.Local }),
                                Class = "page-link"
                            }.ToHtml()
                        )
                        {
                            Class = "page-item"
                        }
                    );
                }
            }

            if (Offset < Count - 1)
            {
                html.Elements.Add
                (
                    new HtmlElementLi
                    (
                        new ControlLink(Page, null)
                        {
                            Params = Parameter.Create(new Parameter("offset", Offset + 1) { Scope = ParameterScope.Local }),
                            Class = "page-link fas fa-angle-right"
                        }.ToHtml()
                    )
                    {
                        Class = "page-item"
                    }
                );
            }
            else
            {
                html.Elements.Add
                (
                    new HtmlElementLi
                    (
                        new ControlLink(Page, null)
                        {
                            Params = Parameter.Create(),
                            Class = "page-link fas fa-angle-right"
                        }.ToHtml()
                    )
                    {
                        Class = "page-item disabled"
                    }
                );
            }

            return html;
        }
    }
}
