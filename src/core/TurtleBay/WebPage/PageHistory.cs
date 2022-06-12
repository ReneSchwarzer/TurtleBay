using TurtleBay.Model;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebPage
{
    [Id("History")]
    [Title("turtlebay:turtlebay.history.label")]
    [Segment("history", "turtlebay:turtlebay.history.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("history")]
    public sealed class PageHistory : PageWebApp, IPageHistory
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
        /// <param name="context">Der Kontext</param>
        public override void Initialization(IResourceContext context)
        {
            base.Initialization(context);
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        /// <param name="context">Der Kontext zum Rendern der Seite</param>
        public override void Process(RenderContextWebApp context)
        {
            base.Process(context);

            context.VisualTree.Content.Preferences.Add(new ControlText()
            {
                Text = "turtlebay:turtlebay.history.description",
                Format = TypeFormatText.Center,
                TextColor = new PropertyColorText(TypeColorText.Primary),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Three)
            });

            var table = new ControlTable();
            table.AddColumn("turtlebay:turtlebay.history.time", new PropertyIcon(TypeIcon.Clock), TypesLayoutTableRow.Info);
            table.AddColumn("turtlebay:turtlebay.history.temperature", new PropertyIcon(TypeIcon.ThermometerQuarter), TypesLayoutTableRow.Danger);
            table.AddColumn("turtlebay:turtlebay.history.lighting", new PropertyIcon(TypeIcon.Lightbulb), TypesLayoutTableRow.Warning);
            table.AddColumn("turtlebay:turtlebay.history.heating", new PropertyIcon(TypeIcon.Fire), TypesLayoutTableRow.Warning);

            foreach (var v in ViewModel.Instance.Statistic.Chart24h)
            {
                var row = new ControlTableRow() { };
                row.Cells.Add(new ControlText() { Text = string.Format("{0} Uhr", v.Time.ToShortTimeString()) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0}°C", v.Temperature) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0} Minuten", v.LightingCount / 60000) });
                row.Cells.Add(new ControlText() { Text = string.Format("{0} Minuten", v.HeatingCount / 60000) });

                table.Rows.Add(row);
            }

            context.VisualTree.Content.Primary.Add(table);
        }
    }
}
