using TurtleBay.Model;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("History")]
    [Title("turtlebay.history.label")]
    [Segment("history", "turtlebay.history.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("history")]
    public sealed class PageHistory : PageTemplateWebApp, IPageHistory
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageHistory()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();

            Favicons.Add(new Favicon(Uri.Root.Append("/assets/img/Favicon.png").ToString(), TypeFavicon.PNG));
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();

            Content.Preferences.Add(new ControlText()
            {
                Text = this.I18N("turtlebay.history.description"),
                Format = TypeFormatText.Center,
                TextColor = new PropertyColorText(TypeColorText.Primary),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Three)
            });

            var table = new ControlTable();
            table.AddColumn(this.I18N("turtlebay.history.time"), new PropertyIcon(TypeIcon.Clock), TypesLayoutTableRow.Info);
            table.AddColumn(this.I18N("turtlebay.history.temperature"), new PropertyIcon(TypeIcon.ThermometerQuarter), TypesLayoutTableRow.Danger);
            table.AddColumn(this.I18N("turtlebay.history.lighting"), new PropertyIcon(TypeIcon.Lightbulb), TypesLayoutTableRow.Warning);
            table.AddColumn(this.I18N("turtlebay.history.heating"), new PropertyIcon(TypeIcon.Fire), TypesLayoutTableRow.Warning);

            foreach (var v in ViewModel.Instance.Statistic.Chart24h)
            {
                var row = new ControlTableRow() { };
                row.Cells.Add(new ControlText() { Text = string.Format("{0} Uhr", v.Time.ToShortTimeString()) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0}°C", v.Temperature) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0} Minuten", v.LightingCount / 60000) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0} Minuten", v.HeatingCount / 60000) });

                table.Rows.Add(row);
            }

            Content.Primary.Add(table);
        }
    }
}
