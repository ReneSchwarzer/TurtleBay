
using TurtleBay.Model;
using WebExpress.UI.WebControl;
using WebExpress.WebApp.WebPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebPage
{
    [Id("Reboot")]
    [Title("turtlebay:turtlebay.reboot.label")]
    [Segment("reboot", "turtlebay:turtlebay.reboot.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("reboot")]
    public sealed class PageReboot : PageWebApp
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

            context.VisualTree.Content.Primary.Add
            (
                new ControlPanelCenter
                (
                    new ControlImage()
                    {
                        Uri = context.Request.Uri.Root.Append("assets/img/reboot.png"),
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
