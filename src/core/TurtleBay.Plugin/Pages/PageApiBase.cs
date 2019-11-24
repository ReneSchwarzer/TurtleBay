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

            var api = new API()
            { 
                Temperature = ViewModel.Instance.PrimaryTemperature.ToString(),
                Lighting = ViewModel.Instance.Lighting.ToString(),
                Heating = ViewModel.Instance.Heating.ToString(),
                LightingCounter = ViewModel.Instance.Statistic.LightingCounter.ToString(),
                HeatingCounter = ViewModel.Instance.Statistic.HeatingCounter.ToString(),
                Status = ViewModel.Instance.Status.ToString(),
                ProgramCounter = ViewModel.Instance.ProgramCounter.ToString(),
                Now = DateTime.Now.ToString()
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            Content = JsonSerializer.Serialize(api, options);
        }
    }
}
