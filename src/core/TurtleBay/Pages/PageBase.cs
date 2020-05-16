﻿using System.Collections.Generic;
using TurtleBay.Plugin.Controls;
using TurtleBay.Plugin.Model;
using WebExpress.Html;
using WebExpress.UI.Controls;
using WebExpress.UI.Pages;

namespace TurtleBay.Plugin.Pages
{
    public class PageBase : PageTemplateWebApp
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="title">Der Titel der Seite</param>
        public PageBase(string title)
            : base()
        {
            Title = "TurtleBay";

            if (!string.IsNullOrWhiteSpace(title))
            {
                Title += " - " + title;
            }
            
            Favicons.Add(new Favicon("/Assets/img/Favicon.png", TypeFavicon.PNG));
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Init()
        {
            base.Init();

            base.Init();
            Head.Styles = new List<string>(new[] { "position: sticky; top: 0; z-index: 99;" });
            Head.Content.Add(HamburgerMenu);
            HamburgerMenu.HorizontalAlignment = TypeHorizontalAlignment.Left;
            HamburgerMenu.Image = Uri?.Root.Append("Assets/img/Logo.png");
            HamburgerMenu.Add(new ControlLink(this) { Text = "Home", Icon = new PropertyIcon(TypeIcon.Home), Uri = Uri.Root });
            HamburgerMenu.Add(new ControlLink(this) { Text = "Verlauf", Icon = new PropertyIcon(TypeIcon.ChartBar), Uri = Uri.Root.Append("history") });
            HamburgerMenu.AddSeperator();
            HamburgerMenu.Add(new ControlLink(this) { Text = "Logging", Icon = new PropertyIcon(TypeIcon.Book), Uri = Uri.Root.Append("log") });
            HamburgerMenu.Add(new ControlLink(this) { Text = "Einstellungen", Icon = new PropertyIcon(TypeIcon.Cog), Uri = Uri.Root.Append("settings") });
            HamburgerMenu.AddSeperator();
            HamburgerMenu.Add(new ControlLink(this) { Text = "Hilfe", Icon = new PropertyIcon(TypeIcon.InfoCircle), Uri = Uri.Root.Append("help") });

            // SideBar
            ToolBar = new ControlToolBar(this)
            {
                BackgroundColor = new PropertyColorBackground("#553322"),
                HorizontalAlignment = TypeHorizontalAlignment.Left
            };
            ToolBar.Classes.Add("sidebar");

            Head.Content.Add(new ControlPanelCenter(this, new ControlText(this)
            {
                Text = Title,
                TextColor = new PropertyColorText(TypeColorText.White),
                Format = TypeFormatText.H1,
                Size = new PropertySizeText(TypeSizeText.Default),
                Padding = new PropertySpacingPadding(PropertySpacing.Space.One),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.None, PropertySpacing.Space.None, PropertySpacing.Space.None, PropertySpacing.Space.Null),
                Styles = new List<string>(new[] { "font-size:190%; height: 50px;" })
            })); ;

            Main.Classes.Add("content");
            PathCtrl.Classes.Add("content");

            Main.Content.Add(new ControlTabMenu(this));
            Main.Content.Add(new ControlLine(this));

            Foot.Content.Add(new ControlText(this, "now")
            {
                Text = string.Format("{0}", ViewModel.Instance.Now),
                TextColor = new PropertyColorText(TypeColorText.Muted),
                Format = TypeFormatText.Center,
                Size = new PropertySizeText(TypeSizeText.Small)
            });
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();


        }
    }
}
