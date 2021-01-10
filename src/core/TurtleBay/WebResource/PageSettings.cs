using TurtleBay.WebControl;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("Setting")]
    [Title("turtlebay.settings.label")]
    [Segment("settings", "turtlebay.settings.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("setting")]
    public sealed class PageSettings : PageTemplateWebApp, IPageSetting
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

            Content.Primary.Add(new ControlFormDayNight()
            {

            });
        }
    }
}
