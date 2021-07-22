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

        /// <summary>
        /// Liefert oder setzt die Einstellungen des Scheinwerfers
        /// </summary>
        public Lighting Lighting { get; set; } = new Lighting();

        /// <summary>
        /// Liefert oder setzt die Einstellungen des Scheinwerfers
        /// </summary>
        public Socket1 Socket1 { get; set; } = new Socket1();

        /// <summary>
        /// Liefert oder setzt die Einstellungen des Scheinwerfers
        /// </summary>
        public Socket2 Socket2 { get; set; } = new Socket2();
    }
}
