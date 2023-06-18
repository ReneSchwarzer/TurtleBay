﻿using TurtleBay.WebControl;
using WebExpress.UI.WebAttribute;
using WebExpress.WebApp.WebPage;
using WebExpress.WebApp.WebSettingPage;
using WebExpress.WebAttribute;
using WebExpress.WebResource;
using WebExpress.WebScope;

namespace TurtleBay.WebResource
{
    [Title("turtlebay:turtlebay.setting.lighting.label")]
    [Segment("lighting", "turtlebay:turtlebay.setting.lighting.label")]
    [ContextPath("/")]
    [Parent<PageSettings>]
    [Module<Module>]
    [SettingHide()]
    [SettingContext("webexpress.webapp:setting.general.label")]
    public sealed class PageSettingsLighting : PageWebAppSetting, IPageSetting, IScope
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSettingsLighting()
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

            context.VisualTree.Content.Primary.Add(new ControlTabSettings()
            {

            });

            context.VisualTree.Content.Primary.Add(new ControlFormLighting()
            {

            });
        }
    }
}