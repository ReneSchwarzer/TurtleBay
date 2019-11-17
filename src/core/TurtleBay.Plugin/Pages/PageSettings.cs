using System;
using TurtleBay.Plugin.Model;
using WebExpress.UI.Controls;

namespace TurtleBay.Plugin.Pages
{
    public sealed class PageSettings : PageBase
    {
        /// <summary>
        /// Liefert oder setzt die Form
        /// </summary>
        private ControlPanelFormular Form { get; set; }

        /// <summary>
        /// Liefert oder setzt das Tagesbeginn
        /// </summary>
        private ControlFormularItemComboBox DayFromCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt das Tagesende
        /// </summary>
        private ControlFormularItemComboBox DayTillCtrl { get; set; }

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
        /// Liefert oder setzt die Startzeit 1 der UVB-Lampe
        /// </summary>
        private ControlFormularItemComboBox FromCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Startzeit 2 der UVB-Lampe
        /// </summary>
        private ControlFormularItemComboBox From2Ctrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Endzeit 1 der UVB-Lampe
        /// </summary>
        private ControlFormularItemComboBox TillCtrl { get; set; }

        /// <summary>
        /// Liefert oder setzt die Endzeit 2 der UVB-Lampe
        /// </summary>
        private ControlFormularItemComboBox Till2Ctrl { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        public PageSettings()
            : base("Einstellungen")
        {
        }

        /// <summary>
        /// Initialisierung
        /// </summary>
        public override void Init()
        {
            base.Init();

            Form = new ControlPanelFormular(this)
            {
                Name = "Settings",
                EnableCancelButton = false,
                Class = "m-3"
            };

            DayFromCtrl = new ControlFormularItemComboBox(Form)
            {
                Name = "DayFrom",
                Label = "Tagesanfang:"
            };

            DayTillCtrl = new ControlFormularItemComboBox(Form)
            {
                Name = "DayTill",
                Label = "Tagesende:"
            };

            NightMinCtrl = new ControlFormularItemComboBox(Form)
            {
                Name = "NightMin",
                Label = "Nachttemperatur Minimum:"
            };

            DayMinCtrl = new ControlFormularItemComboBox(Form)
            {
                Name = "DayMin",
                Label = "Tagestemperatur Minimum:"
            };

            MaxCtrl = new ControlFormularItemComboBox(Form)
            {
                Name = "Max",
                Label = "Temperatur Maximum:"
            };

            FromCtrl = new ControlFormularItemComboBox(Form)
            {
                Name = "From",
                Label = "Erste Startzeit des Scheinwerfers:"
            };

            TillCtrl = new ControlFormularItemComboBox(Form)
            {
                Name = "Till",
                Label = "Erste Endzeit des Scheinwerfers:"
            };

            From2Ctrl = new ControlFormularItemComboBox(Form)
            {
                Name = "From2",
                Label = "Zweite Startzeit des Scheinwerfers:"
            };

            Till2Ctrl = new ControlFormularItemComboBox(Form)
            {
                Name = "Till2",
                Label = "Zweite Endzeit des Scheinwerfers:"
            };

            // Werte festlegen
            for (var i = 0; i < 24; i++)
            {
                DayFromCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                DayTillCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                FromCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                From2Ctrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                TillCtrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });

                Till2Ctrl.Items.Add(new ControlFormularItemComboBoxItem()
                {
                    Text = string.Format("{0} Uhr", i),
                    Value = string.Format("{0}", i)
                });
            }

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

            Form.Add(DayFromCtrl);
            Form.Add(DayTillCtrl);
            Form.Add(NightMinCtrl);
            Form.Add(DayMinCtrl);
            Form.Add(MaxCtrl);
            Form.Add(FromCtrl);
            Form.Add(TillCtrl);
            Form.Add(From2Ctrl);
            Form.Add(Till2Ctrl);

