using TurtleBay.WebControl;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("SettingHeating")]
    [Title("turtlebay.setting.heating.label")]
    [Segment("heating", "turtlebay.setting.heating.label")]
    [Path("/Setting")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("setting")]
    public sealed class PageSettingsHeating : PageTemplateWebApp, IPageSetting
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

            Content.Primary.Add(new ControlFormHeating()
            {

            });
        }
    }
}
