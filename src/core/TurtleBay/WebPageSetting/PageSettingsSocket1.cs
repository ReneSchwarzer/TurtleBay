using TurtleBay.WebControl;
using WebExpress.WebApp.WebAttribute;
using WebExpress.WebApp.WebPage;
using WebExpress.WebApp.WebSettingPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebResource
{
    [Id("SettingSocket1")]
    [Title("turtlebay:turtlebay.setting.socket1.label")]
    [Segment("socket1", "turtlebay:turtlebay.setting.socket1.label")]
    [Path("/Setting")]
    [Module("TurtleBay")]
    [Context("setting")]
    [SettingHide()]
    [SettingContext("webexpress.webapp:setting.general.label")]
    public sealed class PageSettingsSocket1 : PageWebAppSetting, IPageSetting
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSettingsSocket1()
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

            context.VisualTree.Content.Primary.Add(new ControlFormSocket1()
            {

            });
        }
    }
}
