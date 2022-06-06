using System.Reflection;
using TurtleBay.WebControl;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;
using WebExpress.Uri;
using WebExpress.WebApp.WebPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;

namespace TurtleBay.WebPage
{
    [ID("Help")]
    [Title("turtlebay:turtlebay.help.label")]
    [Segment("help", "turtlebay:turtlebay.help.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("help")]
    public sealed class PageHelp : PageWebApp, IPageHelp
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageHelp()
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
                        Uri = context.Request.Uri.Root.Append("assets/img/turtle.png"),
                        Width = 200
                    },
                    new ControlText()
                    {
                        Text = "turtlebay:turtlebay.help.version.label",
                        TextColor = new PropertyColorText(TypeColorText.Primary)
                    },
                    new ControlText()
                    {
                        Text = string.Format("{0}", Context.Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion),
                        TextColor = new PropertyColorText(TypeColorText.Dark)
                    },
                    new ControlText()
                    {
                        Text = "turtlebay:turtlebay.help.contact.label",
                        TextColor = new PropertyColorText(TypeColorText.Primary)
                    },
                    new ControlLink()
                    {
                        Text = string.Format("rene_schwarzer@hotmail.de"),
                        Uri = new UriAbsolute()
                        {
                            Scheme = UriScheme.Mailto,
                            Authority = new UriAuthority("rene_schwarzer@hotmail.de")
                        },
                        TextColor = new PropertyColorText(TypeColorText.Dark)
                    },
                    new ControlLine(),
                    new ControlButtonReboot()
                )
            );

            context.VisualTree.Content.Primary.Add(new ControlLine());

            context.VisualTree.Content.Primary.Add(new ControlText()
            {
                Text = "turtlebay:turtlebay.help.privacypolicy.label",
                Format = TypeFormatText.H4
            });

            context.VisualTree.Content.Primary.Add(new ControlText()
            {
                Text = "turtlebay:turtlebay.help.privacypolicy.description",
                Format = TypeFormatText.Paragraph
            });

            context.VisualTree.Content.Primary.Add(new ControlText()
            {
                Text = "turtlebay:turtlebay.help.disclaimer.label",
                Format = TypeFormatText.H4
            });

            context.VisualTree.Content.Primary.Add(new ControlText()
            {
                Text = "turtlebay:turtlebay.help.disclaimer.description",
                Format = TypeFormatText.Paragraph
            });

        }
    }
}
