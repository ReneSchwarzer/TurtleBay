using System.Linq;
using TurtleBay.Plugin.Controls;
using TurtleBay.Plugin.Model;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageDashboard : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageDashboard()
            : base("Überblick")
        {
            HeaderScriptLinks.Add("/Assets/js/dashboard.js");
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

            var grid = new ControlPanelGrid() { Fluid = TypePanelContainer.Fluid };

            var layout = TypeColorBackground.Success;
            var temp = ViewModel.Instance.PrimaryTemperature;

            if (double.IsNaN(temp))
            {
                layout = TypeColorBackground.Danger;
            }
            else if(temp < ViewModel.Instance.Min)
            {
                layout = TypeColorBackground.Warning;
            }
            else if (temp > ViewModel.Instance.Settings.Max)
            {
                layout = TypeColorBackground.Danger;
            }

            grid.Content.Add(new ControlCardCounter("temperature")
            {
                Text = "Aktuelle Temperatur",
                Value = string.Format("{0} °C", temp.ToString("0.0")),
                Icon = new PropertyIcon(TypeIcon.ThermometerQuarter),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(layout),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.None, PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)

            });

            grid.Content.Add(new ControlCardCounter("lighting")
            {
                Text = "Scheinwerfer",
                Value = ViewModel.Instance.Lighting ? "An" : "Aus",
                Icon = new PropertyIcon(TypeIcon.Lightbulb),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(ViewModel.Instance.Heating ? TypeColorBackground.Success : TypeColorBackground.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.None, PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            grid.Content.Add(new ControlCardCounter("heating")
            {
                Text = "Heizung",
                Value = ViewModel.Instance.Heating ? "An" : "Aus",
                Icon = new PropertyIcon(TypeIcon.Fire),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(ViewModel.Instance.Heating ? TypeColorBackground.Success : TypeColorBackground.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.None, PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            grid.Content.Add(new ControlCardCounter("lc")
            {
                Text = "Gesammteinschaltdauer Scheinwerfer",
                Value = converter.Convert(ViewModel.Instance.Statistic.LightingCounter, typeof(string), null, null).ToString(),
                Icon = new PropertyIcon(TypeIcon.Lightbulb),
                TextColor = new PropertyColorText(ViewModel.Instance.Lighting ? TypeColorText.Success : TypeColorText.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.None, PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            grid.Content.Add(new ControlCardCounter("hc")
            {
                Text = "Gesammteinschaltdauer Heizung",
                Value = converter.Convert(ViewModel.Instance.Statistic.HeatingCounter, typeof(string), null, null).ToString(),
                Icon = new PropertyIcon(TypeIcon.Fire),
                TextColor = new PropertyColorText(ViewModel.Instance.Heating ? TypeColorText.Success : TypeColorText.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.None, PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            grid.Content.Add(new ControlCardCounter("pc")
            {
                Text = "Programmlauf",
                Value = converter.Convert(ViewModel.Instance.ProgramCounter, typeof(string), null, null).ToString(),
                Icon = new PropertyIcon(TypeIcon.Stopwatch),
                TextColor = new PropertyColorText(TypeColorText.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.None, PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            grid.Content.Add(new ControlPanel());

            Main.Content.Add(grid);

            Main.Content.Add(new ControlButtonReset());
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
