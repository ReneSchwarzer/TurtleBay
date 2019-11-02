using System;

namespace TurtleBayNet.Plugin.Model
{
    public class ChartData
    {
        /// <summary>
        /// Lefert odder setzt die Zeit
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Liefert oder setzt die Temperatur
        /// </summary>
        public int Temperature { get; set; }

        /// <summary>
        /// Liefert oder setzt Zeit indem die Heizung an war
        /// </summary>
        public int HeatingCount { get; set; }

        /// <summary>
        /// Liefert oder setzt Zeit indem der Scheinwerfer an war
        /// </summary>
        public int LightingCount { get; set; }
    }
}
