using System;
using System.Xml.Serialization;

namespace TurtleBay.Model
{
    public class ChartData
    {
        /// <summary>
        /// Lefert odder setzt die Zeit
        /// </summary>
        [XmlElement("time")]
        public DateTime Time { get; set; }

        /// <summary>
        /// Liefert oder setzt die Temperatur
        /// </summary>
        [XmlElement("temperature")]
        public int Temperature { get; set; }

        /// <summary>
        /// Liefert oder setzt Zeit indem die Heizung an war
        /// </summary>
        [XmlElement("heatingcount")]
        public int HeatingCount { get; set; }

        /// <summary>
        /// Liefert oder setzt Zeit indem der Scheinwerfer an war
        /// </summary>
        [XmlElement("lightingcount")]
        public int LightingCount { get; set; }
    }
}
