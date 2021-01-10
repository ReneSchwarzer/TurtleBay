using TurtleBay.Model;
using TurtleBay.WebControl;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;
using WebExpress.Uri;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("Home")]
    [Title("turtlebay.dashboard.label")]
    [Segment("", "turtlebay.dashboard.label")]
    [Path("")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("dashboard")]
    public sealed class PageDashboard : PageTemplateWebApp, IPageDashboard
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageDashboard()
        {
           
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Initialization()
        {
            base.Initialization();

            Favicons.Add(new Favicon(Uri.Root.Append("/assets/img/Favicon.png").ToString(), TypeFavicon.PNG));
            HeaderScriptLinks.Add(Uri.Root.Append("/assets/js/dashboard.js"));
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();

            var converter = new TimeSpanConverter();

            var layout = TypeColorBackground.Success;
            var temp = ViewModel.Instance.PrimaryTemperature;

            if (double.IsNaN(temp))
            {
                layout = TypeColorBackground.Danger;
            }
            else if (temp < ViewModel.Instance.Min)
            {
                layout = TypeColorBackground.Warning;
            }
            else if (temp > ViewModel.Instance.Settings.Max)
            {
                layout = TypeColorBackground.Danger;
            }

            var flexboxTop = new ControlPanelFlexbox() 
            {
                Layout = TypeLayoutFlexbox.Default,
                Justify = TypeJustifiedFlexbox.Center,
                Align = TypeAlignFlexbox.Stretch,
                Wrap = TypeWrap.Wrap
            };

            var flexboxMiddle = new ControlPanelFlexbox()
            {
                Layout = TypeLayoutFlexbox.Default,
                Justify = TypeJustifiedFlexbox.Center,
                Align = TypeAlignFlexbox.Stretch,
                Wrap = TypeWrap.Wrap
            };

            var flexboxBottom = new ControlPanelFlexbox()
            {
                Layout = TypeLayoutFlexbox.Default,
                Justify = TypeJustifiedFlexbox.Center,
                Align = TypeAlignFlexbox.Stretch,
                Wrap = TypeWrap.Wrap
            };

            flexboxTop.Content.Add(new ControlCardCounter("temperature")
            {
                Text = this.I18N("turtlebay.dashboard.temperature.current.label"),
                Value = string.Format("{0} °C", temp.ToString("0.0")),
                Icon = new PropertyIcon(TypeIcon.ThermometerQuarter),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(layout),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)

            });

            flexboxTop.Content.Add(new ControlCardCounter("lighting")
            {
                Text = this.I18N("turtlebay.dashboard.lighting.label"),
                Value = ViewModel.Instance.Lighting ? this.I18N("turtlebay.dashboard.lighting.on") : this.I18N("turtlebay.dashboard.lighting.off"),
                Icon = new PropertyIcon(TypeIcon.Lightbulb),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(ViewModel.Instance.Heating ? TypeColorBackground.Success : TypeColorBackground.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            flexboxTop.Content.Add(new ControlCardCounter("heating")
            {
                Text = this.I18N("turtlebay.dashboard.heating.label"),
                Value = ViewModel.Instance.Heating ? this.I18N("turtlebay.dashboard.heating.on") : this.I18N("turtlebay.dashboard.heating.off"),
                Icon = new PropertyIcon(TypeIcon.Fire),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(ViewModel.Instance.Heating ? TypeColorBackground.Success : TypeColorBackground.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            flexboxMiddle.Content.Add(new ControlCardCounter("lc")
            {
                Text = this.I18N("turtlebay.dashboard.lighting.total"),
                Value = converter.Convert(ViewModel.Instance.Statistic.LightingCounter, typeof(string), null, null).ToString(),
                Icon = new PropertyIcon(TypeIcon.Lightbulb),
                TextColor = new PropertyColorText(ViewModel.Instance.Lighting ? TypeColorText.Success : TypeColorText.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            flexboxMiddle.Content.Add(new ControlCardCounter("hc")
            {
                Text = this.I18N("turtlebay.dashboard.heating.total"),
                Value = converter.Convert(ViewModel.Instance.Statistic.HeatingCounter, typeof(string), null, null).ToString(),
                Icon = new PropertyIcon(TypeIcon.Fire),
                TextColor = new PropertyColorText(ViewModel.Instance.Heating ? TypeColorText.Success : TypeColorText.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            flexboxBottom.Content.Add(new ControlCardCounter("pc")
            {
                Text = this.I18N("turtlebay.dashboard.run"),
                Value = converter.Convert(ViewModel.Instance.ProgramCounter, typeof(string), null, null).ToString(),
                Icon = new PropertyIcon(TypeIcon.Stopwatch),
                TextColor = new PropertyColorText(TypeColorText.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            Content.Preferences.Add(flexboxTop);
            Content.Primary.Add(flexboxMiddle);
            Content.Primary.Add(flexboxBottom);

            Content.Secondary.Add(new ControlButtonReset());
        }
    }
}
