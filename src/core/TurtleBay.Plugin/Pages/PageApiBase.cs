using System;
using System.Collections.Generic;
using TurtleBay.Plugin.Model;
using WebExpress.Pages;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TurtleBay.Plugin.Pages
{
    public class PageApiBase : PageApi
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageApiBase()
            : base()
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

            var api = new API()
            { 
                Temperature = ViewModel.Instance.PrimaryTemperature.ToString(),
                Lighting = ViewModel.Instance.Lighting.ToString(),
                Heating = ViewModel.Instance.Heating.ToString(),
                LightingCounter = converter.Convert(ViewModel.Instance.Statistic.LightingCounter, typeof(string), null, null).ToString(),
                HeatingCounter = converter.Convert(ViewModel.Instance.Statistic.HeatingCounter, typeof(string), null, null).ToString(),
                Status = ViewModel.Instance.Status.ToString(),
                ProgramCounter = converter.Convert(ViewModel.Instance.ProgramCounter, typeof(string), null, null).ToString(),
                Now = ViewModel.Instance.Now,
                Min = ViewModel.Instance.Min.ToString(),
                Max = ViewModel.Instance.Settings.Max.ToString()
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            Content = JsonSerializer.Serialize(api, options);
        }
    }
}
