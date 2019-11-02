using System;
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

            foreach (var v in ViewModel.Instance.Logging.OrderByDescending(x => x.Time))
            {
                var row = new ControlTableRow(this) { };
                row.Cells.Add(new ControlText(this) { Class = func(v.Level) });
                row.Cells.Add(new ControlText(this) { Text = string.Format("{0}", v.Instance) });
                row.Cells.Add(new ControlText(this) { Text = string.Format("{0}", v.Massage) });
                row.Cells.Add(new ControlText(this) { Text = string.Format("{0} {1}", v.Time.ToShortDateString(), v.Time.ToShortTimeString()) });

                table.Rows.Add(row);
            }

            Main.Content.Add(table);
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
