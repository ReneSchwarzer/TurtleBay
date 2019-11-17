using TurtleBay.Plugin.Model;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageReset : PageBase
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageReset()
            : base("Reset")
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Init()
        {
            base.Init();
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();

            ViewModel.Instance.ResetCounter();

            Redirecting("/");
        }

        /// <summary>
        /// In String konvertieren
        /// </summary>
        /// <returns>Das Objekt als String</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
