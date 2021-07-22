using TurtleBay.WebControl;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("SettingSocket2")]
    [Title("turtlebay.setting.socket2.label")]
    [Segment("socket2", "turtlebay.setting.socket2.label")]
    [Path("/Setting")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("setting")]
    public sealed class PageSettingsSocket2 : PageTemplateWebApp, IPageSetting
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSettingsSocket2()
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

            Content.Primary.Add(new ControlFormSocket2()
            {

            });
        }
    }
}
