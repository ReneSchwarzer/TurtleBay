using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TurtleBay.Model
{
    [XmlRoot(ElementName = "settings", IsNullable = false)]
    public class Settings
    {
        /// <summary>
        /// Liefert oder setzt den Debug-Modus
        /// </summary>
        [XmlElement(ElementName = "debug", DataType = "boolean")]
        public bool DebugMode { get; set; }

        /// <summary>
        /// Setzt oder liefert die Minimaltemperatur in der Nacht
        /// </summary>
        [XmlElement(ElementName = "nightmin", DataType = "int")]
        public int NightMin { get; set; }

        /// <summary>
        /// Setzt oder liefert die Minimaltemperatur am Tag
        /// </summary>
        [XmlElement(ElementName = "daymin", DataType = "int")]
        public int DayMin { get; set; }

        /// <summary>
        /// Setzt oder liefert die Maximaltemperatur
        /// </summary>
        [XmlElement(ElementName = "max", DataType = "int")]
        public int Max { get; set; }

        /// <summary>
        /// Setzt oder liefert die Startzeit
        /// </summary>
        [XmlElement(ElementName = "from", DataType = "int")]
        public int From { get; set; }

        /// <summary>
        /// Setzt oder liefert die Endzeit
        /// </summary>
        [XmlElement(ElementName = "till", DataType = "int")]
        public int Till { get; set; }

        /// <summary>
        /// Setzt oder liefert die Startzeit
        /// </summary>
        [XmlElement(ElementName = "from2", DataType = "int")]
        public int From2 { get; set; }

        /// <summary>
        /// Setzt oder liefert die Endzeit
        /// </summary>
        [XmlElement(ElementName = "till2", DataType = "int")]
        public int Till2 { get; set; }

        /// <summary>
        /// Setzt oder liefert die Startzeit des Tages
        /// </summary>
        [XmlElement(ElementName = "dayfrom", DataType = "int")]
        public int DayFrom { get; set; }

        /// <summary>
        /// Setzt oder liefert die Tagesendzeit
        /// </summary>
        [XmlElement(ElementName = "daytill", DataType = "int")]
        public int DayTill { get; set; }

        /// <summary>
        /// Liefert oder setzt die PrimäreID
        /// </summary>
        [XmlElement("primary")]
        public string PrimaryID { get; set; }

    }
}
