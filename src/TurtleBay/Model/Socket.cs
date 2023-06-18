using System.Collections.Generic;
using System.Xml.Serialization;

namespace TurtleBay.Model
{
    public abstract class Socket
    {
        /// <summary>
        /// Setzt oder liefert den Namen der Steckdose
        /// </summary>
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

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
    }
}
