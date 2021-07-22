using TurtleBay.WebControl;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("SettingSocket1")]
    [Title("turtlebay.setting.socket1.label")]
    [Segment("socket1", "turtlebay.setting.socket1.label")]
    [Path("/Setting")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("setting")]
    public sealed class PageSettingsSocket1 : PageTemplateWebApp, IPageSetting
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
        public override void Initialization()
        {
            base.Initialization();

            Favicons.Add(new Favicon(Uri.Root.Append("/Assets/img/Favicon.png").ToString(), TypeFavicon.PNG));
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();

            Content.Primary.Add(new ControlTabSettings()
            {

            });

            Content.Primary.Add(new ControlFormSocket1()
            {

            });
        }
    }
}
