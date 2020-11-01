using TurtleBay.Plugin.Model;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageHistory : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageHistory()
            : base("Verlauf")
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

            Main.Content.Add(new ControlText()
            {
                Text = "Temperaturverlauf der letzten 24 Stunden",
                Format = TypeFormatText.Center,
                TextColor = new PropertyColorText(TypeColorText.Primary),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Three)
            });

            var table = new ControlTable();
            table.AddColumn("Zeit", new PropertyIcon(TypeIcon.Clock), TypesLayoutTableRow.Info);
            table.AddColumn("Temperatur", new PropertyIcon(TypeIcon.ThermometerQuarter), TypesLayoutTableRow.Danger);
            table.AddColumn("Scheinwerfer", new PropertyIcon(TypeIcon.Lightbulb), TypesLayoutTableRow.Warning);
            table.AddColumn("Heizung", new PropertyIcon(TypeIcon.Fire), TypesLayoutTableRow.Warning);

            foreach (var v in ViewModel.Instance.Statistic.Chart24h)
            {
                var row = new ControlTableRow() { };
                row.Cells.Add(new ControlText() { Text = string.Format("{0} Uhr", v.Time.ToShortTimeString()) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0}°C", v.Temperature) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0} Minuten", v.LightingCount / 60000) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0} Minuten", v.HeatingCount / 60000) });

                table.Rows.Add(row);
            }

            Main.Content.Add(table);
        }
    }
}
