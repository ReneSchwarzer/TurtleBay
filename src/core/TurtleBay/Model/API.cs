using System;
using System.Collections.Generic;
using System.Text;

namespace TurtleBay.Model
{
    public class API
    {
        /// <summary>
        /// Liefert oder setzt die primäre Temperatur
        /// </summary>
        public string Temperature { get; set;}

        /// <summary>
        /// Liefert oder setzt den Einschaltzustand des Scheinwerfers
        /// </summary>
        public string Lighting { get; set; }
        
        /// <summary>
        /// Liefert oder setzt den Einschaltzustand der Heizung
        /// </summary>
        public string Heating { get; set; }

        /// <summary>
        /// Liefert oder setzt die Einschaltdauer des Scheinwerfers
        /// </summary>
        public string LightingCounter { get; set; }

        /// <summary>
        /// Liefert oder setzt die Einschaltdauer der Heizung
        /// </summary>
        public string HeatingCounter { get; set; }

        /// <summary>
        /// Liefert oder setzt den Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Liefert oder setzt die Programmablaufdauer
        /// </summary>
        public string ProgramCounter { get; set; }

        /// <summary>
        /// Liefert oder setzt die aktuelle Zeit
        /// </summary>
        public string Now { get; set; }

        /// <summary>
        /// Liefert oder setzt die minimale Temperatur
        /// </summary>
        public string Min { get; set; }

        /// <summary>
        /// Liefert oder setzt die maximale Temperatur
        /// </summary>
        public string Max { get; set; }
    }
}
