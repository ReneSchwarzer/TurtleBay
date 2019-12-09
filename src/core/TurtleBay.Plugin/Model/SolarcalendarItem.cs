using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TurtleBay.Plugin.Model
{
    public class SolarcalendarItem
    {
        /// <summary>
        /// Liefert oder setzt den Tag des Jahres
        /// </summary>
        [XmlAttribute("day")]
        public int Day { get; set; }

        /// <summary>
        /// Liefert oder setzt den Sonnenaufgang
        /// </summary>
        [XmlAttribute("sunrise")]
        public string Sunrise { get; set; }

        /// <summary>
        /// Liefert oder setzt den Sonnenuntergang
        /// </summary>
        [XmlAttribute("sunset")]
        public string Sunset { get; set; }
    }
}
