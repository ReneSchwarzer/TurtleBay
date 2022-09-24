using TurtleBay.Model;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebAttribute;
using WebExpress.WebApp.WebPage;
using WebExpress.WebApp.WebSettingPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebPageSetting
{
    [Id("DS18B201")]
    [Title("turtlebay:turtlebay.ds18b201.label")]
    [Segment("ds18b201", "turtlebay:turtlebay.ds18b201.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("setting")]
    [Context("ds18b201")]
    [SettingSection(SettingSection.Preferences)]
    [SettingIcon(TypeIcon.Microchip)]
    [SettingGroup("webexpress.webapp:setting.tab.general.label")]
    [SettingContext("webexpress.webapp:setting.general.label")]
    public sealed class PageDS18B201 : PageWebAppSetting, IPageDS18B201
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
        /// <param name="context">Der Kontext</param>
        public override void Initialization(IResourceContext context)
        {
            base.Initialization(context);

            Form = new ControlFormular("170eddfe-6fcf-4f60-923f-58132d34b343")
            {
                Name = "Settings",
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Three)
            };

            PrimaryIDCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "primary",
                Label = "turtlebay:turtlebay.ds18b201.primary.label"
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

                if (!ViewModel.Instance.Temperature.ContainsKey(id))
                {
                    e.Results.Add(new ValidationResult(TypesInputValidity.Error, this.I18N("turtlebay.ds18b201.validation.invalid")));
                }
            };
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        /// <param name="context">Der Kontext zum Rendern der Seite</param>
        public override void Process(RenderContextWebApp context)
        {
            base.Process(context);

            foreach (var v in ViewModel.Instance.Temperature)
            {
                context.VisualTree.Content.Preferences.Add(new ControlText() { Text = string.Format(this.I18N("turtlebay:turtlebay.ds18b201.current"), v.Key, v.Value) });
            }

            context.VisualTree.Content.Primary.Add(new ControlPanelCard(Form)
            {
                Header = "turtlebay:turtlebay.settings.label",
                BackgroundColor = new PropertyColorBackground(TypeColorBackground.Light),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Three)
            });
        }
    }
}
