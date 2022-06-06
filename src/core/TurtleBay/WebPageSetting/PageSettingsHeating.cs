using TurtleBay.WebControl;
using WebExpress.WebApp.WebAttribute;
using WebExpress.WebApp.WebPage;
using WebExpress.WebApp.WebSettingPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebResource
{
    [ID("SettingHeating")]
    [Title("turtlebay:turtlebay.setting.heating.label")]
    [Segment("heating", "turtlebay:turtlebay.setting.heating.label")]
    [Path("/Setting")]
    [Module("TurtleBay")]
    [Context("setting")]
    [SettingHide()]
    [SettingContext("webexpress.webapp:setting.general.label")]
    public sealed class PageSettingsHeating : PageWebAppSetting, IPageSetting
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSettingsHeating()
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

            context.VisualTree.Content.Primary.Add(new ControlFormHeating()
            {

            });
        }
    }
}
