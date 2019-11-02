using TurtleBayNet.Plugin.Controls;
using TurtleBayNet.Plugin.Model;
using WebExpress.UI.Controls;

namespace TurtleBayNet.Plugin.Pages
{
    public sealed class PageDashboard : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageDashboard()
            : base("Überblick")
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

            var converter = new TimeSpanConverter();

            var grid = new ControlGrid(this) { Fluid = true };

            grid.Add(0, new ControlCardCounter(this)
            {
                Text = "Aktuelle Temperatur",
                Value = string.Format("{0} °C", ViewModel.Instance.Temperature),
                Icon = "fas fa-thermometer-quarter",
                Color = TypesTextColor.White,
                Layout = ViewModel.Instance.Temperature < ViewModel.Instance.Min || ViewModel.Instance.Temperature > ViewModel.Instance.Max ? TypesLayoutCard.Danger : TypesLayoutCard.Success
            });

            grid.Add(0, new ControlCardCounter(this)
            {
                Text = "Scheinwerfer",
                Value = ViewModel.Instance.Lighting ? "An" : "Aus",
                Icon = "fas fa-lightbulb",
                Color = TypesTextColor.White,
                Layout = ViewModel.Instance.Lighting ? TypesLayoutCard.Success : TypesLayoutCard.Info
            });

            grid.Add(0, new ControlCardCounter(this)
            {
                Text = "Heizung",
                Value = ViewModel.Instance.Heating ? "An" : "Aus",
                Icon = "fas fa-fire",
                Color = TypesTextColor.White,
                Layout = ViewModel.Instance.Heating ? TypesLayoutCard.Success : TypesLayoutCard.Info
            });

            grid.Add(1, new ControlCardCounter(this)
            {
                Text = "Gesammteinschaltdauer Scheinwerfer",
                Value = converter.Convert(ViewModel.Instance.LightingCounter, typeof(string), null, null).ToString(),
                Icon = "fas fa-lightbulb",
                Color = ViewModel.Instance.Lighting ? TypesTextColor.Success : TypesTextColor.Info
            });

            grid.Add(1, new ControlCardCounter(this)
            {
                Text = "Gesammteinschaltdauer Heizung",
                Value = converter.Convert(ViewModel.Instance.HeatingCounter, typeof(string), null, null).ToString(),
                Icon = "fas fa-fire",
                Color = ViewModel.Instance.Heating ? TypesTextColor.Success : TypesTextColor.Info
            });

            grid.Add(2, new ControlCardCounter(this)
            {
                Text = "Programmlauf",
                Value = converter.Convert(ViewModel.Instance.ProgramCounter, typeof(string), null, null).ToString(),
                Icon = "fas fa-stopwatch",
                Color = TypesTextColor.Info
            });

            grid.Add(3, new ControlPanel(this));

            Main.Content.Add(grid);

            Main.Content.Add(new ControlButtonReset(this));
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