            Form.InitFormular += (s, e) =>
            {
                DayFromCtrl.Value = ViewModel.Instance.Settings.DayFrom.ToString();
                DayTillCtrl.Value = ViewModel.Instance.Settings.DayTill.ToString();
                NightMinCtrl.Value = ViewModel.Instance.Settings.NightMin.ToString();
                DayMinCtrl.Value = ViewModel.Instance.Settings.DayMin.ToString();
                MaxCtrl.Value = ViewModel.Instance.Settings.Max.ToString();
                FromCtrl.Value = ViewModel.Instance.Settings.From.ToString();
                TillCtrl.Value = ViewModel.Instance.Settings.Till.ToString();
                From2Ctrl.Value = ViewModel.Instance.Settings.From2.ToString();
                Till2Ctrl.Value = ViewModel.Instance.Settings.Till2.ToString();
            };

            Form.ProcessFormular += (s, e) =>
            {
                ViewModel.Instance.Settings.DayFrom = Convert.ToInt32(DayFromCtrl.Value);
                ViewModel.Instance.Settings.DayTill = Convert.ToInt32(DayTillCtrl.Value);
                ViewModel.Instance.Settings.NightMin = Convert.ToInt32(NightMinCtrl.Value);
                ViewModel.Instance.Settings.DayMin = Convert.ToInt32(DayMinCtrl.Value);
                ViewModel.Instance.Settings.Max = Convert.ToInt32(MaxCtrl.Value);
                ViewModel.Instance.Settings.From = Convert.ToInt32(FromCtrl.Value);
                ViewModel.Instance.Settings.Till = Convert.ToInt32(TillCtrl.Value);
                ViewModel.Instance.Settings.From2 = Convert.ToInt32(From2Ctrl.Value);
                ViewModel.Instance.Settings.Till2 = Convert.ToInt32(Till2Ctrl.Value);
                ViewModel.Instance.SaveSettings();
            };

            DayFromCtrl.Validation += (s, e) =>
            {
                try
                {
                    var dayfrom = Convert.ToInt32(e.Value);
                    var daytill = Convert.ToInt32(DayTillCtrl.Value);

                    if (dayfrom < 0 || dayfrom > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (dayfrom > daytill)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Der Tagesanfang darf nicht nach dem Tagesende liegen",
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

            DayTillCtrl.Validation += (s, e) =>
            {
                try
                {
                    var dayfrom = Convert.ToInt32(DayFromCtrl.Value);
                    var daytill = Convert.ToInt32(e.Value);

                    if (daytill < 0 || daytill > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (dayfrom > daytill)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Das Tagesende darf nicht vor dem Tagesbeginn liegen",
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

            FromCtrl.Validation += (s, e) =>
            {
                try
                {
                    var from = Convert.ToInt32(e.Value);
                    var till = Convert.ToInt32(TillCtrl.Value);

                    if (from < 0 || from > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (from > till)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Der erste Startzeitpunkt des Scheinwerfers darf nicht nach dem ersten Ende liegen",
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

            TillCtrl.Validation += (s, e) =>
            {
                try
                {
                    var from = Convert.ToInt32(FromCtrl.Value);
                    var till = Convert.ToInt32(e.Value);

                    if (till < 0 || till > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (from > till)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Das erste Ende darf nicht vor dem ersten Startzeitpunkt des Scheinwerfers liegen",
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

            From2Ctrl.Validation += (s, e) =>
            {
                try
                {
                    var from = Convert.ToInt32(e.Value);
                    var till = Convert.ToInt32(Till2Ctrl.Value);

                    if (from < 0 || from > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (from > till)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Der zweite Startzeitpunkt des Scheinwerfers darf nicht nach dem zweiten Ende liegen",
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

            Till2Ctrl.Validation += (s, e) =>
            {
                try
                {
                    var from = Convert.ToInt32(From2Ctrl.Value);
                    var till = Convert.ToInt32(e.Value);

                    if (till < 0 || till > 24)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Ungültiger Wert",
                            Type = TypesInputValidity.Error
                        });
                    }

                    if (from > till)
                    {
                        e.Results.Add(new ValidationResult()
                        {
                            Text = "Das zweite Ende darf nicht vor dem zweiten Startzeitpunkt des Scheinwerfers liegen",
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
        }

        /// <summary>
        /// Verarbeitung
        /// </summary>
        public override void Process()
        {
            base.Process();

            Main.Content.Add(new ControlCard(this, Form)
            {
                Header = "Einstellungen",
                Layout = TypesLayoutCard.Light,
                Class = "m-3"
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
