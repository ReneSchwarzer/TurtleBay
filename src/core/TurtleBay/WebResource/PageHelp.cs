using System.Reflection;
using TurtleBay.WebControl;
using WebExpress.Attribute;
using WebExpress.Html;
using WebExpress.Internationalization;
using WebExpress.UI.WebControl;
using WebExpress.Uri;
using WebExpress.WebApp.WebResource;

namespace TurtleBay.WebResource
{
    [ID("Help")]
    [Title("turtlebay.help.label")]
    [Segment("help", "turtlebay.help.label")]
    [Path("/")]
    [Module("TurtleBay")]
    [Context("general")]
    [Context("help")]
    public sealed class PageHelp : PageTemplateWebApp, IPageHelp
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
        public override void Initialization()
        {
            base.Initialization();

            Favicons.Add(new Favicon(Uri.Root.Append("/assets/img/Favicon.png").ToString(), TypeFavicon.PNG));
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
                        Uri = Uri.Root.Append("assets/img/turtle.png"),
                        Width = 200
                    },
                    new ControlText()
                    {
                        Text = this.I18N("turtlebay.help.version.label"),
                        TextColor = new PropertyColorText(TypeColorText.Primary)
                    },
                    new ControlText()
                    {
                        Text = string.Format("{0}", Context.Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion),
                        TextColor = new PropertyColorText(TypeColorText.Dark)
                    },
                    new ControlText()
                    {
                        Text = this.I18N("turtlebay.help.contact.label"),
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

            Content.Primary.Add(new ControlLine());

            Content.Primary.Add(new ControlText()
            {
                Text = this.I18N("turtlebay.help.privacypolicy.label"),
                Format = TypeFormatText.H4
            });

            Content.Primary.Add(new ControlText()
            {
                Text = this.I18N("turtlebay.help.privacypolicy.description"),
                Format = TypeFormatText.Paragraph
            });

            Content.Primary.Add(new ControlText()
            {
                Text = this.I18N("turtlebay.help.disclaimer.label"),
                Format = TypeFormatText.H4
            });

            Content.Primary.Add(new ControlText()
            {
                Text = this.I18N("turtlebay.help.disclaimer.description"),
                Format = TypeFormatText.Paragraph
            });

        }
    }
}
