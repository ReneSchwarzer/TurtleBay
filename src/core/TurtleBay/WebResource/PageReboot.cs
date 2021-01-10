
using TurtleBay.Model;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("Reboot")]
    [Title("turtlebay.reboot.label")]
    [Segment("reboot", "turtlebay.reboot.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("reboot")]
    public sealed class PageReboot : PageTemplateWebApp
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageReboot()
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

            Content.Primary.Add
            (
                new ControlPanelCenter
                (
                    new ControlImage()
                    {
                        Uri = Uri.Root.Append("assets/img/reboot.png"),
                        Width = 200
                    },
                    new ControlText()
                    {
                        Text = "Der Rechner wird neu gestartet! Bitte warten Sie einen Augenblick.",
                        TextColor = new PropertyColorText(TypeColorText.Danger)
                    }
                    ,
                    new ControlText()
                    {
                        Text = "...",
                        TextColor = new PropertyColorText(TypeColorText.Primary)
                    }
                )
            );

            ViewModel.Instance.Reboot();
        }
    }
}
