﻿using System.Linq;
using TurtleBay.Plugin.Model;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageDS18B201 : PageBase
    {
        /// <summary>
        /// Liefert oder setzt die Form
        /// </summary>
        private ControlFormular Form { get; set; }

        /// <summary>
        /// Liefert oder setzt den primären Sensor
        /// </summary>
        private ControlFormularItemInputComboBox PrimaryIDCtrl { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageDS18B201()
            : base("DS18B201")
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Init()
        {
            base.Init();

            Form = new ControlFormular()
            {
                Name = "Settings",
                EnableCancelButton = false,
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Three)
            };

            PrimaryIDCtrl = new ControlFormularItemInputComboBox()
            {
                Name = "primary",
                Label = "Primärer Sensor:"
            };

            // Werte festlegen
            foreach (var v in ViewModel.Instance.Temperature.Keys)
            {
                PrimaryIDCtrl.Items.Add(new ControlFormularItemInputComboBoxItem()
                {
                    Text = string.Format("{0}", v),
                    Value = string.Format("{0}", v)
                });
            }

            Form.Add(PrimaryIDCtrl);

            Form.InitFormular += (s, e) =>
            {
                PrimaryIDCtrl.Value = ViewModel.Instance.Settings.PrimaryID;
            };

            Form.ProcessFormular += (s, e) =>
            {
                ViewModel.Instance.Settings.PrimaryID = PrimaryIDCtrl.Value;
                ViewModel.Instance.SaveSettings();
            };

            PrimaryIDCtrl.Validation += (s, e) =>
            {
                var id = e.Value;

                if (!ViewModel.Instance.Temperature.Keys.Contains(id))
                {
                    e.Results.Add(new ValidationResult()
                    {
                        Text = "Ungültiger Wert",
                        Type = TypesInputValidity.Error
                    });
                }
            };
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();

            foreach (var v in ViewModel.Instance.Temperature)
            {
                Main.Content.Add(new ControlText() { Text = string.Format("Aktuelle Temperatur des Sensors {0}: {1} °C", v.Key, v.Value) });
            }

            Main.Content.Add(new ControlPanelCard(Form)
            {
                Header = "Einstellungen",
                BackgroundColor = new PropertyColorBackground(TypeColorBackground.Light),
                Margin = new PropertySpacingMargin(PropertySpacing.Space.Three)
            });
        }

        /// <summary>
        /// In String konvertieren
        /// </summary>
        /// <returns>Das Objekt als String</returns>
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
