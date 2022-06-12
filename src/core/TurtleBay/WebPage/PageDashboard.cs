using TurtleBay.Model;
using TurtleBay.WebControl;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebPage
{
    [Id("Home")]
    [Title("turtlebay:turtlebay.dashboard.label")]
    [Segment("", "turtlebay:turtlebay.dashboard.label")]
    [Path("")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("dashboard")]
    public sealed class PageDashboard : PageWebApp, IPageDashboard
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
        /// <param name="context">Der Kontext</param>
        public override void Initialization(IResourceContext context)
        {
            base.Initialization(context);

            //HeaderScriptLinks.Add(Uri.Root.Append("/assets/js/dashboard.js"));
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        /// <param name="context">Der Kontext zum Rendern der Seite</param>
        public override void Process(RenderContextWebApp context)
        {
            base.Process(context);

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

            var flexboxSwitch = new ControlPanelFlexbox()
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
                Text = "turtlebay:turtlebay.dashboard.temperature.current.label",
                Value = string.Format("{0} °C", temp.ToString("0.0")),
                Icon = new PropertyIcon(TypeIcon.ThermometerQuarter),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(layout),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)

            });

            flexboxTop.Content.Add(new ControlCardCounter("lighting")
            {
                Text = "turtlebay:turtlebay.dashboard.lighting.label",
                Value = ViewModel.Instance.Lighting ? "turtlebay:turtlebay.dashboard.lighting.on" : "turtlebay:turtlebay.dashboard.lighting.off",
                Icon = new PropertyIcon(TypeIcon.Lightbulb),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(ViewModel.Instance.Heating ? TypeColorBackground.Success : TypeColorBackground.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            flexboxTop.Content.Add(new ControlCardCounter("heating")
            {
                Text = "turtlebay:turtlebay.dashboard.heating.label",
                Value = ViewModel.Instance.Heating ? "turtlebay:turtlebay.dashboard.heating.on" : "turtlebay:turtlebay.dashboard.heating.off",
                Icon = new PropertyIcon(TypeIcon.Fire),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(ViewModel.Instance.Heating ? TypeColorBackground.Success : TypeColorBackground.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            flexboxSwitch.Content.Add(new ControlCardCounter("socket1")
            {
                Text = string.IsNullOrWhiteSpace(ViewModel.Instance.Settings.Socket1.Name) ? "turtlebay:turtlebay.dashboard.socket1.label" : ViewModel.Instance.Settings.Socket1.Name,
                Value = ViewModel.Instance.Socket1 || ViewModel.Instance.Socket1Switch ? "turtlebay:turtlebay.dashboard.socket1.on" : "turtlebay:turtlebay.dashboard.socket1.off",
                Icon = new PropertyIcon(TypeIcon.Plug),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(ViewModel.Instance.Socket1 || ViewModel.Instance.Socket1Switch ? TypeColorBackground.Success : TypeColorBackground.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            flexboxSwitch.Content.Add(new ControlButtonLink()
            {
                Text = ViewModel.Instance.Socket1 || ViewModel.Instance.Socket1Switch ? "turtlebay:turtlebay.dashboard.socket1.off" : "turtlebay:turtlebay.dashboard.socket1.on",
                Uri = context.Request.Uri.Root.Append("socket1"),
                BackgroundColor = new PropertyColorButton(TypeColorButton.Secondary),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Null, PropertySpacing.Space.Two)
            });

            flexboxSwitch.Content.Add(new ControlCardCounter("socket2")
            {
                Text = string.IsNullOrWhiteSpace(ViewModel.Instance.Settings.Socket2.Name) ? "turtlebay:turtlebay.dashboard.socket2.label" : ViewModel.Instance.Settings.Socket2.Name,
                Value = ViewModel.Instance.Socket2 || ViewModel.Instance.Socket2Switch ? "turtlebay:turtlebay.dashboard.socket2.on" : "turtlebay:turtlebay.dashboard.socket2.off",
                Icon = new PropertyIcon(TypeIcon.Plug),
                TextColor = new PropertyColorText(TypeColorText.White),
                BackgroundColor = new PropertyColorBackground(ViewModel.Instance.Socket2 || ViewModel.Instance.Socket2Switch ? TypeColorBackground.Success : TypeColorBackground.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            flexboxSwitch.Content.Add(new ControlButtonLink()
            {
                Text = ViewModel.Instance.Socket2 || ViewModel.Instance.Socket2Switch ? "turtlebay:turtlebay.dashboard.socket2.off" : "turtlebay:turtlebay.dashboard.socket2.on",
                Uri = context.Request.Uri.Root.Append("socket2"),
                BackgroundColor = new PropertyColorButton(TypeColorButton.Secondary),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Null, PropertySpacing.Space.Two)
            });

            flexboxMiddle.Content.Add(new ControlCardCounter("lc")
            {
                Text = "turtlebay:turtlebay.dashboard.lighting.total",
                Value = converter.Convert(ViewModel.Instance.Statistic.LightingCounter, typeof(string), null, null).ToString(),
                Icon = new PropertyIcon(TypeIcon.Lightbulb),
                TextColor = new PropertyColorText(ViewModel.Instance.Lighting ? TypeColorText.Success : TypeColorText.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            flexboxMiddle.Content.Add(new ControlCardCounter("hc")
            {
                Text = "turtlebay:turtlebay.dashboard.heating.total",
                Value = converter.Convert(ViewModel.Instance.Statistic.HeatingCounter, typeof(string), null, null).ToString(),
                Icon = new PropertyIcon(TypeIcon.Fire),
                TextColor = new PropertyColorText(ViewModel.Instance.Heating ? TypeColorText.Success : TypeColorText.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            flexboxBottom.Content.Add(new ControlCardCounter("pc")
            {
                Text = "turtlebay:turtlebay.dashboard.run",
                Value = converter.Convert(ViewModel.Instance.ProgramCounter, typeof(string), null, null).ToString(),
                Icon = new PropertyIcon(TypeIcon.Stopwatch),
                TextColor = new PropertyColorText(TypeColorText.Info),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Two),
                GridColumn = new PropertyGrid(TypeDevice.Auto, 2)
            });

            context.VisualTree.Content.Preferences.Add(flexboxTop);
            context.VisualTree.Content.Primary.Add(flexboxSwitch);
            context.VisualTree.Content.Primary.Add(flexboxMiddle);
            context.VisualTree.Content.Primary.Add(flexboxBottom);

            context.VisualTree.Content.Secondary.Add(new ControlButtonReset());
        }
    }
}
