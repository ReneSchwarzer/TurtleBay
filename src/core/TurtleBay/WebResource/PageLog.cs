using System;
using System.Linq;
using TurtleBay.Model;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("Log")]
    [Title("turtlebay.log.label")]
    [Segment("log", "turtlebay.log.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("log")]
    public sealed class PageLog : PageTemplateWebApp, IPageLog
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageLog()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();

            Favicons.Add(new Favicon(Uri.Root.Append("/Assets/img/Favicon.png").ToString(), TypeFavicon.PNG));
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();

            Content.Preferences.Add(new ControlText()
            {
                Text = "Ereignisse",
                Format = TypeFormatText.Center,
                TextColor = new PropertyColorText(TypeColorText.Primary),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.One)
            });

            var table = new ControlTable();
            table.AddColumn("Level", new PropertyIcon(TypeIcon.Hashtag), TypesLayoutTableRow.Info);
            table.AddColumn("Instanz", new PropertyIcon(TypeIcon.Code), TypesLayoutTableRow.Warning);
            table.AddColumn("Nachricht", new PropertyIcon(TypeIcon.CommentAlt), TypesLayoutTableRow.Danger);
            table.AddColumn("Zeit", new PropertyIcon(TypeIcon.Clock), TypesLayoutTableRow.Warning);

            Func<LogItem.LogLevel, PropertyIcon> func = (level) =>
            {
                return level switch
                {
                    LogItem.LogLevel.Info => new PropertyIcon(TypeIcon.Info),
                    LogItem.LogLevel.Debug => new PropertyIcon(TypeIcon.Bug),
                    LogItem.LogLevel.Warning => new PropertyIcon(TypeIcon.ExclamationTriangle),
                    LogItem.LogLevel.Error => new PropertyIcon(TypeIcon.Times),
                    LogItem.LogLevel.Exception => new PropertyIcon(TypeIcon.Bomb),
                    _ => new PropertyIcon(TypeIcon.None),
                };
            };

            var log = ViewModel.Instance.Logging;

            if (!ViewModel.Instance.Settings.DebugMode)
            {
                log = log.Where(x => !(x.Level == LogItem.LogLevel.Debug || x.Level == LogItem.LogLevel.Exception)).ToList();
            }

            foreach (var v in log.OrderByDescending(x => x.Time))
            {
                var row = new ControlTableRow() { };
                row.Cells.Add(new ControlIcon() { Icon = func(v.Level) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0}", v.Instance) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0}", v.Massage) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0}", v.Time.ToString("dd.MM.yyyy HH.mm.ss.f")) });

                table.Rows.Add(row);
            }

            Content.Primary.Add(table);
            Content.Primary.Add(new ControlPanelCenter(new ControlButtonLink()
            {
                Text = ViewModel.Instance.Settings.DebugMode ? "Debug-Ausgaben ausblenden" : "Debug-Ausgaben einblenden",
                Icon = new PropertyIcon(TypeIcon.Bug),
                TextColor = new PropertyColorText(TypeColorText.Warning),
                Uri = Uri.Root.Append("debug"),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Three)
            }));
        }
    }
}
