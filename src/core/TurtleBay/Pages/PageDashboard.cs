﻿using System.Linq;
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

            var grid = new ControlGrid(this) { Fluid = true };

            var layout = TypesLayoutCard.Success;
            var temp = ViewModel.Instance.PrimaryTemperature;

            if (double.IsNaN(temp))
            {
                layout = TypesLayoutCard.Danger;
            }
            else if(temp < ViewModel.Instance.Min)
            {
                layout = TypesLayoutCard.Warning;
            }
            else if (temp > ViewModel.Instance.Settings.Max)
            {
                layout = TypesLayoutCard.Danger;
            }

            grid.Add(0, new ControlCardCounter(this, "temperature")
            {
                Text = "Aktuelle Temperatur",
                Value = string.Format("{0} °C", temp.ToString("0.0")),
                Icon = Icon.ThermometerQuarter,
                Color = TypesTextColor.White,
                Layout = layout
            });

            grid.Add(0, new ControlCardCounter(this, "lighting")
            {
                Text = "Scheinwerfer",
                Value = ViewModel.Instance.Lighting ? "An" : "Aus",
                Icon = Icon.Lightbulb,
                Color = TypesTextColor.White,
                Layout = ViewModel.Instance.Lighting ? TypesLayoutCard.Success : TypesLayoutCard.Info
            });

            grid.Add(0, new ControlCardCounter(this, "heating")
            {
                Text = "Heizung",
                Value = ViewModel.Instance.Heating ? "An" : "Aus",
                Icon = Icon.Fire,
                Color = TypesTextColor.White,
                Layout = ViewModel.Instance.Heating ? TypesLayoutCard.Success : TypesLayoutCard.Info
            });

            grid.Add(1, new ControlCardCounter(this, "lc")
            {
                Text = "Gesammteinschaltdauer Scheinwerfer",
                Value = converter.Convert(ViewModel.Instance.Statistic.LightingCounter, typeof(string), null, null).ToString(),
                Icon = Icon.Lightbulb,
                Color = ViewModel.Instance.Lighting ? TypesTextColor.Success : TypesTextColor.Info
            });

            grid.Add(1, new ControlCardCounter(this, "hc")
            {
                Text = "Gesammteinschaltdauer Heizung",
                Value = converter.Convert(ViewModel.Instance.Statistic.HeatingCounter, typeof(string), null, null).ToString(),
                Icon = Icon.Fire,
                Color = ViewModel.Instance.Heating ? TypesTextColor.Success : TypesTextColor.Info
            });

            grid.Add(2, new ControlCardCounter(this, "pc")
            {
                Text = "Programmlauf",
                Value = converter.Convert(ViewModel.Instance.ProgramCounter, typeof(string), null, null).ToString(),
                Icon = Icon.Stopwatch,
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
