
using TurtleBay.Model;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("Socket2")]
    [Title("turtlebay.socket2.label")]
    [Segment("socket2", "turtlebay.socket2.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("socket")]
    public sealed class PageSocket2 : PageTemplateWebApp
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSocket2()
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

            ViewModel.Instance.Socket2Switch = !ViewModel.Instance.Socket2Switch;

            Redirecting(Uri.Root);
        }
    }
}
