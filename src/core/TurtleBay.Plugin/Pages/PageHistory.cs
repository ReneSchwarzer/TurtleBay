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

            Main.Content.Add(new ControlText(this)
            {
                Text = "Temperaturverlauf der letzten 24 Stunden",
                Format = TypesTextFormat.Center,
                Color = TypesTextColor.Primary,
                Class = "m-3"
            });

            var table = new ControlTable(this);
            table.AddColumn("Zeit", Icon.Clock, TypesLayoutTableRow.Info);
            table.AddColumn("Temperatur", Icon.ThermometerQuarter, TypesLayoutTableRow.Danger);
            table.AddColumn("Scheinwerfer", Icon.Lightbulb, TypesLayoutTableRow.Warning);
            table.AddColumn("Heizung", Icon.Fire, TypesLayoutTableRow.Warning);

            foreach (var v in ViewModel.Instance.Statistic.Chart24h)
            {
                var row = new ControlTableRow(this) { };
                row.Cells.Add(new ControlText(this) { Text = string.Format("{0} Uhr", v.Time.ToShortTimeString()) });
                row.Cells.Add(new ControlText(this) { Text = string.Format("{0}°C", v.Temperature) });
                row.Cells.Add(new ControlText(this) { Text = string.Format("{0} Minuten", v.LightingCount / 60000) });
                row.Cells.Add(new ControlText(this) { Text = string.Format("{0} Minuten", v.HeatingCount / 60000) });

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
