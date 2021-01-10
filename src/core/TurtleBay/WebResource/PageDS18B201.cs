using System.Linq;
using TurtleBay.Model;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("DS18B201")]
    [Title("turtlebay.ds18b201.label")]
    [Segment("ds18b201", "turtlebay.ds18b201.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("ds18b201")]
    public sealed class PageDS18B201 : PageTemplateWebApp, IPageDS18B201
    {
        /// <summary>
        /// Liefert oder setzt die Form
        /// </summary>
        private ControlFormular Form { get; set; }

        /// <summary>
        /// Liefert oder setzt den primären Sensor
        /// </summary>
        private ControlFormularItemInputComboBox PrimaryIDCtrl { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageDS18B201()
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();

            Favicons.Add(new Favicon(Uri.Root.Append("/assets/img/Favicon.png").ToString(), TypeFavicon.PNG));

            Form = new ControlFormular()
            {
                Name = "Settings",
                EnableCancelButton = false,
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Three)
            };

            PrimaryIDCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "primary",
                Label = this.I18N("turtlebay.ds18b201.primary.label")
            };

            // Werte festlegen
            foreach (var v in ViewModel.Instance.Temperature.Keys)
            {
                PrimaryIDCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("{0}", v),
                    Value = string.Format("{0}", v)
                });
            }

            Form.Add(PrimaryIDCtrl);

            Form.FillFormular += (s, e) =>
            {
                PrimaryIDCtrl.Value = ViewModel.Instance.Settings.PrimaryID;
            };

            Form.ProcessFormular += (s, e) =>
            {
                ViewModel.Instance.Settings.PrimaryID = PrimaryIDCtrl.Value;
                ViewModel.Instance.SaveSettings();
            };

            PrimaryIDCtrl.Validation += (s, e) =>
            {
                var id = e.Value;

                if (!ViewModel.Instance.Temperature.Keys.Contains(id))
                {
                    e.Results.Add(new ValidationResult()
                    {
                        Text = this.I18N("turtlebay.ds18b201.validation.invalid"),
                        Type = TypesInputValidity.Error
                    });
                }
            };
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();

            foreach (var v in ViewModel.Instance.Temperature)
            {
                Content.Preferences.Add(new ControlText() { Text = this.I18N(string.Format("turtlebay.ds18b201.current", v.Key, v.Value)) });
            }

            Content.Primary.Add(new ControlPanelCard(Form)
            {
                Header = this.I18N("turtlebay.settings.label"),
                BackgroundColor = new PropertyColorBackground(TypeColorBackground.Light),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Three)
            });
        }
    }
}
