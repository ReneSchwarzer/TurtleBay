
using TurtleBay.Model;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("Socket1")]
    [Title("turtlebay.socket1.label")]
    [Segment("socket1", "turtlebay.socket1.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("socket")]
    public sealed class PageSocket1 : PageTemplateWebApp
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSocket1()
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

            ViewModel.Instance.Socket1Switch = !ViewModel.Instance.Socket1Switch;

            Redirecting(Uri.Root);
        }
    }
}
