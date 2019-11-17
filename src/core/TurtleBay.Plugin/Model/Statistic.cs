using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TurtleBay.Plugin.Model
{
    [XmlRoot(ElementName = "statistic", IsNullable = false)]
    public class Statistic
    {
        /// <summary>
        /// Liefert oder setzt den Zähler für die Heizung
        /// </summary>
        [XmlIgnore]
        public TimeSpan HeatingCounter
        {
            get
            {
                try
                {
                    var ts = TimeSpan.Parse(HeatingCounterString);
                    return ts;
                }
                catch
                {
                    return new TimeSpan();
                }
            }
            set
            {
                if (HeatingCounter != value)
                {
                    HeatingCounterString = value.ToString();
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt den Zähler für den Scheinwerfer
        /// </summary>
        [XmlElement("heatingcounter")]
        public string HeatingCounterString { get; set; }

        /// <summary>
        /// Liefert oder setzt den Zähler für die UVB-Lampe
        /// </summary>
        [XmlIgnore]
        public TimeSpan LightingCounter
        {
            get
            {
                try
                {
                    var ts = TimeSpan.Parse(LightingCounterString);
                    return ts;
                }
                catch
                {
                    return new TimeSpan();
                }
            }
            set
            {
                if (LightingCounter != value)
                {
                    LightingCounterString = value.ToString();
                }
            }
        }

        /// <summary>
        /// Liefert oder setzt den Zähler für den Scheinwerfer
        /// </summary>
        [XmlElement("lightingcounter")]
        public string LightingCounterString { get; set; }

        /// <summary>
        /// Liefert oder setzt die Diagrammdaten
        /// </summary>
        [XmlElement("chart")]
        public List<ChartData> Chart24h { get; set; } = new List<ChartData>();
    }
}
