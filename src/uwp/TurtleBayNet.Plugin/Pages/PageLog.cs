﻿using System;
using System.Linq;
using TurtleBayNet.Plugin.Model;
using WebExpress.UI.Controls;

namespace TurtleBayNet.Plugin.Pages
{
    public sealed class PageLog : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageLog()
            : base("Logging")
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

            Main.Content.Add(new ControlText(this)
            {
                Text = "Ereignisse",
                Format = TypesTextFormat.Center,
                Color = TypesTextColor.Primary,
                Class = "m-3"
            });

            var table = new ControlTable(this);
            table.AddColumn("Level", "fas fa-hashtag", TypesLayoutTableRow.Info);
            table.AddColumn("Instanz", "fas fa-code", TypesLayoutTableRow.Warning);
            table.AddColumn("Nachricht", "fas fa-comment-alt", TypesLayoutTableRow.Danger);
            table.AddColumn("Zeit", "fas fa-clock", TypesLayoutTableRow.Warning);

            Func<LogItem.LogLevel, string> func = (level) =>
            {
                switch (level)
                {
                    case LogItem.LogLevel.Info:
                        return "fas fa-info";
                    case LogItem.LogLevel.Debug:
                        return "fas fa-bug";
                    case LogItem.LogLevel.Warning:
                        return "fas fa-exclamation-triangle";
                    case LogItem.LogLevel.Error:
                        return "fas fa-times";
                    case LogItem.LogLevel.Exception:
                        return "fas fa-bomb";
                }

                return "";
            };

            var log = ViewModel.Instance.Logging;

            if (!ViewModel.Instance.DebugMode)
            {
                log = log.Where(x => !(x.Level == LogItem.LogLevel.Debug || x.Level == LogItem.LogLevel.Exception)).ToList();
            }

            foreach (var v in log.OrderByDescending(x => x.Time))
            {
                var row = new ControlTableRow(this) { };
                row.Cells.Add(new ControlText(this) { Class = func(v.Level) });
                row.Cells.Add(new ControlText(this) { Text = string.Format("{0}", v.Instance) });
                row.Cells.Add(new ControlText(this) { Text = string.Format("{0}", v.Massage) });
                row.Cells.Add(new ControlText(this) { Text = string.Format("{0}", v.Time.ToString("dd.MM.yyyy HH.mm.ss.f")) });

                table.Rows.Add(row);
            }

            Main.Content.Add(table);
            Main.Content.Add(new ControlPanelCenter(this, new ControlButtonLink(this)
            {
                Text = ViewModel.Instance.DebugMode ? "Debug-Ausgaben ausblenden" : "Debug-Ausgaben einblenden",
                Icon = "fas fa-bug",
                Color = TypesTextColor.Warning,
                Url = "/debug",
                Class = "m-3"
            })); ;
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
