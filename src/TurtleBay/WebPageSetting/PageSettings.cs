using TurtleBay.WebControl;
using WebExpress.UI.WebAttribute;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebPage;
using WebExpress.WebApp.WebSettingPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;
using WebExpress.WebScope;

namespace TurtleBay.WebResource
{
    [Title("turtlebay:turtlebay.settings.label")]
    [Segment("settings", "turtlebay:turtlebay.settings.label")]
    [ContextPath("/")]
    [Module<Module>]
    [SettingSection(SettingSection.Preferences)]
    [SettingIcon(TypeIcon.Wrench)]
    [SettingGroup("webexpress.webapp:setting.tab.general.label")]
    [SettingContext("webexpress.webapp:setting.general.label")]
    public sealed class PageSettings : PageWebAppSetting, IPageSetting, IScope
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
