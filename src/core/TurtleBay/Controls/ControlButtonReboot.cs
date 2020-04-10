using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Controls
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
            Icon = Icon.PowerOff;
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
                    Icon = Icon.PowerOff,
                    Class = "m-1",
                    Layout = TypesLayoutButton.Danger,
                    OnClick = "window.location.href = '/reboot'"
                }
            );
        }
    }
}
