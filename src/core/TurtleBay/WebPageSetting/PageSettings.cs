using TurtleBay.WebControl;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebAttribute;
using WebExpress.WebApp.WebPage;
using WebExpress.WebApp.WebSettingPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebResource
{
    [Id("Setting")]
    [Title("turtlebay:turtlebay.settings.label")]
    [Segment("settings", "turtlebay:turtlebay.settings.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [SettingSection(SettingSection.Preferences)]
    [SettingIcon(TypeIcon.Wrench)]
    [SettingGroup("webexpress.webapp:setting.tab.general.label")]
    [SettingContext("webexpress.webapp:setting.general.label")]
    [Context("setting")]
    public sealed class PageSettings : PageWebAppSetting, IPageSetting
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSettings()
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
        /// <param name="context">Der Kontext zum Rendern der Seite</param>
        public override void Process(RenderContextWebApp context)
        {
            base.Process(context);

            context.VisualTree.Content.Primary.Add(new ControlTabSettings()
            {

            });

            context.VisualTree.Content.Primary.Add(new ControlFormDayNight()
            {

            });
        }
    }
}
