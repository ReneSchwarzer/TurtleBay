using System.Text.Json;
using TurtleBay.Model;
using WebExpress.Attribute;
using WebExpress.WebResource;

namespace TurtleBay.WebResource
{
    [ID("API")]
    [Segment("api")]
    [Path("/")]
    [Module("TurtleBay")]
    public sealed class ResourceApi : WebExpress.WebResource.ResourceApi
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public ResourceApi()
            : base()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();
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
                Socket1 = (ViewModel.Instance.Socket1 || ViewModel.Instance.Socket1Switch).ToString(),
                Socket2 = (ViewModel.Instance.Socket2 || ViewModel.Instance.Socket2Switch).ToString(),
                LightingCounter = converter.Convert(ViewModel.Instance.Statistic.LightingCounter, typeof(string), null, null).ToString(),
                HeatingCounter = converter.Convert(ViewModel.Instance.Statistic.HeatingCounter, typeof(string), null, null).ToString(),
                Status = ViewModel.Instance.Status.ToString(),
                ProgramCounter = converter.Convert(ViewModel.Instance.ProgramCounter, typeof(string), null, null).ToString(),
                Now = ViewModel.Now,
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
