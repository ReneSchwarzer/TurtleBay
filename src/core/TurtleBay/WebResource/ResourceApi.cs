using System.Text.Json;
using TurtleBay.Model;
using WebExpress.Message;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebResource
{
    [Id("API")]
    [Segment("api")]
    [Path("/")]
    [Module("TurtleBay")]
    public sealed class ResourceApi : WebExpress.WebResource.ResourceRest
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
        /// <param name="context">Der Kontext</param>
        public override void Initialization(IResourceContext context)
        {
            base.Initialization(context);
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        /// <param name="request">Die Anfrage</param>
        /// <returns>Ein Objekt welches mittels JsonSerializer serialisiert werden kann.</returns>
        public override object GetData(Request request)
        {
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

            return JsonSerializer.Serialize(api, options);
        }
    }
}
