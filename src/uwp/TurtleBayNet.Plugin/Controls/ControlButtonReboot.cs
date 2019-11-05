﻿using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBayNet.Plugin.Controls
{
    public class ControlButtonReboot : ControlButton
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="page">Die zugehörige Seite</param>
        public ControlButtonReboot(IPage page)
            : base(page)
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        private void Init()
        {
            Text = "Reboot";
            Class = "m-1";
            Icon = "fas fa-power-off";
            Layout = TypesLayoutButton.Danger;

            Modal = new ControlModal
            (
                Page,
                "reboot",
                "Neustart",
                new ControlText(Page)
                {
                    Text = "Möchten Sie wirklich den Rechner neu starten?"
                },
                new ControlButton(Page)
                {
                    Text = "Neustart",
                    Icon = "fas fa-power-off",
                    Class = "m-1",
                    Layout = TypesLayoutButton.Danger,
                    OnClick = "window.location.href = '/reboot'"
                }
            );
        }
    }
}