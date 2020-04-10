using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TurtleBay.Plugin.Model
{
    [XmlRoot(ElementName = "solarcalendar", IsNullable = false)]
    public class Solarcalendar
    {
        /// <summary>
        /// Liefert die Items
        /// </summary>
        [XmlElement("item")]
        public List<SolarcalendarItem> Items { get; set; }
    }
}
