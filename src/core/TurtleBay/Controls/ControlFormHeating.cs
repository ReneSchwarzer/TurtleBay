using System;
using TurtleBay.Plugin.Model;
using WebExpress.Pages;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Controls
{
    public class ControlFormHeating : ControlPanelFormular
    {
        /// <summary>
        /// Liefert oder setzt die minimale Nachttemperatur
        /// </summary>
        private ControlFormularItemComboBox NightMinCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die minimale Tagestemperatur
        /// </summary>
        private ControlFormularItemComboBox DayMinCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Maximaltemperatur
        /// </summary>
        private ControlFormularItemComboBox MaxCtrl { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public ControlFormHeating(IPage page)
            : base(page, "heating")
        {
            Init();
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public void Init()
        {
            Name = "heating";
            EnableCancelButton = false;
            Class = "m-3";

            NightMinCtrl = new ControlFormularItemComboBox(this)
            {
                Name = "NightMin",
                Label = "Nachttemperatur Minimum:"
            };

            DayMinCtrl = new ControlFormularItemComboBox(this)
            {
                Name = "DayMin",
                Label = "Tagestemperatur Minimum:"
            };

            MaxCtrl = new ControlFormularItemComboBox(this)
            {
                Name = "Max",
                Label = "Temperatur Maximum:"
            };

            // Werte festlegen
            // Werte festlegen
            for (var i = 0; i < 40; i++)
            {
                NightMinCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} °C", i),
                    Value = string.Format("{0}", i)
                });

                DayMinCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} °C", i),
                    Value = string.Format("{0}", i)
                });

                MaxCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} °C", i),
                    Value = string.Format("{0}", i)
                });
            }

            Add(NightMinCtrl);
            Add(DayMinCtrl);
            Add(MaxCtrl);

            InitFormular += (s, e) =>
            {
                NightMinCtrl.Value = ViewModel.Instance.Settings.NightMin.ToString();
                DayMinCtrl.Value = ViewModel.Instance.Settings.DayMin.ToString();
                MaxCtrl.Value = ViewModel.Instance.Settings.Max.ToString();
            };

            ProcessFormular += (s, e) =>
            {
                ViewModel.Instance.Settings.NightMin = Convert.ToInt32(NightMinCtrl.Value);
                ViewModel.Instance.Settings.DayMin = Convert.ToInt32(DayMinCtrl.Value);
                ViewModel.Instance.Settings.Max = Convert.ToInt32(MaxCtrl.Value);
                ViewModel.Instance.SaveSettings();
            };

            NightMinCtrl.Validation += (s, e) =>
            {
                try
                {
                    var max = Convert.ToInt32(MaxCtrl.Value);
                    var nightMin = Convert.ToInt32(e.Value);

                    if (nightMin < 0)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Die Nachttemperatur sollte nicht im Frostbereich liegen",
                            Type = TypesInputValidity.Warning
                        });
                    }
                    else if (nightMin > 40)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Die Nachttemperatur ist zu hoch eingestellt",
                            Type = TypesInputValidity.Warning
                        });
                    }

                    if (nightMin > max)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Die Nachttemperatur liegt über die Maximaltemperatur",
                            Type = TypesInputValidity.Error
                        });
                    }
                }
                catch (Exception ex)
                {
                    e.Results.Add(new ValidationResult()
                    {
                        Text = ex.Message,
                        Type = TypesInputValidity.Error
                    });
                }
            };

            DayMinCtrl.Validation += (s, e) =>
            {
                try
                {
                    var max = Convert.ToInt32(MaxCtrl.Value);
                    var dayMin = Convert.ToInt32(e.Value);

                    if (dayMin < 0)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Die Tagestemperatur sollte nicht im Frostbereich liegen",
                            Type = TypesInputValidity.Warning
                        });
                    }
                    else if (dayMin > 40)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Die Tagestemperatur ist zu hoch eingestellt",
                            Type = TypesInputValidity.Warning
                        });
                    }

                    if (dayMin > max)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Die Tagestemperatur liegt über die Maximaltemperatur",
                            Type = TypesInputValidity.Error
                        });
                    }
                }
                catch (Exception ex)
                {
                    e.Results.Add(new ValidationResult()
                    {
                        Text = ex.Message,
                        Type = TypesInputValidity.Error
                    });
                }
            };

            MaxCtrl.Validation += (s, e) =>
            {
                try
                {
                    var max = Convert.ToInt32(e.Value);

                    if (max < 1)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Das Maximum darf nicht im Frostbereich liegen",
                            Type = TypesInputValidity.Error
                        });
                    }
                    else if (max > 40)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Das Maximum ist zu hoch eingestellt",
                            Type = TypesInputValidity.Warning
                        });
                    }
                }
                catch (Exception ex)
                {
                    e.Results.Add(new ValidationResult()
                    {
                        Text = ex.Message,
                        Type = TypesInputValidity.Error
                    });
                }
            };
        }
    }
}
