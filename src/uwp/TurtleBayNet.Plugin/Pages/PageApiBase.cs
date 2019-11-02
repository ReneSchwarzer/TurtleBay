using System;
using System.Collections.Generic;
using TurtleBayNet.Plugin.Model;
using WebExpress.Pages;

namespace TurtleBayNet.Plugin.Pages
{
    public class PageApiBase : PageApi
    {
        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageApiBase()
            : base()
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

            var lines = new List<string>();
            var subLines = new List<string>();

            Action<string, string> a = (name, value) =>
            {
                subLines.Add(string.Format("  \"{0}\": \"{1}\"", name, value));
            };

            a("Temperature", ViewModel.Instance.Temperature.ToString());
            a("Lighting", ViewModel.Instance.Lighting.ToString());
            a("Heating", ViewModel.Instance.Heating.ToString());
            a("LightingCounter", ViewModel.Instance.LightingCounter.ToString());
            a("HeatingCounter", ViewModel.Instance.HeatingCounter.ToString());
            a("Status", ViewModel.Instance.Status.ToString());
            a("ProgramCounter", ViewModel.Instance.ProgramCounter.ToString());
            a("Now", DateTime.Now.ToString());

            lines.Add("{");
            lines.Add(string.Join("," + Environment.NewLine + "  ", subLines));
            lines.Add("}");

            Content = string.Join(Environment.NewLine, lines);
        }
    }
}
