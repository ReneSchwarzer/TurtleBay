﻿using TurtleBay.Plugin.Controls;
using TurtleBay.Plugin.Model;
using WebExpress.Html;
using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageHelp : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageHelp()
            : base("")
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

            Main.Content.Add
            (
                new ControlPanelCenter
                (
                    this,
                    new ControlImage(this)
                    {
                        Source = Uri.Root.Append("Assets/img/Turtle.png"),
                        Width = 200
                    },
                    new ControlText(this)
                    {
                        Text = string.Format("Version"),
                        Color = TypesTextColor.Primary
                    },
                    new ControlText(this)
                    {
                        Text = string.Format("{0}", Context.Version),
                        Color = TypesTextColor.Dark
                    },
                    new ControlText(this)
                    {
                        Text = string.Format("Kontakt"),
                        Color = TypesTextColor.Primary
                    },
                    new ControlLink(this)
                    {
                        Text = string.Format("rene_schwarzer@hotmail.de"),
                        Uri = new UriAbsolute()
                        {
                            Scheme = UriScheme.Mailto,
                            Authority = new UriAuthority("rene_schwarzer@hotmail.de")
                        },
                        Color = TypesTextColor.Dark
                    },
                    new ControlLine(this),
                    new ControlButtonReboot(this)
                )
            );

            Main.Content.Add(new ControlLine(this));

            Main.Content.Add(new ControlText(this)
            {
                Text = "Datenschutzrichtlinie",
                Format = TypesTextFormat.H4
            });

            Main.Content.Add(new ControlText(this)
            {
                Text = "Die während der Nutzung eingegebenen Daten werden lokal auf Ihrem Gerät gespeichert. Sie behalten jederzeit die Datenhoheit. Die Daten werden zu keiner Zeit an Dritte übermittelt. Persönliche Informationen und Standortinformationen werden nicht erhoben.",
                Format = TypesTextFormat.Paragraph
            });

            Main.Content.Add(new ControlText(this)
            {
                Text = "Haftungsausschluss",
                Format = TypesTextFormat.H4
            });

            Main.Content.Add(new ControlText(this)
            {
                Text = "Die Haftung für Schäden durch Sachmängel wird ausgeschlossen. Die Haftung auf Schadensersatz wegen Körperverletzung sowie bei grober Fahrlässigkeit oder Vorsatz bleibt unberührt.",
                Format = TypesTextFormat.Paragraph
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
